using Grpc.Core;
using Identity.BLL.Inrefaces;
using Identity.BLL.Models.InputModels;
using Identity.Service.identityProtos;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Service.Services
{
    public class IdentityServiceGrpc : IdentityGrpc.IdentityGrpcBase
    {
        public ILogger<IdentityServiceGrpc> _logger { get; set; }
        public readonly IAccountService _accountService;


        public IdentityServiceGrpc(ILogger<IdentityServiceGrpc> logger, IAccountService accountService) 
        {
            _accountService= accountService;
            _logger= logger;
        }

        public override async Task<ServiceResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"{request.Email}");
            var registerInput = new RegisterInputModel
            {
                Email = request.Email,
                Login = request.Login,
                Password = request.Password
            };

            var registerResult = await _accountService.Register(registerInput);

            if (registerResult.Token.IsNullOrEmpty()) 
            {
                var response = new ServiceResponse
                {
                    Status = new ServiceStatus
                    {
                        Code = 400, 
                        Message = "Error in registration: Try a different username or email"
                    },
                    Data = new RegisterOutputModels()
                    {
                        Email = registerResult.Email,
                        Login = registerResult.Login,
                        Role = registerResult.Role,
                        Token = registerResult.Token
                    }
                };
                return response;
            }
            else
            {
                var response = new ServiceResponse
                {
                    Status = new ServiceStatus
                    {
                        Code = 200, 
                        Message = "Ok" 
                    },
                    Data = new RegisterOutputModels()
                    {
                        Email = registerResult.Email,
                        Login = registerResult.Login,
                        Role = registerResult.Role,
                        Token = registerResult.Token
                    }
                };
                return response;
            }

            
        }

        public override async Task<ServiceResponse> Authenticate(AuthRequest request, ServerCallContext context)
        {
            var userLogin = new AuthenticateInputModel()
            {
                Login = request.LoginOrEmail,
                Password = request.Password,
            };

            var user = await _accountService.Authenticate(userLogin);

            if (string.IsNullOrEmpty(user.Token))
            {
                return new()
                {
                    Status = new ServiceStatus
                    {
                        Code = 400,
                        Message = "Error in Authenticate: Try a different username or email or password"
                    },
                    Data = new RegisterOutputModels()
                    {
                        Email = "",
                        Login = "",
                        Role = "",
                        Token = ""
                    }
                };
            }
            else
            {
                return new()
                {
                    Status = new()
                    {
                        Code = 200,
                        Message = "Ok"
                    },
                    Data = new RegisterOutputModels()
                    {
                        Email = user.Email,
                        Login = user.Login,
                        Role = user.Role,
                        Token = user.Token
                    }
                };
            }
        }
    }
}
