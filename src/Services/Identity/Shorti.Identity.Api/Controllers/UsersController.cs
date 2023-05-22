using Microsoft.AspNetCore.Mvc;
using Shorti.Identity.Api.Data;
using Shorti.Identity.Api.Identity.JwtPipeline;
using Shorti.Shared.Contracts.Identity;
using Shorti.Shared.Kernel;
using Shorti.Shared.Kernel.Abstractions;

namespace Shorti.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthorize]
    public class UsersController : ControllerBase
    {
        private readonly ShortiIdentityContext _db;
        private readonly IFileService _fileService;

        public UsersController(
            ShortiIdentityContext db,
            IFileService fileService)
        {
            _db = db;
            _fileService = fileService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById([FromRoute] Guid userId)
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
            string path = $"avatars/{Path.GetRandomFileName()}";

            var user = (User)HttpContext.Items["User"]!;

            if (user == null)
            {
                return Problem(detail: "Не найден пользователь.");
            }

            await _fileService.DownloadAsync(avatar, path);
            var isDownloaded = System.IO.File.Exists(Path.Combine(_fileService.FilePath, path));

            if (!isDownloaded)
            {
                return Problem(detail: "Ошибка загрузки аватара.");
            }

            user.AvatarPath = path;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return Ok(user.AvatarPath);
        }
    }
}
