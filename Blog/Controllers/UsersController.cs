using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            User user = _context.Users.SingleOrDefault(e => e.Id == id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound("User with the id " + id + " does not exist");
        }

        [HttpGet]
        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        [HttpPost]
        public IActionResult Add(User user)
        {
            try
            {
                User userToAdd = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Modified_at = DateTime.Now,
                    Posts = { }
                };
                _context.Users.Add(userToAdd);
                _context.SaveChanges();
                return Created("user/" + userToAdd.Id, userToAdd);
            }
            catch (Exception e)
            {
                return NotFound(e.InnerException.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound("User with the id " + id + " does not exist");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok("User with the id " + id + " deleted successfully");
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, User user)
        {
            try
            {
                User userToUpdate = new User
                {
                    Id = id,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    Modified_at = DateTime.Now
                };
                _context.Update(userToUpdate);
                _context.SaveChanges();

                return Created("user/" + userToUpdate.Id, userToUpdate);
            }
            catch (Exception e)
            {
                if (e.HResult == -2146233088)
                {
                    return NotFound("User with the id " + id + " does not exist");
                }
                return NotFound(e.InnerException.Message);
            }
        }
    }
}
