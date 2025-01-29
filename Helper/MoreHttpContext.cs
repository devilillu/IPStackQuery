using Microsoft.AspNetCore.Http;
using System.Text;

namespace Helper;

public static class MoreHttpContext
{
    public static async Task WriteToBodyAsync(this HttpContext context, object message) =>
        await context.Response.Body.WriteAsync(
            Encoding.UTF8.GetBytes(message + Environment.NewLine));

    public static string RetrieveFromUri(this HttpContext context, string key) =>
        (context.Request.RouteValues.TryGetValue(key, out object? value) && value is string valueAsString)
        ? valueAsString
        : throw new InvalidProgramException();

    public async static Task<T?> ReadJsonAsync<T>(this HttpContext context) =>
        await context.Request.ReadFromJsonAsync<T>();
}

