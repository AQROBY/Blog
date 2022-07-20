using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content is required")]
        public string Contents { get; set; }
        public string Owner { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Modified_at { get; set; }
        public User User { get; set; }
    }
}
