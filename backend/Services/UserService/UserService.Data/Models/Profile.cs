namespace UserService.Data.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string UserAccount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Sex { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; } = string.Empty;
    }
}
