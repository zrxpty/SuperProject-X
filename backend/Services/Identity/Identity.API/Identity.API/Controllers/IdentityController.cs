using Identity.BLL.Inrefaces;
using Identity.BLL.Models.InputModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tools.Controller;
using Tools.GenericModels;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : DefaultController
    {
        private readonly IIdentityService _accountService;
        private readonly ILogger<IdentityController> _logger;

        public IdentityController(IIdentityService accountService, ILogger<IdentityController> logger)
        : base(logger) 
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterInputModel input)
        {
            return Ok(await _accountService.Register(input));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] AuthenticateInputModel input)
        {
            return Ok(await _accountService.Authenticate(input));
        }

        [Authorize]
        [HttpGet("example")]
        public async Task<ActionResult> ExampleAction()
        {
            return await GetServiceResponseWithAuthAsync(YourServiceFunction);
        }

        private async Task<ServiceResponse<List<ClaimModel>>> YourServiceFunction(List<ClaimModel> userClaims)
        {
            var result = new ServiceResponse<List<ClaimModel>>()
            {
                Data = userClaims,
                Message = string.Join(", ", userClaims)
            };

            return await Task.FromResult(result);
        }
    }
}
