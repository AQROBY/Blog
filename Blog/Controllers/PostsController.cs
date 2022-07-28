using Blog.Data;
using Blog.Dtos.Post;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Add(AddPostDto post)
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
            catch(DbUpdateException e)
            {
                if (e.InnerException.Message.Contains("UPDATE statement conflicted with the FOREIGN KEY constraint"))
                {
                    return NotFound("User " + post.UserId + " does not exist");
                }
                return NotFound(e.InnerException.Message);
            }
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
        public IActionResult Update(int id, AddPostDto post)
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
                if (e.Message.Contains("The database operation was expected to affect 1 row") ||
                    e.Message.Contains("Attempted to update or delete an entity that does not exist in the store"))
                {
                    return NotFound("Post with the id " + id + " does not exist");
                }
                if (e.InnerException.Message.Contains("UPDATE statement conflicted with the FOREIGN KEY constraint"))
                {
                    return NotFound("User " + post.UserId + " does not exist");
                }
                return NotFound(e.InnerException.Message);
            }
        }

        private List<Post> GetAll()
        {

            return _context.Posts.ToList();
        }
    }
}
