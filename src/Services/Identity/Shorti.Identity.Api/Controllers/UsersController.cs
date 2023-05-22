using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shorti.Identity.Api.Data;
using Shorti.Shared.Contracts.Identity;
using Shorti.Shared.Kernel;

namespace Shorti.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ShortiIdentityContext _db;

        public UsersController(ShortiIdentityContext db)
        {
            _db = db;
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
    }
}
