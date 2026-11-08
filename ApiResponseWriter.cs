using Fcg.UsersAPI.Models;

namespace Fcg.UsersAPI;

public static class ApiResponseWriter
{
    public static Task WriteErrorAsync(HttpContext context, int statusCode, string message, IReadOnlyList<string>? errors = null)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsJsonAsync(BaseResponse.Failure(statusCode, message, errors));
    }
}
