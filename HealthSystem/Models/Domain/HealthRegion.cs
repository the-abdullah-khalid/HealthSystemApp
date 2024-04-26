namespace HealthSystemApp.Models.Domain
{
    public class HealthRegion
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        //adding navigational properties
        public List<HealthSystem> HealthSystems { get; set; } = [];

        public List<Organization> Organizations { get; } = [];

        public List<HealthSystemHealthRegion> HealthSystemHealthRegions { get; } = [];

        public List<HealthRegionOrganization> HealthRegionOrganizations { get; } = [];

    }
}
