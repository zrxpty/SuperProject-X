using Microsoft.AspNetCore.Mvc;
using GATEWAY.Controllers.CustomController;

namespace GATEWAY.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : DefaultController
    {

        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
            
        }

        [HttpPost("for-you")]
        public async Task<IActionResult> Register()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://service-identity:80/", UriKind.Absolute);
            var response = await httpClient.GetAsync("api/public/lubov");
            var responseBody = await response.Content.ReadAsStringAsync();
            return Ok(responseBody);
        }


    }
}