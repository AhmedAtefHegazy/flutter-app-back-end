using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PA.API.Data;
using PA.API.Models;

namespace PA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems(string userId)
    {
        return await _context.CartItems
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<CartItem>> AddToCart(CartItem cartItem)
    {
        var existingItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.UserId == cartItem.UserId && c.ProductId == cartItem.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += cartItem.Quantity;
            await _context.SaveChangesAsync();
            return existingItem;
        }

        cartItem.Id = Guid.NewGuid().ToString();
        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCartItems), new { userId = cartItem.UserId }, cartItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCartItem(string id, CartItem cartItem)
    {
        if (id != cartItem.Id)
        {
            return BadRequest();
        }

        _context.Entry(cartItem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CartItemExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveFromCart(string id)
    {
        var cartItem = await _context.CartItems.FindAsync(id);
        if (cartItem == null)
        {
            return NotFound();
        }

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("user/{userId}")]
    public async Task<IActionResult> ClearCart(string userId)
    {
        var cartItems = await _context.CartItems
            .Where(c => c.UserId == userId)
            .ToListAsync();

        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CartItemExists(string id)
    {
        return _context.CartItems.Any(e => e.Id == id);
    }
} 