using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BionicPro.ReportsApi.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class ReportsController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "ProtheticOnly")]
        public IActionResult GetReport()
        {
            return Ok(new { message = "This is a protected report." });
        }
    }
}