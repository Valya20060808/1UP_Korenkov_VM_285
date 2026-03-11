namespace UP_Korenkov_VM_285.DTO
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string MasterName { get; set; } = string.Empty;
        public int MasterId { get; set; }
        public int RequestId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}