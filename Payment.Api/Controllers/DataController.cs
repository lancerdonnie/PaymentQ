using Microsoft.AspNetCore.Mvc;
using Payment.Api.Repository;
using Payment.Api.Utilities;

namespace Payment.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[action]")]
    public class DataController : ControllerBase
    {


        [HttpGet]
        public ActionResult<string> Users()
        {
            return Ok(new Response200
            {
                Data = Repo.users
            });
        }


    }

}
