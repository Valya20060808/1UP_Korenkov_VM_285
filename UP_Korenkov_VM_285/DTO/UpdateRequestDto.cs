namespace UP_Korenkov_VM_285.DTO
{
    public class UpdateRequestDto
    {
        public int? MasterId { get; set; }
        public int? RequestStatusId { get; set; }
        public string? RepairParts { get; set; }
        public string? ProblemDescription { get; set; }
    }
}