using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("public/lubov")]
        public async Task<ActionResult> Love()
        {
            return Ok(new Response() { Message = "service by Nikita send  via http" });
        }

        public class Response
        {
            public string Message { get; set; }
        }
    }
}