using Identity.BLL.Models;
using Identity.BLL.Models.InputModels;
using Identity.BLL.Models.OutputModels;

namespace Identity.BLL.Inrefaces
{
    public interface IAccountService
    {
        Task<ServiceResponse<AuthenticationOutputModel>> Register(RegisterInputModel input);
        Task<ServiceResponse<AuthenticationOutputModel>> Authenticate(AuthenticateInputModel input);
    }
}