using HealthSystemApp.Models.Domain;

namespace HealthSystemApp.DTOs.HealthSystemDTOs
{
    public class DeleteHealthSystemDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        //adding navigational properties

        public List<HealthRegion> HealthRegions { get; } = [];
    }
}
