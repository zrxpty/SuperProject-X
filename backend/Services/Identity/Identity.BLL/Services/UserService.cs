using Identity.BLL.Interface;
using Identity.BLL.Models;
using Identity.BLL.Models.OutputModels;
using Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace Identity.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IdentityDbContext _db;

        public UserService(IdentityDbContext db)
        {
            _db = db;
        }

        public async Task<ServiceResponse<List<UserOutputModel>>> GetAllUsers()
        {
            return new() 
            { 
                Data = await _db.UserAccounts.Select(x => new UserOutputModel(x)).ToListAsync()
            };
        }

        public async Task<ServiceResponse<ProfileOutputModel>> GetUser(string id)
        {
            var user = await _db.Profiles.Include(u => u.UserAccount).FirstOrDefaultAsync(u => u.UserAccount.Login == id);

            if (user == null) return new() { Code = 404, Message="Ну нет его"};

            return new()
            {
                Data = new ProfileOutputModel()
                {
                    City= user.City,
                    BirthDay = user.BirthDay,
                    CreatedDate = user.CreatedDate,
                    Login = user.UserAccount.Login,
                    Region = user.Region,
                    sex = user.Sex,
                }
            };
        }
    }
}
