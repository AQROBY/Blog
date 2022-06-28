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
        PostRepo p = new PostRepo();
        [HttpGet]
        public string GetName()
        {
            return "Test";
        }

        [HttpGet]
        public string GetFullName()
        {
            return "Robert";
        }

        [HttpPost]
        public string Add()
        {
            Post post = new Post { Id = 1, Contents = "asd", Title = "ffff", Owner = "RObert", Modified_at = DateTime.Now };
            p.Add(post);
            if (p != null)
            {
                return "Ok";
            }

            return "Failed";
        }
    }
}
