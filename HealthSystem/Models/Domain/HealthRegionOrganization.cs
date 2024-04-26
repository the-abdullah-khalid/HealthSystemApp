namespace HealthSystemApp.Models.Domain
{
    public class HealthRegionOrganization
    {
        public Guid HealthRegionId { get; set; }
        public Guid OrganizationId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; } = null;

        public HealthRegion HealthRegion { get; set; } = null!;
        public Organization Organization { get; set; } = null!;
    }
}
