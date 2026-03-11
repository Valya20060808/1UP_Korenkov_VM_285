using System.ComponentModel.DataAnnotations;

namespace UP_Korenkov_VM_285.DTO
{
    public class LoginDto
    {
        [Required]
        public string Login { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}