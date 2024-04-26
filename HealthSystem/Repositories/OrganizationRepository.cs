using AutoMapper;
using HealthSystemApp.Data;
using HealthSystemApp.DTOs.OrganizationDTOs;
using HealthSystemApp.Interfaces;
using HealthSystemApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApp.Repositories
{
    public class OrganizationRepository:IOrganization
    {
        private readonly HealthSystemDbContext healthSystemDb;
        private readonly IMapper mapper;

        public OrganizationRepository(HealthSystemDbContext healthSystemDb, IMapper mapper)
        {
            this.healthSystemDb = healthSystemDb;
            this.mapper = mapper;
        }

        public async Task<Organization?> Create(Organization organization, Guid healthRegionId)
        {
            if (await healthSystemDb.healthRegions.FindAsync(healthRegionId) == null)
            {
                return null;
            }

            await healthSystemDb.organizations.AddAsync(organization);

            var relation = new HealthRegionOrganization
            {
                HealthRegionId = healthRegionId,
                OrganizationId = organization.Id,
                FromDate= DateTime.UtcNow,
            };
            await healthSystemDb.healthRegionOrganizations.AddAsync(relation);

            await healthSystemDb.SaveChangesAsync();

            //var org=await healthSystemDb.organizations.Include(org=>org.HealthRegions).FirstOrDefaultAsync(org => org.Id == organization.Id);

            organization = new Organization
            {
                Id = organization.Id,
                Name = organization.Name,
                Description = organization.Description,
                HealthRegions = organization.HealthRegions.Select(hr => new HealthRegion
                {
                    Id = hr.Id,
                    Name = hr.Name,
                    Description = hr.Description
                }).ToList()
            };

            return organization;
        }

        public async Task<Organization?> Delete(Guid id)
        {
            var OrgToBeDeleted= await healthSystemDb.organizations.Include(org => org.HealthRegions).FirstOrDefaultAsync(org => org.Id == id);
            if (OrgToBeDeleted == null)
            {
                return null;
            }
            healthSystemDb.organizations.Remove(OrgToBeDeleted);
            await healthSystemDb.SaveChangesAsync();

            OrgToBeDeleted = new Organization
            {
                Id = OrgToBeDeleted.Id,
                Name = OrgToBeDeleted.Name,
                Description = OrgToBeDeleted.Description,
                HealthRegions = OrgToBeDeleted.HealthRegions.Select(hr => new HealthRegion
                {
                    Id = hr.Id,
                    Name = hr.Name,
                    Description = hr.Description
                }).ToList()
            };

            return OrgToBeDeleted;
        }

        public async Task<Organization?> Get(Guid id)
        {
            var org = await healthSystemDb.organizations.Include(org => org.HealthRegions).AsNoTracking().FirstOrDefaultAsync(org => org.Id == id);
            if(org == null)
            {
                return null;
            }

            org = new Organization
            {
                Id = org.Id,
                Name = org.Name,
                Description = org.Description,
                HealthRegions = org.HealthRegions.Select(hr => new HealthRegion
                {
                    Id = hr.Id,
                    Name = hr.Name,
                    Description = hr.Description
                }).ToList()
            };

            return org;
        }

        public async Task<List<Organization>> GetAll()
        {
            // Query the database to retrieve all organizations
            var organizations = await healthSystemDb.organizations
                .Include(org => org.HealthRegions) // Include related HealthRegions
                .ToListAsync();

            // If there are no organizations, return an empty list
            if (organizations == null || organizations.Count == 0)
            {
                return new List<Organization>();
            }

            // Construct a new list of Organization objects with necessary properties
            var organizationList = organizations.Select(org => new Organization
            {
                Id = org.Id,
                Name = org.Name,
                Description = org.Description,
                HealthRegions = org.HealthRegions.Select(hr => new HealthRegion
                {
                    Id = hr.Id,
                    Name = hr.Name,
                    Description = hr.Description
                }).ToList()
            }).ToList();

            return organizationList;
        }

        public async Task<Organization?> Update(Guid Id, Organization organization, Guid healthRegionId)
        {
            if (await healthSystemDb.healthRegions.FindAsync(healthRegionId) == null)
            {
                return null;
            }


            var updatedOrg = await healthSystemDb.organizations.Include(org => org.HealthRegions).FirstOrDefaultAsync(org => org.Id == Id);
            if (updatedOrg == null)
            {
                return null;
            }

            updatedOrg.Name = organization.Name;
            updatedOrg.Description = organization.Description;

            if (updatedOrg.HealthRegions.Any(hr => !hr.Id.Equals(healthRegionId)))
            {
                var relation = await healthSystemDb.healthRegionOrganizations.FirstOrDefaultAsync(o => o.OrganizationId == Id);

                healthSystemDb.healthRegionOrganizations.Remove(relation);

                relation = new HealthRegionOrganization
                {
                    HealthRegionId = healthRegionId,
                    OrganizationId = updatedOrg.Id,
                    FromDate = DateTime.UtcNow,
                };
                await healthSystemDb.healthRegionOrganizations.AddAsync(relation);
            }



            await healthSystemDb.SaveChangesAsync();

            var org = await Get(Id);
            return org;
        }
    }
}
