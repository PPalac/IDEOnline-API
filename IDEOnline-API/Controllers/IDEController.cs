using Microsoft.AspNetCore.Mvc;

namespace IDEOnline_API.Controllers
{
    [Route("api/[controller]")]
    public class IDEController : Controller
    {
        // GET api/values
        [HttpGet]
        [Route("Run")]
        public IActionResult Run()
        {
            return Ok("value1 value2");
        }

        // POST api/values
        [HttpPost]
        public void Compile([FromBody]string value)
        {
        }
    }
}
