using AutoMapper;
using HealthSystemApp.Data;
using HealthSystemApp.DTOs.HealthRegionDTOs;
using HealthSystemApp.Interfaces;
using HealthSystemApp.Models.Domain;
using HealthSystemApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using System.Xml;

namespace HealthSystemApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class HealthRegionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHealthRegion _healthRegionRepository;

        public HealthRegionController(IMapper mapper, IHealthRegion healthRegionRepository)
        {
            this._mapper = mapper;
            this._healthRegionRepository = healthRegionRepository;
        }

        [HttpGet]
        [Authorize(Policy = "AdministratorOnly")]
        public async Task<IActionResult> GetAllHealthRegions()
        {
            var listOfHealthRegions = await _healthRegionRepository.GetAll();

            var healthRegionsDto= _mapper.Map<List<HealthRegionsDTO>>(listOfHealthRegions);

            string json = JsonConvert.SerializeObject(healthRegionsDto, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Ok(json);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Policy = "AdministratorOrHealthSystemAdminOrHealthRegionAdmin")]
        public async Task<IActionResult> GetHealthRegionById([FromRoute] Guid id)
        {
            var region=await _healthRegionRepository.GetSingleRegion(id);
            if(region == null)
            {
                return NotFound(id);
            }
            var regionDto=_mapper.Map<GetHealthRegionDTO>(region);

            return Ok(regionDto);
        }

        [HttpPost]
        [Authorize(Policy = "AdministratorOnly")]
        public async Task<IActionResult> CreateHealthRegion([FromBody] AddHealthRegionDTO addHealthRegionDTO)
        {
            if (addHealthRegionDTO == null)
            {
                return BadRequest("The request body cannot be empty.");
            }
            

            var healthRegionToBeAdded= _mapper.Map<HealthRegion>(addHealthRegionDTO);

            healthRegionToBeAdded = await _healthRegionRepository.Create(healthRegionToBeAdded,addHealthRegionDTO.HealthSystemId);

            var added=_mapper.Map<AddHealthRegionDTO>(healthRegionToBeAdded);


            return Ok(added);
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Policy = "AdministratorOrHealthSystemAdminOrHealthRegionAdmin")]
        public async Task<IActionResult> DeleteHealthRegionById([FromRoute] Guid id)
        {
            var deletedRegion=await _healthRegionRepository.Delete(id);
            if (deletedRegion == null)
            {
                return NotFound() ;
            }
            var deletedRegionDto=_mapper.Map<DeleteHealthRegionDTO>(deletedRegion);

            return Ok(deletedRegionDto);
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Policy = "AdministratorOrHealthSystemAdminOrHealthRegionAdmin")]
        public async Task<IActionResult> UpdateHealthRegion([FromRoute] Guid id, [FromBody] UpdateHealthRegionDTO updateHealthRegionDTO)
        {
            if(updateHealthRegionDTO==null)
            {
                return BadRequest("The request body cannot be empty.");
            }

            var updatedRegion = _mapper.Map<HealthRegion>(updateHealthRegionDTO);
            updatedRegion=await _healthRegionRepository.Update(id, updatedRegion, updateHealthRegionDTO.HealthSystemId);
            if(updatedRegion == null)
            {
                return NotFound("Region ID entered doesn't exist");
            }

            return Ok(updatedRegion);
        }
    }


}
