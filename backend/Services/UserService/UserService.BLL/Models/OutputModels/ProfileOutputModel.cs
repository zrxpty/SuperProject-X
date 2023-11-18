namespace UserService.BLL.Models.OutputModels
{
    public class ProfileOutputModel
    {
        public required string Login { get; set; }
        public string sex { get; set; } = null!;
        public string? City { get; set; }
        public string? Region { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        public DateTime? BirthDay { get; set; }

    }
}
