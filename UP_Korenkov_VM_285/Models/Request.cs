using UP_Korenkov_VM_285.Models;
using System;

namespace UP_Korenkov_VM_285.Models
{
    public class Request
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }

        public int EquipmentTypeId { get; set; }
        public EquipmentType? EquipmentType { get; set; }

        public string EquipmentModel { get; set; } = string.Empty;
        public string ProblemDescription { get; set; } = string.Empty;

        public int RequestStatusId { get; set; }
        public RequestStatus? RequestStatus { get; set; }

        public DateTime? CompletionDate { get; set; }
        public string? RepairParts { get; set; }

        public int? MasterId { get; set; }
        public User? Master { get; set; }

        public int ClientId { get; set; }
        public User? Client { get; set; }
        public DateTime? Deadline { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}