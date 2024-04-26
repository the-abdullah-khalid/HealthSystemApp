
namespace HealthSystemApp.Models.Domain
{
    public class HealthSystem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        //adding navigational properties

        public List<HealthRegion> HealthRegions { get; } = [];

        public List<HealthSystemHealthRegion> HealthSystemHealthRegions { get; } = [];

    }
}
