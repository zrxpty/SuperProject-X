using Tools.GenericModels;
using UserService.BLL.Intrefaces;
using UserService.BLL.Models.OutputModels;
using UserService.Data;

namespace UserService.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UsersDbContext _db;

        public UserService(UsersDbContext db)
        {
            _db = db;
        }

        public async Task<ServiceResponse<List<UserOutputModel>>> GetAllUsers()
        {
            return new();
        }

        public async Task<ServiceResponse<ProfileOutputModel>> GetUser(string id)
        {
            return new()
            {
                
            };
        }
    }
}
