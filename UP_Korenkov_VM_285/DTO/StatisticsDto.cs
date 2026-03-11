namespace UP_Korenkov_VM_285.DTO
{
    public class StatisticsDto
    {
        public int TotalRequests { get; set; }
        public int CompletedRequests { get; set; }
        public double AverageCompletionDays { get; set; }
        public Dictionary<string, int> RequestsByStatus { get; set; } = new();
        public Dictionary<string, int> RequestsByEquipmentType { get; set; } = new();
    }
}