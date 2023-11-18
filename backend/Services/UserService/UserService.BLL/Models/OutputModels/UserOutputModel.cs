namespace UserService.BLL.Models.OutputModels
{
    public class UserOutputModel
    {
        public string Login { get; set; }

        public UserOutputModel(string user)
        {
            Login = user;
        }
    }
}
