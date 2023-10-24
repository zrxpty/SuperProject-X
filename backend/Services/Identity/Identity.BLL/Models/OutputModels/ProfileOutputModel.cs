namespace Identity.BLL.Models.OutputModels
{
    public class ProfileOutputModel
    {
        public string? Login { get; set; }
        public string? sex { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        public DateTime? BirthDay { get; set; }

    }
}
