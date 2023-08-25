namespace MovieWebApp.DTO
{
    public class DvdStatusDTO
    {
        public string? Title { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public int? customerSubscriptionId { get; set; }
    }
}
