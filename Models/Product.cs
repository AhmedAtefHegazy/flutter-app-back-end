using System.ComponentModel.DataAnnotations;

namespace PA.API.Models;

public class Product
{
    public string Id { get; set; } = string.Empty;
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Range(0, double.MaxValue)]
    public double Price { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
    
    [Required]
    public string Category { get; set; } = string.Empty;
    
    public string ImageUrl { get; set; } = string.Empty;
    
    public bool IsAvailable { get; set; } = true;
} 