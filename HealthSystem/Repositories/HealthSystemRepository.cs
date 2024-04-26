using HealthSystemApp.Data;
using HealthSystemApp.Interfaces;
using HealthSystemApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApp.Repositories
{
    public class HealthSystemRepository : IHealthSystem
    {
        private readonly HealthSystemDbContext _context;

        public HealthSystemRepository(HealthSystemDbContext context)
        {
            this._context = context;
        }
        public async Task<HealthSystem> AddHealthSystemAsync(HealthSystem healthSystem)
        {
            await _context.healthSystems.AddAsync(healthSystem);
            await _context.SaveChangesAsync();
            return healthSystem;
        }

        public async Task DeleteHealthSystemAsync(Guid id)
        {
            var healthSystem = await _context.healthSystems.FirstOrDefaultAsync(a => a.Id == id);
            if (healthSystem != null)
            {
                _context.healthSystems.Remove(healthSystem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<HealthSystem>> GetAllHealthSystemsAsync()
        {
            return await _context.healthSystems.ToListAsync();
        }

        public async Task<HealthSystem?> GetHealthSystemByIdAsync(Guid id)
        {
            return await _context.healthSystems
            .Include(s => s.HealthRegions)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<HealthSystem?> UpdateHealthSystemAsync(Guid id,HealthSystem healthSystem)
        {
            var ExistingHealthSystem = await _context.healthSystems.FirstOrDefaultAsync(a => a.Id == id);

            if (ExistingHealthSystem == null)
                return null;

            ExistingHealthSystem.Name = healthSystem.Name;
            ExistingHealthSystem.Description = healthSystem.Description;

            await _context.SaveChangesAsync();
            return ExistingHealthSystem;
        }
    }
}
