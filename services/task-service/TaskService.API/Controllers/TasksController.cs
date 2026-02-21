using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace TaskService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly string _connectionString;
    private readonly IHttpClientFactory _http;

    public TasksController(string connectionString, IHttpClientFactory http)
    {
        _connectionString = connectionString;
        _http = http;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = new List<object>();
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new SqlCommand("SELECT id, title, assigned_user_id, status FROM tasks", conn);
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            tasks.Add(new {
                id = reader["id"],
                title = reader["title"],
                assignedUserId = reader["assigned_user_id"],
                status = reader["status"]
            });
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        // Verify user exists by calling User Service
        var client = _http.CreateClient("UserService");
        var userResponse = await client.GetAsync($"/api/users/{request.AssignedUserId}");
        if (!userResponse.IsSuccessStatusCode)
            return BadRequest(new { error = "User not found" });

        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new SqlCommand(
            "INSERT INTO tasks (title, assigned_user_id, status) OUTPUT INSERTED.id VALUES (@title, @userId, 'open')",
            conn);
        cmd.Parameters.AddWithValue("@title", request.Title);
        cmd.Parameters.AddWithValue("@userId", request.AssignedUserId);
        var id = await cmd.ExecuteScalarAsync();
        return Ok(new { id, request.Title, request.AssignedUserId, status = "open" });
    }
}

public record CreateTaskRequest(string Title, int AssignedUserId);
