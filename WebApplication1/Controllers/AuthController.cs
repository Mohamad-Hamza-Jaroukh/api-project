using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Portfolio.API.Data;
using Portfolio.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


using Portfolio.API.DTOs;
namespace Portfolio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto model)

        {
            var admin = _context.Admins.FirstOrDefault(a => a.Username == model.Username);

            if (admin == null)
                return Unauthorized();

            var hasher = new PasswordHasher<Admin>();
            var result = hasher.VerifyHashedPassword(admin, admin.PasswordHash, model.Password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized();

            var token = GenerateJwtToken(admin);

            return Ok(new { token });
        }

        private string GenerateJwtToken(Admin admin)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, admin.Username)
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("register")]
        public IActionResult Register(RegisterDto model)
        {
            if (_context.Admins.Any())
                return BadRequest("Username already exists");

            var hasher = new PasswordHasher<Admin>();

            var admin = new Admin
            {
                Username = model.Username
            };

            admin.PasswordHash = hasher.HashPassword(admin, model.Password);

            _context.Admins.Add(admin);
            _context.SaveChanges();

            return Ok("Admin created");
        }
    }
}
