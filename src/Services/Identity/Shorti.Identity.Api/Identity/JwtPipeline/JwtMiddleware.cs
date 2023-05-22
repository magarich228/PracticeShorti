using Shorti.Identity.Api.Data;
using Shorti.Identity.Api.Identity.Abstractions;
using Shorti.Identity.Api.Identity.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Shorti.Identity.Api.Identity.JwtPipeline
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IdentityConfiguration _identityConfiguration;
        private readonly IJwtIdentityManager _jwtIdentityManager;

        public JWTMiddleware(
            RequestDelegate next, 
            IdentityConfiguration identityConfiguration,
            IJwtIdentityManager jwtIdentityManager)
        {
            _next = next;
            _identityConfiguration = identityConfiguration;
            _jwtIdentityManager = jwtIdentityManager;
        }

        public async Task Invoke(HttpContext context, ShortiIdentityContext db)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await AttachAccountToContext(context, db, token);

            await _next(context);
        }

        private async Task AttachAccountToContext(HttpContext context, ShortiIdentityContext db, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_identityConfiguration.ServiceKey);

                var decode = _jwtIdentityManager.DecodeJwtToken(token);

                var jwtToken = decode.token;
                var userId = decode.claims.Claims.GetId();

                var user = await db.Users.FindAsync(new object[] { userId });

                if (user == null)
                {
                    throw new ApplicationException("Не найден пользователь токена.");
                }

                context.Items["User"] = user;
            }
            catch
            {
                throw new ApplicationException("Ошибка привязки токена к контексту.");
            }
        }
    }
}
