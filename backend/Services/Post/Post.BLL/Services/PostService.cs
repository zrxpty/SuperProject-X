using Microsoft.EntityFrameworkCore;
using Post.BLL.Interface;
using Post.BLL.Models;
using Post.Data;
using Tools.Controller;
using Tools.GenericModels;

namespace Post.BLL.Services
{
    public class PostService : IPostService
    {
        private readonly PostDbContext _db;

        public PostService(PostDbContext db)
        {
            _db = db;
        }

        public async Task<ServiceResponse<string>> Create(List<ClaimModel> userClaims, PostInputModel input)
        {
            await _db.Posts.AddAsync(new Data.Models.Post()
            {
                Body = input.Body,
                Title = input.Title,
                CreatedDate = DateTime.Now,
                Login = GetObject.GetUser(userClaims).Name
            });

            await _db.SaveChangesAsync();
            return new() {
                Data = "Ok"
            };
        }

        public async Task<ServiceResponse<List<Data.Models.Post>>> GetPost()
        {
            return new() { 
                Data = await _db.Posts.ToListAsync(),
            };
        }

        public async Task<ServiceResponse<List<Data.Models.Post>>> GetPost(string id)
        {
            return new()
            {
                Data = await _db.Posts.Where(x => x.Login == id).ToListAsync(),
            };
        }
    }
}
