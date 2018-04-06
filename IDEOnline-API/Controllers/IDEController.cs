using IDEOnlineAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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

        //// GET api/values
        //[HttpGet]
        //[Route("Run")]
        //[Produces("text/plain")]
        //public async Task<IActionResult> RunAsync()
        //{
        //    try
        //    {
        //        var result = await ideService.RunAsync();
        //        var jsonResult = Json(result);
        //        return jsonResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest();
        //    }
        //}

        // POST api/values
        [HttpPost]
        [Route("Compile")]
        public async Task<IActionResult> CompileAsync([FromBody]Code code)
        {
            var ID = Guid.NewGuid();

            try
            {
                var result = await ideService.CompileAsync(code.value, ID.ToString());
                var response = new CompileResult()
                {
                    compileResult = result,
                    ID = ID.ToString()
                };

                var jsonResult = Json(response);
                return jsonResult;
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }

    public class Code
    {
        public string value { get; set; }
    }

    public class CompileResult
    {
        public string compileResult { get; set; }
        public string ID { get; set; }
    }
}
