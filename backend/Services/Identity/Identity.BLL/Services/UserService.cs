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
    }
}
