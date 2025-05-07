using PetFamily.Api.Response;
using Serilog;

namespace PetFamily.Api.Middleware;

public class ExceptionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            var responseError = new ResponseError("internal.server", e.Message, null);
            var envelope = Envelope.Error([responseError]);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            Log.Warning("! Exception: code {0}, message: {1}",
                responseError.ErrorCode, responseError.ErrorMessage);
            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}