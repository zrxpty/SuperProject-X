using Tools.GenericModels;
using UserService.BLL.Models.OutputModels;

namespace UserService.BLL.Intrefaces
{
    public interface IUserService
    {
        Task<ServiceResponse<List<UserOutputModel>>> GetAllUsers();
        Task<ServiceResponse<ProfileOutputModel>> GetUser(string id);
    }
}
