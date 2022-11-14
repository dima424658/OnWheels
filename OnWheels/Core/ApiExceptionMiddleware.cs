using System;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace OnWheels.Core;

public class ApiExceptionMiddleware
{
    private readonly ILogger Logger;
    private readonly RequestDelegate Next;

    public ApiExceptionMiddleware(ILogger<ApiExceptionMiddleware> logger, RequestDelegate next)
    {
        Logger = logger;
        Next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Call the next delegate/middleware in the pipeline.
            await Next(context);
        }
        catch (ApiException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            var error = new Dictionary<string, object?>();
            error.Add(nameof(ex.Message), ex.Message);
            if (ex.Details.Any())
                error.Add(nameof(ex.Details), ex.Details);

            await context.Response.WriteAsJsonAsync(error);

            Logger.LogError(ex, "ApiException thrown.");
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            var error = new Dictionary<string, object?>();
            error.Add(nameof(ex.Message), ex.Message);

            await context.Response.WriteAsJsonAsync(error);
        }
    }
}

public static class ApiExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseApiExceptionMiddleware(this IApplicationBuilder builder) =>
        builder.UseMiddleware<ApiExceptionMiddleware>();
}