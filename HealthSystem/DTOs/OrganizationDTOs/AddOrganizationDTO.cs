namespace HealthSystemApp.DTOs.OrganizationDTOs
{
    public class AddOrganizationDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        //to which healthregion the organization belongs to
        public Guid healthRegionId {  get; set; }
    }
}
