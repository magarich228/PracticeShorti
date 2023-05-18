using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shorti.Shared.Contracts.Shorts;

namespace Shorti.ShortsService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShortsController : ControllerBase
    {
        public ShortsController()
        {

        }

        [HttpPost]
        public async Task Upload([FromBody] NewShortVideoDto shortVideoDto)
        {
            
        }
    }
}
