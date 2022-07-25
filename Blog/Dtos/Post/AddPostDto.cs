namespace Blog.Dtos.Post
{
    public class AddPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Modified_at { get; set; }
        public int UserId { get; set; }
    }
}
