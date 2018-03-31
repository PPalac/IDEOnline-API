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
            var jsonResult = Json("No uruchamiam");
            return Ok(jsonResult);
        }

        // POST api/values
        [HttpPost]
        [Route("Compile")]
        public IActionResult Compile([FromBody]string value)
        {
            var jsonResult = Json("No kompiluje");
            return Ok(jsonResult);
        }
    }
}
