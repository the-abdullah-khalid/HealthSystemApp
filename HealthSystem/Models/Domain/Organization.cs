namespace HealthSystemApp.Models.Domain
{
    public class Organization
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        //adding navigational properties
        public List<HealthRegion> HealthRegions { get; set; } = [];

        public List<HealthRegionOrganization> HealthRegionOrganizations { get; } = [];
    }
}
