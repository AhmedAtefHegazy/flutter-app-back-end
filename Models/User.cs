using System.ComponentModel.DataAnnotations;

namespace PA.API.Models;

public class User
{
    public string Id { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Role { get; set; } = "customer";
    
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
} 