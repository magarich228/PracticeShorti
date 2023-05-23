using Microsoft.AspNetCore.Mvc;
using Shorti.Shared.Contracts.Shorts;
using Shorti.Shared.Kernel;
using Shorti.Shared.Kernel.Abstractions;
using Shorti.Shared.Kernel.Filters;
using Shorti.ShortsService.Api.Data;
using Shorti.ShortsService.Api.Data.Models;

namespace Shorti.ShortsService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [JwtAuthorize]
    public class ShortsController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ShortsContext _db;

        public ShortsController(IFileService fileService, ShortsContext db)
        {
            _fileService = fileService;
            _db = db;
        }

        [HttpGet("{shortId}")]
        public async Task<IActionResult> GetShortById(Guid shortId)
        {
            var @short = await _db.Shorts.FindAsync(new object[] { shortId });
            
            if (@short == null)
            {
                return NotFound(shortId);
            }

            var result = Mapping.Map<ShortVideo, ShortVideoDto>(@short);
            
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] NewShortVideoDto shortVideoDto)
        {
            var validationResult = await _fileService.ValidateFile(shortVideoDto.File);

            if (validationResult.Any())
            {
                foreach (var error in validationResult)
                {
                    ModelState.AddModelError(
                        shortVideoDto.File.FileName, 
                        error?.ErrorMessage ?? string.Empty);
                }

                return BadRequest(ModelState);
            }

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(shortVideoDto.File.FileName)}";

            await _fileService.DownloadAsync(shortVideoDto.File, fileName);
            var isDownloaded = System.IO.File.Exists(Path.Combine(_fileService.FilePath, fileName));

            var @short = Mapping.Map<NewShortVideoDto, ShortVideo>(shortVideoDto);

            @short.Id = Guid.NewGuid();
            @short.FileName = $"shorts/{fileName}";

            await _db.Shorts.AddAsync(@short);
            var rows = await _db.SaveChangesAsync();

            if (rows == 0)
            {
                return Problem(detail: "Видео не добавлено в БД.");
            }

            return Ok(new
            {
                VideoId = @short.Id,
                FileDownloadIsSuccess = isDownloaded
            });
        }
    }
}
