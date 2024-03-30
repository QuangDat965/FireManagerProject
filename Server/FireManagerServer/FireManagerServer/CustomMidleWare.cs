using FireManagerServer.Common;
using FireManagerServer.Service.JwtService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

public class CustomMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IJwtService _jwtService = new JwtService(new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build());

    public CustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Code xử lý trước khi yêu cầu được chuyển đến endpoint xử lý chính
        Console.WriteLine("Custom Middleware: Before handling the request.");
        var authorizationHeader = context.Request.Headers["Authorization"].ToString();
        if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader.Substring("Bearer ".Length);
            var claims = _jwtService.VerifyToken(token);
            if (claims != null)
            {
                CurrentContext.userId = _jwtService.GetId(token);
            }
        }
        // Chuyển yêu cầu đến middleware tiếp theo trong pipeline
        await _next(context);

        // Code xử lý sau khi yêu cầu đã được xử lý bởi endpoint chính
        Console.WriteLine("Custom Middleware: After handling the request.");
    }
}

// Extension method để sử dụng middleware này trong ứng dụng
public static class CustomMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomMiddleware>();
    }
}
