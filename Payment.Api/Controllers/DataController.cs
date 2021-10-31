using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Payment.Api.Repository;

namespace Payment.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[action]")]
    public class DataController : ControllerBase
    {

        [HttpGet]
        public ActionResult<IEnumerable<User>> Users()
        {
            return Ok(Repo.users);
        }
    }

}
