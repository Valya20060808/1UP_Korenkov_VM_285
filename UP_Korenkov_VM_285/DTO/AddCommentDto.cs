using System.ComponentModel.DataAnnotations;

namespace UP_Korenkov_VM_285.DTO
{
    public class AddCommentDto
    {
        [Required]
        public string Message { get; set; } = string.Empty;
    }
}