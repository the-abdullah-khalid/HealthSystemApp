using HealthSystemApp.Models.Domain;

namespace HealthSystemApp.Interfaces
{
    public interface IHealthSystem
    {
        Task<IEnumerable<HealthSystem>> GetAllHealthSystemsAsync();
        Task<HealthSystem?> GetHealthSystemByIdAsync(Guid id);
        Task<HealthSystem> AddHealthSystemAsync(HealthSystem healthSystem);
        Task<HealthSystem?> UpdateHealthSystemAsync(Guid id,HealthSystem healthSystem);
        Task DeleteHealthSystemAsync(Guid id);
    }
}
