using medgem_backend.Data;
using medgem_backend.DTOs;
using medgem_backend.Models;
using medgem_backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace medgem_backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AppDbContext db, JwtService jwt) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest req)
    {
        var user = await db.AdminUsers.FirstOrDefaultAsync(u => u.Email == req.Email && u.IsActive);
        if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return Unauthorized(new ApiResponse<LoginResponse>(false, "Invalid credentials", null));

        user.LastLoginAt = DateTime.UtcNow;
        await db.SaveChangesAsync();

        var token = jwt.GenerateToken(user);
        return Ok(new ApiResponse<LoginResponse>(true, "Login successful",
            new LoginResponse(token, user.Email, user.FullName, user.Role)));
    }
}
