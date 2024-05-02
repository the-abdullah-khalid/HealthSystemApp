using AutoMapper;
using HealthSystemApp.DTOs.OrganizationDTOs;
using HealthSystemApp.Interfaces;
using HealthSystemApp.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HealthSystemApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IOrganization organizationRepository;

        public OrganizationController(IMapper mapper, IOrganization organizationRepository)
        {
            this.mapper = mapper;
            this.organizationRepository = organizationRepository;
        }

        [HttpPost]
        [Authorize(Policy = "AdministratorOnly")]
        public async Task<IActionResult> AddOrganization([FromBody] AddOrganizationDTO addOrganizationDTO) 
        {
     
            var organization= mapper.Map<Organization>(addOrganizationDTO);

            organization=await organizationRepository.Create(organization, addOrganizationDTO.healthRegionId);
            if (organization == null)
            {
                return NotFound("Health Region Id entered doesnt exist");
            }

            var orgDto=mapper.Map<GetOrganizationDTO>(organization);

            string json = JsonConvert.SerializeObject(orgDto, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Ok(json);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Policy = "AdministratorOrHealthSystemAdminOrHealthRegionAdminOrOrganizationAdmin")]
        public async Task<IActionResult> GetOrganizationByID([FromRoute] Guid id)
        {
            var organization=await organizationRepository.Get(id);
            if(organization == null)
            {
                return NotFound("Org not found in DB with ID: " + id);
            }

            var orgDTO=mapper.Map<GetOrganizationDTO>(organization);

            return Ok(orgDTO);
        }

        [HttpGet]
        [Authorize(Policy = "AdministratorOnly")]
        public async Task<IActionResult> GetAllOrganizations()
        {
            var listOfOrganizations= await organizationRepository.GetAll();

            var listOfOrganizationsDto = mapper.Map<List<GetOrganizationDTO>>(listOfOrganizations);

            return Ok(listOfOrganizationsDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Policy = "AdministratorOrHealthSystemAdminOrHealthRegionAdminOrOrganizationAdmin")]
        public async Task<IActionResult> DeleteOrganizationById([FromRoute] Guid id)
        {
            var deletedOrganization = await organizationRepository.Delete(id);
            if(deletedOrganization == null)
            {
                return NotFound("Org not found in DB with ID: " + id);
            }

            return Ok(deletedOrganization);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Policy = "AdministratorOrHealthSystemAdminOrHealthRegionAdminOrOrganizationAdmin")]
        public async Task<IActionResult> UpdateOrganizationById([FromRoute] Guid id, [FromBody] UpdateOrganizationDTO updateOrganizationDTO)
        {
            var orgDomain = mapper.Map<Organization>(updateOrganizationDTO);

            var updatedOrganization=await organizationRepository.Update(id, orgDomain,updateOrganizationDTO.healthRegionId);
            if(updatedOrganization == null)
            {
                return NotFound();
            }

            return Ok(updatedOrganization);
        }

    }
}
