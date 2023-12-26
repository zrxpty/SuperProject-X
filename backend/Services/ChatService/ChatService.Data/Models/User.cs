namespace ChatService.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Login { get; set; }
        public int? UserId { get; set; }
        public Guid GroupId { get; set; }
    }
}
