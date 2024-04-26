using HealthSystemApp.Models.Domain;

namespace HealthSystemApp.Interfaces
{
    public interface IOrganization
    {
        Task<Organization?> Create(Organization organization, Guid healthRegionId);
        Task<Organization?> Get(Guid Id);
        Task <List<Organization>> GetAll();
        Task<Organization?> Delete(Guid Id);
        Task<Organization?> Update(Guid Id, Organization organization, Guid healthRegionId);
    }
}
