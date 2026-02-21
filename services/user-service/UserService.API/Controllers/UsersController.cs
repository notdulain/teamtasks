using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly string _connectionString;

    public UsersController(string connectionString)
    {
        _connectionString = connectionString;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = new List<object>();
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new SqlCommand("SELECT id, name, email FROM users", conn);
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            users.Add(new { id = reader["id"], name = reader["name"], email = reader["email"] });
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new SqlCommand("SELECT id, name, email FROM users WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return NotFound();
        return Ok(new { id = reader["id"], name = reader["name"], email = reader["email"] });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new SqlCommand(
            "INSERT INTO users (name, email) OUTPUT INSERTED.id VALUES (@name, @email)", conn);
        cmd.Parameters.AddWithValue("@name", request.Name);
        cmd.Parameters.AddWithValue("@email", request.Email);
        var id = await cmd.ExecuteScalarAsync();
        return CreatedAtAction(nameof(GetById), new { id }, new { id, request.Name, request.Email });
    }
}

public record CreateUserRequest(string Name, string Email);
