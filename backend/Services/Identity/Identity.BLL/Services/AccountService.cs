using Identity.BLL.Inrefaces;
using Identity.BLL.Models;
using Identity.BLL.Models.InputModels;
using Identity.BLL.Models.OutputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BLL.Services
{
    public class AccountService : IAccountService
    {

        private readonly IRepositotyService _repositotyService;

        public AccountService(IRepositotyService repositotyService)
        {
            _repositotyService = repositotyService;
        }

        public async Task<ServiceResponse<AuthenticationOutputModel>> Register(RegisterInputModel input)
        {
            return await _repositotyService.CreateUser(input);
        }

        public async Task<ServiceResponse<AuthenticationOutputModel>> Authenticate(AuthenticateInputModel input)
        {
            return await _repositotyService.Authenticate(input);
        }

        
    }
}
