using System.ComponentModel.DataAnnotations;

namespace PA.API.Models;

public class Message
{
    public string Id { get; set; } = string.Empty;
    
    [Required]
    public string SenderId { get; set; } = string.Empty;
    
    [Required]
    public string ReceiverId { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    public string? ImageUrl { get; set; }
    
    public bool IsRead { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 