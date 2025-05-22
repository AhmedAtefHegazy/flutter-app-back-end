using System.ComponentModel.DataAnnotations;

namespace PA.API.Models;

public class CartItem
{
    public string Id { get; set; } = string.Empty;
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    [Required]
    public string ProductId { get; set; } = string.Empty;
    
    [Required]
    public string ProductName { get; set; } = string.Empty;
    
    [Required]
    public string ProductImage { get; set; } = string.Empty;
    
    [Required]
    [Range(0, double.MaxValue)]
    public double Price { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
} 