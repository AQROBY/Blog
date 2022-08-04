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

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            Post post = _context.Posts.SingleOrDefault(e => e.Id == id);
            if (post != null)
            {
                return Ok(post);
            }
            return NotFound("Post with the id " + id + " does not exist");
        }

        [HttpGet]
        public List<Post> Get()
        {
            return _context.Posts.ToList();
        }

        [HttpPost]
        public IActionResult Add(Post post)
        {
            try
            {
                Post postToAdd = new Post
                {
                    Contents = post.Contents,
                    Title = post.Title,
                    Modified_at = DateTime.Now,
                    UserId = post.UserId
                };
                _context.Posts.Add(postToAdd);
                _context.SaveChanges();
                return Created("post/" + postToAdd.Id, postToAdd);
            }
            catch (Exception e)
            {
                if (e.HResult == -2146233088)
                {
                    return NotFound("User " + post.UserId + " does not exist");
                }
                return NotFound(e.InnerException.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
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
            try
            {
                Post postToUpdate = new Post
                {
                    Id = id,
                    Contents = post.Contents,
                    Title = post.Title,
                    Modified_at = DateTime.Now,
                    UserId = post.UserId
                };
                _context.Update(postToUpdate);
                _context.SaveChanges();

                return Created("post/" + postToUpdate.Id, postToUpdate);
            }
            catch (Exception e)
            {
                if (e.HResult == -2146233088 && e.InnerException == null)
                {
                    return NotFound("Post with the id " + id + " does not exist");
                }
                if (e.InnerException.HResult == -2146232060)
                {
                    return NotFound("User " + post.UserId + " does not exist");
                }
                return NotFound(e.InnerException.Message);
            }
        }
    }
}
