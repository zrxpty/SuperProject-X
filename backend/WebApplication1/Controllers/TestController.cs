using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using Grpc.Core;
using ClientWebApp;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Atributes;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {

        private readonly ILogger<TestController> _logger;


        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "Register")]
        public async Task<IActionResult> Get([FromBody] RegisterRequest registerRequest)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7275");
            try
            {
                var newClient = new IdentityGrpc.IdentityGrpcClient(channel);
                var registerResponse = await newClient.RegisterAsync(registerRequest);

                if (registerResponse.Status.Code == 200)
                {
                    return Ok($"{registerResponse}");
                }
                else
                {
                    return Ok($"{registerResponse.Status.Message}");
                }
            }
            catch (RpcException ex)
            {
                return Ok($"RpcException: StatusCode={ex.StatusCode}, Detail={ex.Status.Detail}");
            }

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Auth([FromBody] AuthRequest authRequest)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7275");
            try
            {
                var newClient = new IdentityGrpc.IdentityGrpcClient(channel);
                var authResponse = await newClient.AuthenticateAsync(authRequest);

                if (authResponse.Status.Code == 200)
                {
                    return Ok($"{authResponse}");
                }
                else
                {
                    return Ok($"{authResponse}");
                }
            }
            catch (RpcException ex)
            {
                return Ok($"RpcException: StatusCode={ex.StatusCode}, Detail={ex.Status.Detail}");
            }
        }

        [Auth]
        [HttpGet]
        public async Task<IActionResult> Testtqweq()
        {
            return Ok();
        }
    }
}