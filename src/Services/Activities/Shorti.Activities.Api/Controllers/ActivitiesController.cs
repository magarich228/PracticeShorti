using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shorti.Activities.Api.Data;

namespace Shorti.Activities.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly ActivitiesContext _db;

        public ActivitiesController(ActivitiesContext db)
        {
            _db = db;
        }


    }
}
