using HealthSystemApp.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace HealthSystemApp.Data
{
    public class HealthSystemDbContext: DbContext
    {
        public HealthSystemDbContext(DbContextOptions<HealthSystemDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<HealthSystem> healthSystems { get; set; }

        public DbSet<HealthRegion> healthRegions { get; set; }

        public DbSet<Organization> organizations { get; set; }

        public DbSet<HealthSystemHealthRegion> healthSystemHealthRegions { get; set; }

        public DbSet<HealthRegionOrganization> healthRegionOrganizations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("healthDB");


            modelBuilder.Entity<HealthSystem>()
                .HasMany(e => e.HealthRegions)
                .WithMany(e => e.HealthSystems)
                .UsingEntity<HealthSystemHealthRegion>();

            modelBuilder.Entity<HealthRegion>()
            .HasMany(e => e.Organizations)
            .WithMany(e => e.HealthRegions)
            .UsingEntity<HealthRegionOrganization>();
        }


    }


}
