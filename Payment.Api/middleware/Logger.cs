using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseLogger(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggerMiddleware>();
    }
}

class LoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggerMiddleware> _logger;

    public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    async Task LogRequestAsync(HttpContext context)
    {
        string responseString = $"Request: {context.Request?.Method} {context.Request?.Path.Value}";
        context.Request.EnableBuffering();
        string body = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;
        responseString += $"\nBody: {body}";
        _logger.LogInformation(responseString);
    }
    async Task LogResponseAsync(HttpContext context, Stream originalBody, MemoryStream newBody)
    {
        newBody.Seek(0, SeekOrigin.Begin);
        var bodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        newBody.Seek(0, SeekOrigin.Begin);
        await newBody.CopyToAsync(originalBody);
        _logger.LogInformation($"Response: { context.Response?.StatusCode} \nBody: {bodyText}");
    }

    public async Task Invoke(HttpContext context)
    {
        Task logRequest = LogRequestAsync(context);
        var originalBody = context.Response.Body;
        using var newBody = new MemoryStream();
        context.Response.Body = newBody;
        await _next(context);
        await logRequest;
        await LogResponseAsync(context, originalBody, newBody);
    }
}