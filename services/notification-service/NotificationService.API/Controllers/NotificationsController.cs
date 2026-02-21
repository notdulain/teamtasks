using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace NotificationService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly string _connectionString;

    public NotificationsController(string connectionString)
    {
        _connectionString = connectionString;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notifications = new List<object>();
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new SqlCommand("SELECT id, message, created_at FROM notifications", conn);
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            notifications.Add(new { id = reader["id"], message = reader["message"], createdAt = reader["created_at"] });
        return Ok(notifications);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNotificationRequest request)
    {
        using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();
        using var cmd = new SqlCommand(
            "INSERT INTO notifications (message, created_at) OUTPUT INSERTED.id VALUES (@message, GETUTCDATE())",
            conn);
        cmd.Parameters.AddWithValue("@message", request.Message);
        var id = await cmd.ExecuteScalarAsync();
        return Ok(new { id, request.Message });
    }
}

public record CreateNotificationRequest(string Message);
