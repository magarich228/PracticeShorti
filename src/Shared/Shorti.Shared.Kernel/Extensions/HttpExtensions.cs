using Microsoft.AspNetCore.Http;

namespace Shorti.Shared.Kernel.Extensions
{
    public static class HttpExtensions
    {
        public static string? GetToken(this HttpContext context) =>
            context.Request.Headers["Authorization"]
                .FirstOrDefault()?
                .Split(" ")
                .Last();
    }
}
