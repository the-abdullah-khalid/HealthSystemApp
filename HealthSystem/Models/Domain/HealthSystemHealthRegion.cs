namespace HealthSystemApp.Models.Domain
{
    public class HealthSystemHealthRegion
    {
        public Guid HealthSystemId { get; set; }
        public Guid HealthRegionId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; } = null;
        public HealthSystem HealthSystem { get; set; } = null!;
        public HealthRegion HealthRegion { get; set; } = null!;
    }

}
