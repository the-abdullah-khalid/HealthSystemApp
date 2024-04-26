namespace HealthSystemApp.DTOs.OrganizationDTOs
{
    public class UpdateOrganizationDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        //to which healthregion the organization belongs to
        public Guid healthRegionId { get; set; }
    }
}
