using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shorti.Activities.Api.Data;
using Shorti.Activities.Api.Data.Models;
using Shorti.Shared.Contracts.Activities;
using Shorti.Shared.Contracts.Services;
using Shorti.Shared.Contracts.Shorts;
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

        [HttpPost("subscribe/{userId}")]
        public async Task<ActionResult<SubscriptionDto>> SubscribeOnUser([FromRoute] Guid userId)
        {
            //Решить баг с Not Found
            var user = await _identityServiceClient.GetUserById(userId);

            if (user == null)
            {
                ModelState.AddModelError("userId", $"Пользователь с Id: {userId} не найден.");

                return BadRequest(ModelState);
            }

            var currentUser = await _identityServiceClient.GetCurrentUserAsync(HttpContext.GetToken()!);
            var existedSub = await _db.Subscriptions.FirstOrDefaultAsync(s => s.SubscriberId == currentUser!.Id && s.SubscriptionId == userId);

            if (existedSub != null)
            {
                _db.Subscriptions.Remove(existedSub!);
            }

            var sub = new Subscription
            {
                Id = Guid.NewGuid(),
                SubscriberId = currentUser!.Id,
                SubscriptionId = user.Id
            };

            await _db.Subscriptions.AddAsync(sub);
            var rows = await _db.SaveChangesAsync();

            return rows > 0 ?
                Ok(Mapping.Map<Subscription, SubscriptionDto>(sub)) :
                Problem();
        }

        [HttpDelete("unsubscribe/{userId}")]
        public async Task<IActionResult> UnSubscribeOnUser([FromRoute] Guid userId)
        {
            var user = await _identityServiceClient.GetUserById(userId);

            if (user == null)
            {
                ModelState.AddModelError("userId", $"Пользователь с Id: {userId} не найден.");

                return BadRequest(ModelState);
            }

            var currentUser = await _identityServiceClient.GetCurrentUserAsync(HttpContext.GetToken()!);
            var existedSub = await _db.Subscriptions.FirstOrDefaultAsync(s => s.SubscriberId == currentUser!.Id && s.SubscriptionId == userId);

            if (existedSub != null)
            {
                _db.Subscriptions.Remove(existedSub!);
            }

            var rows = await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("likedShorts/{userId}")]
        public async Task<ActionResult<IEnumerable<ShortVideoDto>>> GetUserLikedShorts([FromRoute] Guid userId)
        {
            var user = await _identityServiceClient.GetUserById(userId);

            if (user == null)
            {
                ModelState.AddModelError("userId", $"Пользователь с Id: {userId} не найден.");

                return BadRequest(ModelState);
            }

            var shortsQuery = _db.Likes.Where(l => l.UserId == userId);

            List<ShortVideoDto> shorts = new();

            foreach (var like in shortsQuery)
            {
                var @short = await _shortsServiceClient.GetShortByIdAsync(like.ShortId);

                if (@short != null)
                {
                    shorts.Add(@short);
                }
            }

            return shorts;
        }

        [HttpGet("subscriptionVideo/{userId}")]
        public async Task<ActionResult<IEnumerable<ShortVideoDto>>> GetUserSubscriptionShorts([FromRoute] Guid userId)
        {
            var user = await _identityServiceClient.GetUserById(userId);

            if (user == null)
            {
                ModelState.AddModelError("userId", $"Пользователь с Id: {userId} не найден.");

                return BadRequest(ModelState);
            }

            var subsIds = _db.Subscriptions.Where(s => s.SubscriberId == userId);

            List<ShortVideoDto> shorts = new();
            
            foreach (var subId in subsIds)
            {
                var userShorts = await _shortsServiceClient.GetUserShorts(subId.SubscriptionId);

                shorts.AddRange(userShorts);
            }

            return shorts;
        }
    }
}
