using System.ComponentModel.DataAnnotations;

namespace UP_Korenkov_VM_285.DTO
{
    public class UpdateProfileDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Login { get; set; } = string.Empty;
    }
}