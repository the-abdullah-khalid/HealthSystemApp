using HealthSystemApp.Models.Domain;

namespace HealthSystemApp.DTOs.OrganizationDTOs
{
    public class GetOrganizationDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        //adding navigational properties
        public List<HealthRegion> HealthRegions { get; } = [];
    }
}
