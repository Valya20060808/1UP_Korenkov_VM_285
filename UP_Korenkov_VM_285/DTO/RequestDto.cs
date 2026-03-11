namespace UP_Korenkov_VM_285.DTO
{
    public class RequestDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public string EquipmentType { get; set; } = string.Empty;
        public int EquipmentTypeId { get; set; }
        public string EquipmentModel { get; set; } = string.Empty;
        public string ProblemDescription { get; set; } = string.Empty;
        public string RequestStatus { get; set; } = string.Empty;
        public int RequestStatusId { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string? RepairParts { get; set; }
        public DateTime? Deadline { get; set; }
        public int? MasterId { get; set; }
        public string? MasterName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public List<CommentDto>? Comments { get; set; }
    }
}