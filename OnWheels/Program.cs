using Microsoft.AspNetCore.Identity;
using OnWheels.Core;
using OnWheels.Models;
using OnWheels.Sql.Postgres;

namespace OnWheels;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureApplicationBuilder(builder);

        var application = builder.Build();
        ConfigureApplication(application);

        await application.RunAsync();
    }

    private static void ConfigureApplicationBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.AddSwaggerGen();

        builder.Services.Configure<PgOptions>(builder.Configuration.GetSection(PgOptions.SectionName));
        builder.Services.AddPostgresContext();

        builder.Services.AddSingleton<PasswordHasher<object?>>();
        builder.Services.AddScoped<UserModel>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromHours(1);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });
    }

    private static void ConfigureApplication(WebApplication application)
    {
        if (application.Environment.IsDevelopment())
        {
            application.UseSwagger();
            application.UseSwaggerUI();
        }

        if (application.Environment.IsProduction())
        {
            application.UseHttpsRedirection();
        }

        application.UseAuthorization();

        application.UseSession();

        application.MapControllers();

        application.UseApiExceptionMiddleware();

        application.Services.ApplyPostgresMigrations();
    }
}