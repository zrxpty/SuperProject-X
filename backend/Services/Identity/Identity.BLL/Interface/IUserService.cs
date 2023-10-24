using Identity.BLL.Models;
using Identity.BLL.Models.OutputModels;

namespace Identity.BLL.Interface
{
    public interface IUserService
    {
        Task<ServiceResponse<List<UserOutputModel>>> GetAllUsers();
        Task<ServiceResponse<ProfileOutputModel>> GetUser(string id);
    }
}
