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

        [HttpPost("register")]
        public async Task<IActionResult> Register()
        {
            var httpClient = new HttpClient();
            using var request = new HttpRequestMessage(HttpMethod.Get, "http://host.docker.internal:5001/api/public/lubov");
            using var response = await httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            return Ok(responseBody);
        }


    }
}