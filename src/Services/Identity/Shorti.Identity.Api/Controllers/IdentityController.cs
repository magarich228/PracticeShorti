using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shorti.Identity.Api.Data;
using Shorti.Shared.Contracts.Identity;

namespace Shorti.Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;

        public IdentityController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IUserStore<User> userStore)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    loginDto.Username,
                    loginDto.Password,
                    loginDto.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                    return BadRequest(ModelState);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, registerDto.Username, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return Ok(userId);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [NonAction]
        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
