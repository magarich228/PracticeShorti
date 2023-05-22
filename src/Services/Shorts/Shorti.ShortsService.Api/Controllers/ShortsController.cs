using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shorti.Shared.Contracts.Shorts;
using Shorti.Shared.Kernel;
using Shorti.Shared.Kernel.Abstractions;
using Shorti.ShortsService.Api.Data;
using Shorti.ShortsService.Api.Data.Models;

namespace Shorti.ShortsService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> Upload([FromBody] NewShortVideoDto shortVideoDto)
        {
            string path = Path.GetRandomFileName();

            await _fileService.DownloadAsync(shortVideoDto.File, path);
            var result = System.IO.File.Exists(Path.Combine(_fileService.FilePath, path));

            var @short = Mapping.Map<NewShortVideoDto, ShortVideo>(shortVideoDto);
            @short.FileName = path;

            await _db.Shorts.AddAsync(@short);
            var rows = await _db.SaveChangesAsync();

            return Ok($"File download sucsess: {result}\nRows added: {rows}");
        }
    }
}
