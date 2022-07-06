using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostDbContext _context;

        public PostsController(PostDbContext context)
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
            Post postToAdd = new Post { Id = AssignId(), Contents = post.Contents, Title = post.Title, Owner = Owner(), 
                Modified_at = DateTime.Now };
            _context.Posts.Add(postToAdd);
            _context.SaveChanges();
            return Created("post/" + postToAdd.Id, postToAdd);
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

            return Ok("Post with the id " + id + " updated successfully");
        }

        private List<Post> GetAll()
        {

            return _context.Posts.ToList();
        }

        private int AssignId()
        {
            return _context.Posts.Count() + 1;
        }

        private string Owner()
        {
            int ownerNumber = _context.Posts.Count() + 1;
            return "Owner " + ownerNumber;
        }
    }
}
