using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UP_Korenkov_VM_285.Data;
using UP_Korenkov_VM_285.DTO;
using UP_Korenkov_VM_285.Models;
using System.Security.Claims;

namespace UP_Korenkov_VM_285.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Менеджер")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .ToListAsync();

            return Ok(users.Select(MapToDto));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Менеджер")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound(new { message = "Пользователь не найден" });

            return Ok(MapToDto(user));
        }

        [HttpGet("role/{roleId}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByRole(int roleId)
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .Where(u => u.RoleId == roleId)
                .ToListAsync();

            return Ok(users.Select(MapToDto));
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetProfile()
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound(new { message = "Пользователь не найден" });

            return Ok(MapToDto(user));
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = "Пользователь не найден" });

            if (user.Login != dto.Login)
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Login == dto.Login);
                if (existingUser != null)
                    return BadRequest(new { message = "Пользователь с таким логином уже существует" });
            }

            user.FullName = dto.FullName;
            user.Phone = dto.Phone;
            user.Login = dto.Login;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Профиль обновлён" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Менеджер")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "Пользователь не найден" });

            if (user.Login != dto.Login)
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Login == dto.Login);
                if (existingUser != null)
                    return BadRequest(new { message = "Пользователь с таким логином уже существует" });
            }

            user.FullName = dto.FullName;
            user.Phone = dto.Phone;
            user.Login = dto.Login;
            user.RoleId = dto.RoleId;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Пользователь обновлён" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Менеджер")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "Пользователь не найден" });

            var currentUserId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            if (currentUserId == id)
                return BadRequest(new { message = "Нельзя удалить свою учётную запись" });

            var hasRequests = await _context.Requests
                .AnyAsync(r => r.ClientId == id || r.MasterId == id);

            if (hasRequests)
                return BadRequest(new { message = "Нельзя удалить пользователя, у которого есть заявки" });

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Пользователь удалён" });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound(new { message = "Пользователь не найден" });

            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
                return BadRequest(new { message = "Неверный текущий пароль" });

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Пароль успешно изменён" });
        }

        private UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Phone = user.Phone,
                Login = user.Login,
                Role = user.Role?.Name ?? "",
                RoleId = user.RoleId
            }; 
        }
    }
}