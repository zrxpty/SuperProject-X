using Identity.BLL.Models;
using Identity.BLL.Models.InputModels;
using Identity.BLL.Models.OutputModels;

namespace Identity.BLL.Inrefaces
{
    public interface IRepositotyService
    {
        Task<ServiceResponse<AuthenticationOutputModel>> CreateUser(RegisterInputModel input);

        Task<ServiceResponse<AuthenticationOutputModel>> Authenticate(AuthenticateInputModel input);
    }
}