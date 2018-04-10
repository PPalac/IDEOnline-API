using IDEOnlineAPI.Models;
using IDEOnlineAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace IDEOnlineAPI.Controllers
{
    /// <summary>
    /// Main controller of IDEOnline. Used only to compile code.
    /// </summary>
    [Route("api/[controller]")]
    public class IDEController : Controller
    {
        private IIDEService ideService;

        public IDEController(IIDEService ideService)
        {
            this.ideService = ideService;
        }

        /// <summary>
        /// Compile endpoint. Used to compile given code.
        /// </summary>
        /// <param name="code">Param is object with code property.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Compile")]
        public async Task<IActionResult> CompileAsync([FromBody]CodeViewModel code)
        {
            var ID = Guid.NewGuid();

            try
            {
                var result = await ideService.CompileAsync(code.Value, ID.ToString());
                var response = new CompileResult()
                {
                    Result = result,
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
}
