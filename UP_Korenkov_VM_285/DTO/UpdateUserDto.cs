using System.ComponentModel.DataAnnotations;

namespace UP_Korenkov_VM_285.DTO
{
    public class UpdateUserDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Login { get; set; } = string.Empty;

        public string? Password { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}