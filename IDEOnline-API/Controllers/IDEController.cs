using IDEOnlineAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IDEOnlineAPI.Controllers
{
    [Route("api/[controller]")]
    public class IDEController : Controller
    {
        private IIDEService ideService;

        public IDEController(IIDEService ideService)
        {
            this.ideService = ideService;
        }
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
        public IActionResult Compile([FromBody]Code code)
        {
            try
            {
                var result = ideService.Compile(code.value);
                var jsonResult = Json(result);
                return Ok(jsonResult);
            }
            catch
            {
                return BadRequest();
            }
        }
    }

    public class Code
    {
        public string value { get; set; }
    }
}
