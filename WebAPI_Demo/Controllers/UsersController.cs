using Microsoft.AspNetCore.Mvc;
using WebAPI_Demo.Models;

namespace WebAPI_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = new User { Id = id, Name = "John Doe" };

            return Ok(user);
        }
    }
}
