using Post.BLL.Models;
using Tools.Controller;
using Tools.GenericModels;

namespace Post.BLL.Interface
{
    public interface IPostService
    {
        Task<ServiceResponse<List<Post.Data.Models.Post>>> GetPost();
        Task<ServiceResponse<List<Post.Data.Models.Post>>> GetPost(string id);
        Task<ServiceResponse<string>> Create(List<ClaimModel> userClaims, PostInputModel input);
    }
}
