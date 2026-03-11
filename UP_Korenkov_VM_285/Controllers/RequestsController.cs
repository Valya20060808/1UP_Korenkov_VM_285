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
    public class RequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestDto>>> GetRequests([FromQuery] int? statusId)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            IQueryable<Request> query = _context.Requests
                .Include(r => r.EquipmentType)
                .Include(r => r.RequestStatus)
                .Include(r => r.Master)
                .Include(r => r.Client)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.Master);

            if (userRole == "Заказчик")
            {
                query = query.Where(r => r.ClientId == userId);
            }
            else if (userRole == "Специалист")
            {
                query = query.Where(r => r.MasterId == userId);
            }

            if (statusId.HasValue)
            {
                query = query.Where(r => r.RequestStatusId == statusId.Value);
            }

            var requests = await query
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();

            return Ok(requests.Select(MapToDto));
        }


        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<RequestDto>>> GetMyRequests()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var requests = await _context.Requests
                .Include(r => r.EquipmentType)
                .Include(r => r.RequestStatus)
                .Include(r => r.Master)
                .Include(r => r.Client)
                .Where(r => r.ClientId == userId || r.MasterId == userId)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();

            return Ok(requests.Select(MapToDto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RequestDto>> GetRequest(int id)
        {
            var request = await _context.Requests
                .Include(r => r.EquipmentType)
                .Include(r => r.RequestStatus)
                .Include(r => r.Master)
                .Include(r => r.Client)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.Master)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return NotFound(new { message = "Заявка не найдена" });

            return Ok(MapToDto(request));
        }

        [HttpGet("statuses")]
        public async Task<ActionResult<IEnumerable<RequestStatus>>> GetStatuses()
        {
            var statuses = await _context.RequestStatuses
                .OrderBy(s => s.Id)
                .ToListAsync();
            return Ok(statuses);
        }

        [HttpPost]
        [Authorize(Roles = "Оператор,Менеджер")]
        public async Task<ActionResult<RequestDto>> CreateRequest(CreateRequestDto createDto)
        {
            var request = new Request
            {
                StartDate = createDto.StartDate.ToUniversalTime(),
                EquipmentTypeId = createDto.EquipmentTypeId,
                EquipmentModel = createDto.EquipmentModel,
                ProblemDescription = createDto.ProblemDescription,
                RequestStatusId = createDto.RequestStatusId,
                ClientId = createDto.ClientId
            };

            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            var createdRequest = await _context.Requests
                .Include(r => r.EquipmentType)
                .Include(r => r.RequestStatus)
                .Include(r => r.Client)
                .FirstOrDefaultAsync(r => r.Id == request.Id);

            return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, MapToDto(createdRequest!));
        }

        [HttpPut("{id}/assign-master")]
        [Authorize(Roles = "Оператор,Менеджер,Менеджер по качеству")]
        public async Task<IActionResult> AssignMaster(int id, [FromBody] AssignMasterDto dto)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
                return NotFound(new { message = "Заявка не найдена" });

            var master = await _context.Users.FindAsync(dto.MasterId);
            if (master == null || master.RoleId != 2) 
                return BadRequest(new { message = "Указанный пользователь не является специалистом" });

            request.MasterId = dto.MasterId;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Специалист назначен" });
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] int statusId)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
                return NotFound(new { message = "Заявка не найдена" });

            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

 
            if (userRole == "Специалист" && request.MasterId != userId)
                return Forbid();



            request.RequestStatusId = statusId;

            if (statusId == 2 && request.CompletionDate == null)
            {
                request.CompletionDate = DateTime.UtcNow;
            }

            else if (statusId != 2 && request.CompletionDate != null)
            {
                request.CompletionDate = null;
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Статус обновлён" });
        }

        [HttpPut("{id}/extend-deadline")]
        [Authorize(Roles = "Менеджер по качеству")]
        public async Task<IActionResult> ExtendDeadline(int id, [FromBody] ExtendDeadlineDto dto)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
                return NotFound(new { message = "Заявка не найдена" });

            request.Deadline = dto.NewDeadline.ToUniversalTime();
            await _context.SaveChangesAsync();

            return Ok(new { message = "Срок выполнения продлён" });
        }

        [HttpPost("{id}/comments")]
        public async Task<ActionResult<CommentDto>> AddComment(int id, [FromBody] AddCommentDto dto)
        {
            var request = await _context.Requests
                .Include(r => r.Master)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
                return NotFound(new { message = "Заявка не найдена" });

            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            bool canComment = (userRole == "Специалист" && request.MasterId == userId) ||
                              userRole == "Менеджер" ||
                              userRole == "Менеджер по качеству" ||
                              userRole == "Оператор";

            if (!canComment)
                return StatusCode(403, new { message = "У вас нет прав для добавления комментария к этой заявке" });

            var comment = new Comment
            {
                Message = dto.Message,
                MasterId = userId,
                RequestId = id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            await _context.Entry(comment).Reference(c => c.Master).LoadAsync();

            var commentDto = new CommentDto
            {
                Id = comment.Id,
                Message = comment.Message,
                MasterName = comment.Master?.FullName ?? "",
                MasterId = comment.MasterId,
                RequestId = comment.RequestId,
                CreatedAt = comment.CreatedAt
            };

            return Ok(commentDto);
        }

        [HttpGet("statistics")]
        [Authorize(Roles = "Менеджер,Менеджер по качеству")]
        public async Task<ActionResult<StatisticsDto>> GetStatistics()
        {
            var requests = await _context.Requests
                .Include(r => r.RequestStatus)
                .Include(r => r.EquipmentType)
                .ToListAsync();

            var completedRequests = requests.Where(r => r.CompletionDate != null).ToList();

            var statusStats = requests
                .GroupBy(r => r.RequestStatus?.Name ?? "Неизвестно")
                .ToDictionary(g => g.Key, g => g.Count());

            var typeStats = requests
                .GroupBy(r => r.EquipmentType?.Name ?? "Неизвестно")
                .ToDictionary(g => g.Key, g => g.Count());

            var statistics = new StatisticsDto
            {
                TotalRequests = requests.Count,
                CompletedRequests = completedRequests.Count,
                AverageCompletionDays = completedRequests.Any()
                    ? completedRequests.Average(r => (r.CompletionDate!.Value - r.StartDate).TotalDays)
                    : 0,
                RequestsByStatus = statusStats,
                RequestsByEquipmentType = typeStats
            };

            return Ok(statistics);
        }

        private RequestDto MapToDto(Request request)
        {
            return new RequestDto
            {
                Id = request.Id,
                StartDate = request.StartDate,
                EquipmentType = request.EquipmentType?.Name ?? "",
                EquipmentTypeId = request.EquipmentTypeId,
                EquipmentModel = request.EquipmentModel,
                ProblemDescription = request.ProblemDescription,
                RequestStatus = request.RequestStatus?.Name ?? "",
                RequestStatusId = request.RequestStatusId,
                CompletionDate = request.CompletionDate,
                RepairParts = request.RepairParts,
                MasterId = request.MasterId,
                MasterName = request.Master?.FullName,
                ClientId = request.ClientId,
                ClientName = request.Client?.FullName ?? "",
                Deadline = request.Deadline,
                Comments = request.Comments?.Select(c => new CommentDto
                {
                    Id = c.Id,
                    Message = c.Message,
                    MasterName = c.Master?.FullName ?? "",
                    MasterId = c.MasterId,
                    RequestId = c.RequestId,
                    CreatedAt = c.CreatedAt
                }).ToList()
            };
        }
    }
}