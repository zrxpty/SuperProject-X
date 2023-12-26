using UserService.Data.Models;

namespace UserService.BLL.Models.OutputModels
{
    public class ProfileOutputModel
    {
        public string Login { get; set; }
        public string Sex { get; set; } = null!;
        public string? City { get; set; }
        public string? Region { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        public DateTime? BirthDay { get; set; }

        public ProfileOutputModel(Profile input)
        {
            Login = input.UserAccount;
            City = input.City;
            Region = input.Region;
            BirthDay = input.BirthDay;
            Sex = input.Sex;
            CreatedDate = input.CreatedDate;
            BirthDay = input.BirthDay;
        }

    }
}
