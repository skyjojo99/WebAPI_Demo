using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebAPI_Demo.Models;
using WebAPI_Demo.Dtos;
using System.Data;

namespace WebAPI_Demo.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly string _connStr;
        public UsersController(IConfiguration configuration)
        {
            _connStr = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            User user = null;
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                SqlCommand cmd = new SqlCommand("usp_GetUserById", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new User
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                    };
                }

            }
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserUpsertDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Name is required.");
            }
            int newId;
            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand("usp_CreateUser", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", dto.Name.Trim());

                conn.Open();
                object result = cmd.ExecuteScalar(); // SCCOPE_IDENTITY();
                newId = Convert.ToInt32(result);
            }
            return CreatedAtAction(nameof(GetUser), new { id = newId }, new { Id = newId, name = dto.Name.Trim() }); 
        }
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpsertDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("Name is required.");
            }
            int affectedRows;
            using (SqlConnection conn = new SqlConnection(_connStr))
            using (SqlCommand cmd = new SqlCommand("usp_UpdateUser", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", dto.Name.Trim());
                conn.Open();
                affectedRows = Convert.ToInt32(cmd.ExecuteScalar()); // @@ROW_COUNT;
            }
            if (affectedRows == 0)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            int affectedRows;
            using (SqlConnection conn = new SqlConnection(_connStr))
            using(SqlCommand cmd = new SqlCommand("usp_DeleteUser", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                affectedRows = Convert.ToInt32(cmd.ExecuteScalar()); // @@ROW_COUNT;
            }
            if (affectedRows == 0)
                return NotFound();
            
            return NoContent();  //204 No Content
        }
    }

}
