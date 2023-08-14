

using Identity.BLL.Models.InputModels;
using Identity.BLL.Models.OutputModels;

namespace Identity.BLL.Inrefaces
{
    public interface IRepositotyService
    {
        Task<RegisterOutputModel> CreateUser(RegisterInputModel input);

        Task<RegisterOutputModel> Authenticate(AuthenticateInputModel input);
    }
}