namespace Post.Data.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Login {  get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set;}
    }
}
