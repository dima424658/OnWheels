using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using OnWheels.Core;
using OnWheels.Requests;
using OnWheels.Sql;

namespace OnWheels.Models;

public class UserModel
{
    private readonly SqlContext SqlContext;
    private readonly PasswordHasher<object?> PasswordHasher; // TODO: а сюда можно строку посылать???
    private readonly ISession? Session;

    public UserModel(SqlContext sqlContext, PasswordHasher<object?> passwordHasher, IHttpContextAccessor contextAccessor)
    {
        SqlContext = sqlContext;
        PasswordHasher = passwordHasher;
        Session = contextAccessor.HttpContext?.Session;
    }

    public bool TryGetCurrentUser(out int userId)
    {
        userId = CurrentUserId ?? 0;

        return CurrentUserId != null;
    }

    public int GetCurrentUser()
    {
        var userId = CurrentUserId;
        if (userId == null)
            throw new ApiException(StatusCodes.Status401Unauthorized, "Не удалось авторизоваться.");
        else
            return userId.Value;
    }

    public bool TryGetUserIdByEmail(string email, out int userId)
    {
        var dbUser = SqlContext.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        userId = dbUser?.Id ?? 0;

        return dbUser != null;
    }

    public int GetUserIdByEmail(string email)
    {
        if (!TryGetUserIdByEmail(email, out var userId))
            throw new ApiException($"Пользователь с эл. почтой {email} не найден.");

        return userId;
    }

    public bool TryGetUserEmailById(int userId, out string email)
    {
        var dbUser = SqlContext.Users.FirstOrDefault(u => u.Id == userId);
        email = dbUser?.Email ?? string.Empty;

        return dbUser != null;
    }

    public string GetUserEmailById(int userId)
    {
        if (!TryGetUserEmailById(userId, out var email))
            throw new ApiException($"Пользователь c идентификатором {userId} не найден.");

        return email;
    }

    public async Task CreateUserAsync(UserCreateRequest user, CancellationToken cancellationToken)
    {
        List<(string field, string error)> errors = new();

        var dbUser = SqlContext.Users.FirstOrDefault(u => user.Email.ToLower() == u.Email.ToLower());
        if (dbUser != null)
            throw new ApiException($"Пользователь с эл. почтой {user.Email} уже существует.");

        errors.AddRange(ValidateUserFormat(user));

        if (errors.Any())
            throw new ApiException("Не удалось создать пользователя.")
            {
                Details = errors.ToDictionary(x => x.field, x => (object?)x.error)
            };

        await SqlContext.Users.AddAsync(new()
        {
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.Email,
            Password = PasswordHasher.HashPassword(null, user.Password),
            City = user.City,
            Birthday = user.Birthday,
        }, cancellationToken);

        await SqlContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AuthorizeAsync(int userId, string password, CancellationToken cancellationToken)
    {
        var dbUser = GetSqlUser(userId);
        var result = PasswordHasher.VerifyHashedPassword(null, dbUser?.Password, password);
        if (dbUser != null && result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            dbUser.Password = PasswordHasher.HashPassword(null, password);
            SqlContext.Update(dbUser);
            await SqlContext.SaveChangesAsync(cancellationToken);
        }

        if (result == PasswordVerificationResult.Failed)
            throw new ApiException(StatusCodes.Status401Unauthorized, "Неверная почта или пароль.");
        else
            CurrentUserId = userId;
    }

    public async Task ChangePasswordAsync(int userId, string newPassword, CancellationToken cancellationToken)
    {
        var errors = ValidatePasswordFormat(newPassword).ToList();
        if (errors.Any())
            throw new ApiException("Не удалось изменить пароль")
            {
                Details = new() { { nameof(UserCreateRequest.Password), errors} }
            };

        var dbUser = GetSqlUser(userId);
        if (dbUser == null)
            throw new ApiException($"Не удалось найти пользователя {userId}.");
        
        dbUser.Password = PasswordHasher.HashPassword(null, newPassword);
        SqlContext.Users.Update(dbUser);

        await SqlContext.SaveChangesAsync(cancellationToken);
    }

    private int? CurrentUserId
    {
        get => Session?.GetInt32("userId");
        set
        {
            if (value == null)
                Session?.Remove("userId");
            else
                Session?.SetInt32("userId", value.Value);
        }
    }

    private static IEnumerable<(string field, string error)> ValidateUserFormat(UserCreateRequest user)
    {
        var errors = new List<(string, string)>();

        if (!Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            errors.Add((nameof(user.Email), "Эл. почта должна быть в формате local@domain.com"));

        if (user.Firstname.Length < 3 || user.Firstname.Length > 20)
            errors.Add((nameof(user.Firstname), "Имя должно быть от 3 до 20 символов."));

        if (user.Firstname.Length < 3 || user.Firstname.Length > 20)
            errors.Add((nameof(user.Firstname), "Фамилия должна быть от 3 до 20 символов."));

        var passwordErrors = ValidatePasswordFormat(user.Password);
        errors.AddRange(passwordErrors.Select(err => (nameof(user.Password), err)));

        return errors;
    }

    private static IEnumerable<string> ValidatePasswordFormat(string password)
    {
        var errors = new List<string>();

        if (password.Length < 8)
            errors.Add("Пароль должен содержать не менее 8 знаков.");

        if (!password.Any(x => char.IsDigit(x)))
            errors.Add("Пароль должен содержать цифры.");

        if (!password.Any(x => char.IsLetter(x)))
            errors.Add("Пароль должен содержать буквы.");

        return errors;
    }

    private Sql.Models.SqlUser? GetSqlUser(int userId) => SqlContext.Users.FirstOrDefault(u => u.Id == userId);
}