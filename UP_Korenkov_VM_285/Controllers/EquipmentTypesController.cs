using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UP_Korenkov_VM_285.Data;
using UP_Korenkov_VM_285.Models;

namespace UP_Korenkov_VM_285.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EquipmentTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EquipmentTypesController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentType>>> GetEquipmentTypes()
        {
            var types = await _context.EquipmentTypes
                .OrderBy(t => t.Name)
                .ToListAsync();

            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentType>> GetEquipmentType(int id)
        {
            var type = await _context.EquipmentTypes.FindAsync(id);

            if (type == null)
                return NotFound(new { message = "Тип оборудования не найден" });

            return Ok(type);
        }
    }
}