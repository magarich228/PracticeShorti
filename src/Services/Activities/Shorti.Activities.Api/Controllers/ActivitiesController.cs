using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shorti.Activities.Api.Data;
using Shorti.Activities.Api.Data.Models;
using Shorti.Shared.Contracts.Activities;
using Shorti.Shared.Contracts.Services;
using Shorti.Shared.Kernel;
using Shorti.Shared.Kernel.Extensions;

namespace Shorti.Activities.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly ActivitiesContext _db;
        private readonly IIdentityServiceClient _identityServiceClient;
        private readonly IShortsServiceClient _shortsServiceClient;

        public ActivitiesController(
            ActivitiesContext db,
            IIdentityServiceClient identityServiceClient,
            IShortsServiceClient shortsServiceClient)
        {
            _db = db;
            _identityServiceClient = identityServiceClient;
            _shortsServiceClient = shortsServiceClient;
        }

        [HttpPost("like/{shortId}")]
        public async Task<ActionResult<LikeReactionDto>> LikeShort([FromRoute] Guid shortId)
        {
            var @short = await _shortsServiceClient.GetShortByIdAsync(shortId);

            if (@short == null)
            {
                ModelState.AddModelError("shortId", $"Видео с Id: {shortId} не найдено.");

                return BadRequest(ModelState);
            }

            var user = await _identityServiceClient.GetCurrentUserAsync(HttpContext.GetToken()!);
            var existedLike = await _db.Likes.FirstOrDefaultAsync(l => l.UserId == user!.Id && l.ShortId == shortId);

            if (existedLike != null)
            {
                _db.Likes.Remove(existedLike);
            }

            var like = new LikeReaction
            {
                Id = Guid.NewGuid(),
                ShortId = shortId,
                UserId = user!.Id
            };

            await _db.Likes.AddAsync(like);
            var rows = await _db.SaveChangesAsync();
            
            return rows > 0 ?
                Ok(Mapping.Map<LikeReaction, LikeReactionDto>(like)) :
                Problem();
        }

        [HttpDelete("unlike/{shortId}")]
        public async Task<IActionResult> UnLikeShort([FromRoute] Guid shortId)
        {
            var @short = await _shortsServiceClient.GetShortByIdAsync(shortId);

            if (@short == null)
            {
                ModelState.AddModelError("shortId", $"Видео с Id: {shortId} не найдено.");

                return BadRequest(ModelState);
            }

            var user = await _identityServiceClient.GetCurrentUserAsync(HttpContext.GetToken()!);
            var existedLike = await _db.Likes.FirstOrDefaultAsync(l => l.UserId == user!.Id && l.ShortId == shortId);

            if (existedLike != null)
            {
                _db.Likes.Remove(existedLike!);
            }

            var rows = await _db.SaveChangesAsync();

            return Ok();
        }
    }
}
