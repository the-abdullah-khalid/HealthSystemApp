﻿using HealthSystemApp.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace HealthSystemApp.Data
{
    public class HealthSystemAuthDbContext: IdentityDbContext<ApplicationUser>
    {
        public HealthSystemAuthDbContext(DbContextOptions<HealthSystemAuthDbContext> options) : base(options)
        {

        }

        //public DbSet<ApplicationUser> applicationUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("authDB");


            base.OnModelCreating(builder);
            var administratorRoleId = Guid.NewGuid().ToString();
            var healthSystemAdminRoleId = Guid.NewGuid().ToString();
            var regionAdminRoleId = Guid.NewGuid().ToString();
            var orgAdminRoleId = Guid.NewGuid().ToString();


            var roles = new List<IdentityRole>
            {

                new IdentityRole
                {
                    Id = administratorRoleId,
                    ConcurrencyStamp = administratorRoleId,
                    Name = "Administrator",
                    NormalizedName = "Administrator".ToUpper()
                },
                new IdentityRole
                {
                    Id = healthSystemAdminRoleId,
                    ConcurrencyStamp = healthSystemAdminRoleId,
                    Name = "HealthSystemAdmin",
                    NormalizedName = "HealthSystemAdmin".ToUpper()
                },
                new IdentityRole
                {
                    Id = regionAdminRoleId,
                    ConcurrencyStamp = regionAdminRoleId,
                    Name = "RegionAdmin",
                    NormalizedName = "RegionAdmin".ToUpper()
                },
                new IdentityRole
                {
                    Id = orgAdminRoleId,
                    ConcurrencyStamp = orgAdminRoleId,
                    Name = "OrganizationAdmin",
                    NormalizedName = "OrganizationAdmin".ToUpper()
                }

            };
            builder.Entity<IdentityRole>().HasData(roles);
        }

    }
}
