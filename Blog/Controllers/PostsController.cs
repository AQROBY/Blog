using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int?}")]
        public IActionResult Get (int id = 0)
        {
            if (id != 0)
            {
                Post post = _context.Posts.SingleOrDefault(e => e.Id == id);
                if (post != null)
                {
                    return Ok(post);
                }
                return NotFound("Post with the id " + id + " does not exist");
            }
            return Ok(GetAll()); 
        }

        [HttpPost]
        public IActionResult Add(Post post)
        {
            User userToAdd = _context.Users.SingleOrDefault(e => e.Id == post.User.Id);
            if (userToAdd != null)
            {
                Post postToAdd = new Post
                {
                    Contents = post.Contents,
                    Title = post.Title,
                    Owner = post.User.Name,
                    Modified_at = DateTime.Now,
                    User = userToAdd
                };
                _context.Posts.Add(postToAdd);
                _context.SaveChanges();
                return Created("post/" + postToAdd.Id, postToAdd);
            }
            return NotFound("User with the id " + post.User.Id + " does not exist");
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete (int id)
        {
            var post = _context.Posts.SingleOrDefault(x => x.Id == id);

            if (post == null)
            {
                return NotFound("Post with the id " + id + " does not exist");
            }

            _context.Posts.Remove(post);
            _context.SaveChanges();

            return Ok("Post with the id " + id + " deleted successfully");
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, Post post)
        {
            User userToAdd = _context.Users.SingleOrDefault(e => e.Id == post.User.Id);
            if (userToAdd != null)
            {
                var postToUpdate = _context.Posts.SingleOrDefault(x => x.Id == id);

                if (postToUpdate == null)
                {
                    return NotFound("Post with the id " + id + " does not exist");
                }

                if (post.Contents != null)
                {
                    postToUpdate.Contents = post.Contents;
                }

                if (post.Title != null)
                {
                    postToUpdate.Title = post.Title;
                }

                postToUpdate.Modified_at = DateTime.Now;
                _context.Update(postToUpdate);
                _context.SaveChanges();

                return Created("post/" + postToUpdate.Id, postToUpdate);
            }
            return NotFound("User with the id " + post.User.Id + " does not exist");
        }

        private List<Post> GetAll()
        {

            return _context.Posts.ToList();
        }
    }
}
