using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;


[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
  private readonly AppDbContext _context;
  private readonly IConfiguration _configuration;

  public UsersController(AppDbContext context, IConfiguration configuration)
  {
    _context = context;
    _configuration = configuration;
  }

  // POST: api/users/register
  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] User user)
  {
    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
    _context.Users.Add(user);
    await _context.SaveChangesAsync();
    return Ok(new { message = "User registered succesfully" });
  }

  // Post:  api/users/login
  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] User user)
  {
    var existingUser = _context.Users.SingleOrDefault(u => u.Email == user.Email);
    if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.PasswordHash, existingUser.PasswordHash))
    {
      return Unauthorized(new { message = "Incorrect Credentials" });
    }

    // Token
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, existingUser.UserId.ToString())
            }),
      Expires = DateTime.UtcNow.AddDays(7),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    var tokenString = tokenHandler.WriteToken(token);

    return Ok(new { Token = tokenString });
  }
}