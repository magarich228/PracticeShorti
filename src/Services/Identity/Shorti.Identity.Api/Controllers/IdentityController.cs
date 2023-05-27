using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shorti.Identity.Api.Data;
using Shorti.Identity.Api.Data.Models;
using Shorti.Identity.Api.Identity;
using Shorti.Identity.Api.Identity.Abstractions;
using Shorti.Identity.Api.Identity.Extensions;
using Shorti.Identity.Api.Services;
using Shorti.Shared.Contracts.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shorti.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IJwtIdentityManager _identityManager;
        private readonly ILogger<IdentityController> _logger;
        private readonly ShortiIdentityContext _db;
        private readonly HashService _hashService;

        public IdentityController(
            IJwtIdentityManager identityManager,
            ILogger<IdentityController> logger,
            ShortiIdentityContext db,
            HashService hashService)
        {
            _identityManager = identityManager;
            _logger = logger;
            _db = db;
            _hashService = hashService;
        }

        [HttpPost("signin")]
        public ActionResult<LoginResultDto> SignIn([FromBody] LoginDto loginRequest)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName == loginRequest.UserName);

            if (user == null)
            {
                return Unauthorized(loginRequest);
            }

            var result = _hashService.VerifyHashedPassword(user.PasswordHash, loginRequest.Password);

            if (!result)
            {
                return Unauthorized(loginRequest);
            }

            return Login(user);
        }

        [HttpPost("signup")]
        public async Task<ActionResult<LoginResultDto>> SignUp([FromBody] RegisterDto registerRequest)
        {
            var isExist = _db.Users.FirstOrDefault(u => u.UserName == registerRequest.UserName) != null;

            if (isExist)
            {
                ModelState.AddModelError("username", "Такой UserName занят.");
                return BadRequest(ModelState);
            }

            User newUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = registerRequest.UserName,
                PasswordHash = _hashService.HashPassword(registerRequest.Password),
                AvatarPath = "avatars/defaultuser.png"
            };

            _db.Users.Add(newUser);
            var rows = await _db.SaveChangesAsync();

            if (rows == 0)
            {
                return Problem(detail: "Ошибка при добавлении нового пользователя.");
            }

            return Login(newUser);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginResultDto>> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            try
            {
                var accessToken = HttpContext.Request.Headers["Authorization"]
                    .FirstOrDefault()?
                    .Split(" ")
                    .Last();

                if (string.IsNullOrWhiteSpace(request.RefreshToken) || accessToken == null)
                {
                    return Unauthorized();
                }

                var claims = _identityManager.DecodeJwtToken(accessToken).claims.Claims;
                var jwtResult = _identityManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);

                return Ok(await Task.FromResult(new LoginResultDto
                {
                    Id = claims.GetId(),
                    UserName = claims.GetUsername(),
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                }));
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?
                .Split(" ")
                .Last();

            if (token == null)
            {
                return Unauthorized();
            }

            await Task.Run(() =>
            {
                var decode = _identityManager.DecodeJwtToken(token);
                var claims = decode.claims.Claims;

                _identityManager.RemoveRefreshTokenByUser(
                    claims.GetId(),
                    claims.GetUsername());

                HttpContext.Response.Headers.Remove(IdentityConstants.AuthorizationHeaderName);
            });

            return Ok();
        }

        [NonAction]
        private ActionResult<LoginResultDto> Login(User user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var jwtResult = _identityManager.GenerateTokens(user.Id, user.UserName, claims, DateTime.Now);

            return Ok(new LoginResultDto
            {
                Id = user.Id,
                UserName = user.UserName,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }
    }
}
