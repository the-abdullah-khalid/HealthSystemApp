namespace HealthSystemApp.DTOs.HealthRegionDTOs
{
    public class UpdateHealthRegionDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        //to which health system ,the health region belongs to 
        public Guid HealthSystemId { get; set; }
    }
}
