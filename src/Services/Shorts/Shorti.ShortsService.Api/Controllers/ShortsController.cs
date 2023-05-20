using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shorti.Shared.Contracts.Shorts;
using Shorti.Shared.Kernel.Abstractions;

namespace Shorti.ShortsService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShortsController : ControllerBase
    {
        private readonly IFileService _fileService;

        public ShortsController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromBody] NewShortVideoDto shortVideoDto)
        {
            string newFileName = Path.GetRandomFileName();

            await _fileService.DownloadAsync(shortVideoDto.File, newFileName);

            var result = System.IO.File.Exists(Path.Combine(_fileService.FilePath, newFileName));

            return Ok(result);
        }
    }
}
