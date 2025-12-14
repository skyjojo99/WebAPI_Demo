using Microsoft.AspNetCore.Mvc;
using WebAPI_Demo.Dtos;
using WebAPI_Demo.Services;

namespace DemoApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _service.GetUser(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserUpsertDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required.");

            var name = dto.Name.Trim();
            var newId = _service.CreateUser(name);

            return CreatedAtAction(nameof(GetUser), new { id = newId }, new { id = newId, name });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpsertDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required.");

            var ok = _service.UpdateUser(id, dto.Name.Trim());
            if (!ok) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var ok = _service.DeleteUser(id);
            if (!ok) return NotFound();

            return NoContent();
        }
    }
}
