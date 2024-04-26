using HealthSystemApp.Models.Domain;

namespace HealthSystemApp.DTOs.HealthRegionDTOs
{
    public class GetHealthRegionDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        //adding navigational properties
        public List<HealthSystem> HealthSystems { get; } = [];
    }
}
