using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PA.API.Data;
using PA.API.Models;

namespace PA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ChatController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Message>>> GetUserMessages(string userId)
    {
        return await _context.Messages
            .Where(m => m.SenderId == userId || m.ReceiverId == userId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    [HttpGet("admin/users")]
    public async Task<ActionResult<IEnumerable<string>>> GetAdminChatUsers()
    {
        return await _context.Messages
            .Where(m => m.ReceiverId == "admin")
            .Select(m => m.SenderId)
            .Distinct()
            .ToListAsync();
    }

    [HttpGet("conversation/{userId1}/{userId2}")]
    public async Task<ActionResult<IEnumerable<Message>>> GetConversation(string userId1, string userId2)
    {
        return await _context.Messages
            .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                       (m.SenderId == userId2 && m.ReceiverId == userId1))
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Message>> SendMessage(Message message)
    {
        message.Id = Guid.NewGuid().ToString();
        message.CreatedAt = DateTime.UtcNow;
        message.IsRead = false;

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserMessages), new { userId = message.SenderId }, message);
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkMessageAsRead(string id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message == null)
        {
            return NotFound();
        }

        message.IsRead = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }
} 