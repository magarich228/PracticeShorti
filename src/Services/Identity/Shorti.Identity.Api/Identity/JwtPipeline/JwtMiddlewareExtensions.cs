using Shorti.Identity.Api.Data;

namespace Shorti.Identity.Api.Identity.JwtPipeline
{
    public static class JWTMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtContextAttach(this IApplicationBuilder app)
        {
            app.UseMiddleware<JWTMiddleware>();

            return app;
        }
    }
}
