using HealthSystemApp.Models.Domain;

namespace HealthSystemApp.Interfaces
{
    public interface IHealthRegion
    {
        Task<List<HealthRegion>> GetAll();
        Task<HealthRegion?> GetSingleRegion(Guid id);
        Task<HealthRegion?> Create(HealthRegion healthRegion,Guid healthSystemId);
        Task<HealthRegion?> Delete(Guid id);
        Task<HealthRegion?> Update(Guid Id,HealthRegion healthRegion,Guid healthSystemId);
    }
}
