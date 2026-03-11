using System.ComponentModel.DataAnnotations;

namespace UP_Korenkov_VM_285.DTO
{
    public class CreateRequestDto
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public int EquipmentTypeId { get; set; }

        [Required]
        [StringLength(200)]
        public string EquipmentModel { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string ProblemDescription { get; set; } = string.Empty;

        [Required]
        public int RequestStatusId { get; set; } = 3;

        [Required]
        public int ClientId { get; set; }
    }
}