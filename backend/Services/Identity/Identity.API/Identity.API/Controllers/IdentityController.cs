using Identity.BLL.Inrefaces;
using Identity.BLL.Models.InputModels;
using Identity.BLL.Models.OutputModels;
using Identity.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _accountService;

        public IdentityController(IIdentityService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthenticationOutputModel), 200)]
        [ProducesResponseType(typeof(Exception), 500)]
        public async Task<ActionResult> Register([FromBody] RegisterInputModel input)
        {
            return Ok(await _accountService.Register(input));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] AuthenticateInputModel input)
        {
            return Ok(await _accountService.Authenticate(input));
        }

        

        [HttpGet("qwe")]
        [Authorize]
        public async Task<ActionResult> qwe()
        {  
             return Ok("qweqweqweqweqwe");
        }
        
    }
}
