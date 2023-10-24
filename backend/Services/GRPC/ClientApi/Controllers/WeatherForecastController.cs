using Grpc.Core;
using Grpc.Net.Client;
using GrpcServerCalculation;
using Microsoft.AspNetCore.Mvc;


namespace ClientApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly GrpcChannel _channel;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _channel = GrpcChannel.ForAddress("http://service-grpc");
        }

        [HttpPost("add")]
        public ActionResult<int> Add([FromBody] Object input)
        {
            try
            {
                var calculationClient = new Calculation.CalculationClient(_channel);
                var result = calculationClient.Add(new InputNumbers { Number1 = input.a, Number2 = input.b });
                return Ok(result.Result);
            }
            catch (RpcException ex)
            {
                _logger.LogError($"Status Code: {ex.StatusCode} | Error: {ex.Message}");
                return StatusCode((int)ex.StatusCode, new { Error = ex.Message });
            }
        }

        [HttpPost("subtract")]
        public ActionResult<int> Subtract([FromBody] Object input)
        {
            try
            {
                var calculationClient = new Calculation.CalculationClient(_channel);
                var headers = new Metadata
                {
                    { "Authorization", $"Bearer {input.Token}" }
                };

                var result = calculationClient.Subtract(new InputNumbers { Number1 = input.a, Number2 = input.b }, headers);
                return Ok(result.Result);
            }
            catch (RpcException ex)
            {
                _logger.LogError($"Status Code: {ex.StatusCode} | Error: {ex.Message}");
                return StatusCode((int)ex.StatusCode, new { Error = ex.Message });
            }
        }

        [HttpPost("multiply")]
        public ActionResult<int> Multiply([FromBody] Object input)
        {
            try
            {
                var calculationClient = new Calculation.CalculationClient(_channel);
                var headers = new Metadata
                {
                    { "Authorization", $"Bearer {input.Token}" }
                };

                var result = calculationClient.Multiply(new InputNumbers { Number1 = input.a, Number2 = input.b }, headers);
                return Ok(result.Result);
            }
            catch (RpcException ex)
            {
                _logger.LogError($"Status Code: {ex.StatusCode} | Error: {ex.Message}");
                return StatusCode((int)ex.StatusCode, new { Error = ex.Message });
            }
        }

        public class Object
        {
            public int a { get; set; }
            public int b { get; set; }
            public string? Token { get; set; }
        }
    }
}
