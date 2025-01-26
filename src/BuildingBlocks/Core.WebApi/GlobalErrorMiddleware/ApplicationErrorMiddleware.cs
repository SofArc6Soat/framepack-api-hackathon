using Microsoft.AspNetCore.Http;

namespace Core.WebApi.GlobalErrorMiddleware;

public class ApplicationErrorMiddleware
{
    private readonly RequestDelegate _next;

    public ApplicationErrorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Lógica de tratamento de erro
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
        }
    }
}