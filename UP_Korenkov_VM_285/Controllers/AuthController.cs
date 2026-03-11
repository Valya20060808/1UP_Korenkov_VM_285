using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UP_Korenkov_VM_285.Data;
using UP_Korenkov_VM_285.DTO;
using UP_Korenkov_VM_285.Models;

namespace UP_Korenkov_VM_285.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Login == loginDto.Login);

          
            if (user == null)
            {
                return Unauthorized(new { message = "Неверный логин или пароль" });
            }

           
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Неверный логин или пароль" });
            }

           
            var token = GenerateJwtToken(user);

          
            var response = new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Login = user.Login,
                Role = user.Role?.Name ?? "Неизвестно",
                Token = token
            };

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Login == registerDto.Login);

            if (existingUser != null)
            {
                return BadRequest(new { message = "Пользователь с таким логином уже существует" });
            }

          
            var user = new User
            {
                FullName = registerDto.FullName,
                Phone = registerDto.Phone,
                Login = registerDto.Login,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                RoleId = registerDto.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

        
            await _context.Entry(user).Reference(u => u.Role).LoadAsync();

     
            var userDto = new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Phone = user.Phone,
                Login = user.Login,
                Role = user.Role?.Name ?? ""
            };

            return Ok(userDto);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "YourSecretKeyHereMakeItLongAndSecure12345");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Login),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? ""),
                new Claim("userId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}