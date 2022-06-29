using Blog.Models;
using Blog.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostDbContext _context;
        private int counter = 0;

        public PostController(PostDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public Post GetById(int id)
        {
            Post post = _context.Posts.SingleOrDefault(e => e.Id == id);
            if (post != null)
            {
                return post;
            }

            return null;
        }

        [HttpGet]
        public List<Post> GetAll()
        {
            return _context.Posts.ToList();
        }

        [HttpPost]
        public string Add(string contents, string title)
        {
            string date = Date();
            Post post = new Post { Id = AssignId(), Contents = contents, Title = title, Owner = Owner(), 
                Created_at = date, Modified_at = date };
            _context.Posts.Add(post);
            _context.SaveChanges();
            if (_context.Posts.ToList() != null)
            {
                return "Post was added";
            }

            return "Post adding failed";
        }

        private int AssignId()
        {
            return counter++;
        }

        private string Owner()
        {
            return "Owner" + counter;
        }

        private string Date()
        {
            DateTime date = DateTime.Now;
            return date.ToShortDateString() + " " + date.ToShortTimeString();
        }
    }
}
