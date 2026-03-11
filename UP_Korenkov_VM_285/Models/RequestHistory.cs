using System;

namespace UP_Korenkov_VM_285.Models
{
    public class RequestHistory
    {
        public int Id { get; set; }

        public int RequestId { get; set; }
        public Request? Request { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public string Field { get; set; } = string.Empty;
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; }

        public string Action { get; set; } = string.Empty;
    }
}