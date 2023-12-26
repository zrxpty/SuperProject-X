namespace ChatService.Data.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string? Body { get;  set; }
        public DateTime Created {  get; set; }
        public string? To { get; set; }
        public string? From { get; set; }
    }
}
