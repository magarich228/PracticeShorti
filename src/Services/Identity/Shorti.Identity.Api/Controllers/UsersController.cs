using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shorti.Identity.Api.Data;
using Shorti.Shared.Contracts.Identity;
using Shorti.Shared.Kernel;

namespace Shorti.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById([FromRoute] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return NotFound(userId);
            }

            var result = Mapping.Map<User, UserDto>(user);

            return Ok(result);
        }
    }
}
