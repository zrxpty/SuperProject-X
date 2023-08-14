
using Identity.BLL.Models.InputModels;
using Identity.BLL.Models.OutputModels;

namespace Identity.BLL.Inrefaces
{
    public interface IAccountService
    {
        Task<RegisterOutputModel> Register(RegisterInputModel input);
        Task<RegisterOutputModel> Authenticate(AuthenticateInputModel input);
    }
}