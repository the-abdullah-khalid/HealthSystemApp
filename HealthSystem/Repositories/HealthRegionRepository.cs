using AutoMapper;
using AutoMapper.Configuration.Annotations;
using HealthSystemApp.Data;
using HealthSystemApp.DTOs.HealthRegionDTOs;
using HealthSystemApp.Interfaces;
using HealthSystemApp.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace HealthSystemApp.Repositories
{
    public class HealthRegionRepository : IHealthRegion
    {
        private readonly HealthSystemDbContext healthSystemDb;
        private readonly IMapper mapper;


        public HealthRegionRepository(HealthSystemDbContext healthSystemDb, IMapper mapper)
        {
            this.healthSystemDb = healthSystemDb;
            this.mapper = mapper;
        }

        public async Task<HealthRegion?> Create(HealthRegion healthRegion, Guid healthSystemId)
        {
            if (await healthSystemDb.healthSystems.FindAsync(healthSystemId) == null)
            {
                return null;
            }

            await healthSystemDb.healthRegions.AddAsync(healthRegion);
            await healthSystemDb.SaveChangesAsync();

            var relation = new HealthSystemHealthRegion
            {
                HealthSystemId = healthSystemId,
                HealthRegionId = healthRegion.Id,
                FromDate = DateTime.Now
            };
            await healthSystemDb.healthSystemHealthRegions.AddAsync(relation);
            await healthSystemDb.SaveChangesAsync() ;

            //await healthSystemDb.healthSystemHealthRegions

            return healthRegion;
        }

        public async Task<HealthRegion?> Delete(Guid id)
        {
            var DeletedRegion= await healthSystemDb.healthRegions.FirstOrDefaultAsync(hr => hr.Id == id);
            if (DeletedRegion == null)
            {
                return null;
            }

            healthSystemDb.healthRegions.Remove(DeletedRegion);
            await healthSystemDb.SaveChangesAsync();

            return DeletedRegion;
        }

        public async Task<List<HealthRegion>> GetAll()
        {
            var listOfHealthRegions = await healthSystemDb.healthRegions
            .Include(hr => hr.HealthSystems)
            .ToListAsync();

            var healthRegions = listOfHealthRegions.Select(hr => new HealthRegion
            {
                Id = hr.Id,
                Name = hr.Name,
                Description = hr.Description,
                HealthSystems = hr.HealthSystems.Select(hs => new HealthSystem
                {
                    Id = hs.Id,
                    Name = hs.Name,
                    Description = hs.Description
                }).ToList()

            }).ToList();

            return healthRegions;

        }

        public async Task<HealthRegion?> GetSingleRegion(Guid id)
        {
            var region=await healthSystemDb.healthRegions.Include(hr => hr.HealthSystems).FirstOrDefaultAsync(hr=>hr.Id==id);
            if(region == null)
            {
                return null;
            }
                region = new HealthRegion
                {
                    Id = region.Id,
                    Name = region.Name,
                    Description = region.Description,
                    HealthSystems = region.HealthSystems.Select(hs => new HealthSystem
                    {
                        Id = hs.Id,
                        Name = hs.Name,
                        Description = hs.Description
                    }).ToList()
                };
           return region;
        }

        public async Task<HealthRegion?> Update(Guid Id,HealthRegion healthRegion, Guid healthSystemId)
        {
            if (await healthSystemDb.healthSystems.FindAsync(healthSystemId) == null)
            {
                return null;
            }

            var updateHealthRegion=await healthSystemDb.healthRegions.Include(hr => hr.HealthSystems).FirstOrDefaultAsync(hr => hr.Id==Id);
            if (updateHealthRegion == null)
            {
                return null ;
            }

            //updating entities region table
            updateHealthRegion.Name=healthRegion.Name;
            updateHealthRegion.Description=healthRegion.Description;

            //updating relational table
            var relation=await healthSystemDb.healthSystemHealthRegions.FirstOrDefaultAsync(r=>r.HealthRegionId==updateHealthRegion.Id);
            if (relation != null)
            {
                healthSystemDb.healthSystemHealthRegions.Remove(relation);
            }

            relation = new HealthSystemHealthRegion
            {
                HealthSystemId = healthSystemId,
                HealthRegionId = updateHealthRegion.Id,
                FromDate = DateTime.Now
            };
            await healthSystemDb.healthSystemHealthRegions.AddAsync(relation);
            await healthSystemDb.SaveChangesAsync();


            updateHealthRegion = new HealthRegion
            {
                Id = updateHealthRegion.Id,
                Name = updateHealthRegion.Name,
                Description = updateHealthRegion.Description,
                HealthSystems = updateHealthRegion.HealthSystems.Select(hs => new HealthSystem
                {
                    Id = hs.Id,
                    Name = hs.Name,
                    Description = hs.Description
                }).ToList()
            };
            return updateHealthRegion;
        }
    }
}


