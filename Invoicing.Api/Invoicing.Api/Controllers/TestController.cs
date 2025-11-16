using Microsoft.AspNetCore.Mvc;

namespace Invoicing.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "API is working!" });
        }
    }
}
