using PetFamily.Api.Middleware;

namespace PetFamily.Api.Extensions;

public static class ExceptionMiddlewareExtentions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    } 
}