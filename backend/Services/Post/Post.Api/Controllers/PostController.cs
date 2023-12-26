using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Post.BLL.Interface;
using Post.BLL.Models;
using Tools.Controller;

namespace Post.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : DefaultController
    {
        private readonly IPostService _postService;

        public PostController(ILogger<PostController> logger, IPostService postService) : base(logger)
        {
            _postService = postService;
        }

        [HttpGet("posts")]
        public async Task<IActionResult> GetPosts()
        {
            return await GetServiceResponseWithAuthAsync(async (user) => await _postService.GetPost());
        }

        [HttpGet("posts/{id}")]
        public async Task<IActionResult> GetPosts(string id)
        {
            return await GetServiceResponseWithAuthAsync(async (user) => await _postService.GetPost(id));
        }

        [HttpPost("post")]
        public async Task<IActionResult> Create([FromBody] PostInputModel input)
        {
            return await GetServiceResponseWithAuthAsync(async (user) => await _postService.Create(user, input));
        }


    }
}
