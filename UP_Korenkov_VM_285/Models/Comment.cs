using UP_Korenkov_VM_285.Models;

namespace UP_Korenkov_VM_285.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;

        public int MasterId { get; set; }
        public User? Master { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RequestId { get; set; }
        public Request? Request { get; set; }
    }
}