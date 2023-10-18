using Identity.Data.Models;

namespace Identity.BLL.Models.OutputModels
{
    public class UserOutputModel
    {
        public string Login { get; set; }

        public UserOutputModel(UserAccount user) 
        {
            Login = user.Login;
        }
    }
}
