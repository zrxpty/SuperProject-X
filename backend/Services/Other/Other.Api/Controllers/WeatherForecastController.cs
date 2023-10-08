using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Other.Api.Controllers
{
    [ApiController]
    [Route("api/other")]
    public class WeatherForecastController : ControllerBase
    {
        

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("checkclaims")]
        [Authorize]
        public async Task<ActionResult> Get()
        {
            var claims = User.Claims.Select(c => new ClaimModel { Type = c.Type, Value = c.Value }).ToList();
            return Ok(claims);
        }
        public class ClaimModel
        {
            public string Type { get; set; }
            public string Value { get; set; }
        }
    }
}