using Microsoft.AspNetCore.Mvc;
using Shorti.Identity.Api.Data;
using Shorti.Identity.Api.Data.Models;
using Shorti.Identity.Api.Identity.Abstractions;
using Shorti.Identity.Api.Identity.Extensions;
using Shorti.Shared.Contracts.Identity;
using Shorti.Shared.Kernel;
using Shorti.Shared.Kernel.Abstractions;

namespace Shorti.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ShortiIdentityContext _db;
        private readonly IFileService _fileService;
        private readonly IJwtIdentityManager _jwtIdentityManager;

        public UsersController(
            ShortiIdentityContext db,
            IFileService fileService,
            IJwtIdentityManager jwtIdentityManager)
        {
            _db = db;
            _fileService = fileService;
            _jwtIdentityManager = jwtIdentityManager;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> GetById([FromRoute] Guid userId)
        {
            var user = await _db.Users.FindAsync(new object[] { userId });

            if (user == null)
            {
                return NotFound(userId);
            }

            var result = Mapping.Map<User, UserDto>(user);

            return Ok(result);
        }

        [HttpPut("avatarUpdate")]
        public async Task<IActionResult> UpdateAvatar(IFormFile avatar)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"]
                    .FirstOrDefault()?
                    .Split(" ")
                    .Last();

            if (accessToken == null)
            {
                return Unauthorized();
            }

            var user = await _db.Users.FindAsync(new object[]
            {
                _jwtIdentityManager.DecodeJwtToken(accessToken).claims.Claims.GetId()
            });

            if (user == null)
            {
                return Problem(detail: "Пользователь не найден.");
            }

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(avatar.FileName)}";

            await _fileService.DownloadAsync(avatar, fileName);
            var isDownloaded = System.IO.File.Exists(Path.Combine(_fileService.FilePath, fileName));

            if (!isDownloaded)
            {
                return Problem(detail: "Ошибка загрузки аватара.");
            }

            user.AvatarPath = $"avatars/{fileName}";

            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return Ok(user.AvatarPath);
        }

        [HttpGet("current")]
        public async Task<ActionResult<UserDto>> GetCurrentUserInfo()
        {
            var accessToken = HttpContext.Request.Headers["Authorization"]
                    .FirstOrDefault()?
                    .Split(" ")
                    .Last();

            if (accessToken == null)
            {
                return Unauthorized();
            }

            var user = await _db.Users.FindAsync(new object[]
            {
                _jwtIdentityManager.DecodeJwtToken(accessToken).claims.Claims.GetId()
            });

            if (user == null)
            {
                return Problem(detail: "Пользователь не найден.");
            }

            var result = Mapping.Map<User, UserDto>(user);

            return Ok(result);
        }
    }
}
