using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Shorti.Shared.Contracts.Services;
using Shorti.Shared.Contracts.Shorts;
using Shorti.Shared.Kernel;
using Shorti.Shared.Kernel.Abstractions;
using Shorti.Shared.Kernel.Extensions;
using Shorti.ShortsService.Api.Data;
using Shorti.ShortsService.Api.Data.Models;

namespace Shorti.ShortsService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortsController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ShortsContext _db;

        public ShortsController(
            IFileService fileService, 
            ShortsContext db)
        {
            _fileService = fileService;
            _db = db;
        }

        [HttpGet("{shortId}")]
        public async Task<ActionResult<ShortVideoDto>> GetShortById([FromRoute] Guid shortId)
        {
            var @short = await _db.Shorts.FindAsync(new object[] { shortId });
            
            if (@short == null)
            {
                return NotFound(shortId);
            }

            var result = Mapping.Map<ShortVideo, ShortVideoDto>(@short);
            
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ShortVideoDto>>> Search([FromQuery] string[] words)
        {
            var shorts = await _db.Shorts.ToArrayAsync();
                
            var result = shorts.Select(s => new
            {
                Short = s,
                MatchesCount = words.Count(w => s.Title.Contains(w)) + 
                ((s.Description != null) ? words.Count(w => s.Description!.Contains(w)) : 0)
            })
                .Where(s => s.MatchesCount > 0)
                .OrderBy(s => s.MatchesCount)
                .Select(s => Mapping.Map<ShortVideo, ShortVideoDto>(s.Short));

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetPage([FromQuery] int page, [FromQuery] int count)
        {
            ValidatePaginationQueryParams(page, count);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var allShortsCount = await _db.Shorts.CountAsync();
            HttpContext.Response.Headers.Add(new KeyValuePair<string, StringValues>("Count", allShortsCount.ToString()));

            var query =  _db.Shorts.Skip((page - 1) * count).Take(count);

            var shorts = await query.ToListAsync();

            return shorts.Any() ? 
                Ok(shorts) :
                NoContent();
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ShortVideoDto>>> GetUserShorts([FromRoute] Guid userId)
        {
            var query = _db.Shorts.Where(s => s.AuthorId == userId);

            return await query.AnyAsync() ?
                Ok(await query.ToListAsync()) :
                NoContent();
        } 

        [HttpPost]
        public async Task<IActionResult> Upload(
            [FromForm] NewShortVideoDto shortVideoDto,
            [FromServices] IIdentityServiceClient identityServiceClient)
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

            var token = HttpContext.GetToken();
            
            var authorId = (await identityServiceClient.GetCurrentUserAsync(token!))!.Id;

            await _fileService.DownloadAsync(shortVideoDto.File, fileName);
            var isDownloaded = System.IO.File.Exists(Path.Combine(_fileService.FilePath, fileName));

            var @short = Mapping.Map<NewShortVideoDto, ShortVideo>(shortVideoDto);

            @short.Id = Guid.NewGuid();
            @short.AuthorId = authorId;
            @short.FileName = $"shorts/{fileName}";
            @short.UploadedAt = DateTime.UtcNow;

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

        [NonAction]
        private void ValidatePaginationQueryParams(int page, int count)
        {
            if (page <= 0)
            {
                ModelState.AddModelError("page", "Номер страниц контента начинается с 1.");
            }

            if (count <= 0)
            {
                ModelState.AddModelError("count", "Количества элементов на странице должно быть больше нуля.");
            }
        }
    }
}
