﻿using Identity.BLL.Models.InputModels;
using Identity.BLL.Models.OutputModels;
using Tools.GenericModels;

namespace Identity.BLL.Inrefaces
{
    public interface IIdentityService
    {
        Task<ServiceResponse<AuthenticationOutputModel>> Register(RegisterInputModel input);
        Task<ServiceResponse<AuthenticationOutputModel>> Authenticate(AuthenticateInputModel input);
    }
}