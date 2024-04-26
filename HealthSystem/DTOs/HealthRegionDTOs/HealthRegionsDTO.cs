using HealthSystemApp.Models.Domain;

namespace HealthSystemApp.DTOs.HealthRegionDTOs
{
    public class HealthRegionsDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        //adding navigational properties
        public List<HealthSystem> HealthSystems { get; } = [];

        //public List<Organization> Organizations { get; } = [];
    }
}
