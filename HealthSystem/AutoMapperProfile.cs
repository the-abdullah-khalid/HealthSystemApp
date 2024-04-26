using AutoMapper;
using HealthSystemApp.DTOs.HealthRegionDTOs;
using HealthSystemApp.DTOs.HealthSystemDTOs;
using HealthSystemApp.DTOs.OrganizationDTOs;
using HealthSystemApp.Models.Domain;

namespace HealthSystemApp
{
    public class AutoMapperProfile: Profile
    {
         public AutoMapperProfile()
         {
            //health system mappings
              CreateMap<HealthSystem,AddHealthSystemDTO>().ReverseMap();
              CreateMap<HealthSystem, GetHealthSystemDTO>().ReverseMap();
              CreateMap<HealthSystem, UpdateHealthSystemDTO>().ReverseMap();
              CreateMap<HealthSystem,HealthSystemsDTO > ().ReverseMap();
              CreateMap<HealthSystem, DeleteHealthSystemDTO>().ReverseMap();
            //health region mappings
              CreateMap<HealthRegion, HealthRegionsDTO>().ReverseMap();
              CreateMap<HealthRegion,AddHealthRegionDTO>().ReverseMap();
              CreateMap<HealthRegion, GetHealthRegionDTO>().ReverseMap();
              CreateMap<HealthRegion, DeleteHealthRegionDTO>().ReverseMap();
              CreateMap<HealthRegion, UpdateHealthRegionDTO>().ReverseMap();

            //organization mappings
              CreateMap<Organization, AddOrganizationDTO>().ReverseMap();
              CreateMap<Organization, GetOrganizationDTO>().ReverseMap();
              CreateMap<Organization, UpdateOrganizationDTO>().ReverseMap();
        }
       
    }
}
