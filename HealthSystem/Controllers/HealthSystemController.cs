using AutoMapper;
using HealthSystemApp.Data;
using HealthSystemApp.DTOs.HealthSystemDTOs;
using HealthSystemApp.Interfaces;
using HealthSystemApp.Models.Domain;
using HealthSystemApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthSystemApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthSystemController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IHealthSystem healthSystemRepository;

        public HealthSystemController(IMapper mapper,IHealthSystem healthSystemRepository)
        {
            this.mapper = mapper;
            this.healthSystemRepository = healthSystemRepository;
        }

        [HttpGet]
        [Authorize(Policy = "AdministratorOnly")]
        public async Task< IActionResult> GetAllHealthSystems()
        {
            var healthSystems = await healthSystemRepository.GetAllHealthSystemsAsync();
            var hSystemsDto = mapper.Map<List<HealthSystemsDTO>>(healthSystems);
            return Ok(hSystemsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Policy = "AdministratorOrHealthSystemAdmin")]
        public async Task<IActionResult> GetHealthSystemById([FromRoute] Guid id)
        {
            var healthSystem = await healthSystemRepository.GetHealthSystemByIdAsync(id);
            if (healthSystem == null)
                return NotFound(id);

            var healthsystemdto = mapper.Map<GetHealthSystemDTO>(healthSystem);
            return Ok(healthsystemdto);
        }

        [HttpPost]
        [Authorize(Policy = "AdministratorOnly")]
        public async Task<IActionResult> CreateHealthSystem([FromBody] AddHealthSystemDTO healthSystemDTO)
        {
            if (healthSystemDTO == null)
            {
               return BadRequest("The request body cannot be empty.");
            }

            var HS= mapper.Map<HealthSystem>(healthSystemDTO);
            HS = await healthSystemRepository.AddHealthSystemAsync(HS);

            var getHDTO = mapper.Map<GetHealthSystemDTO>(HS);

            return CreatedAtAction(nameof(GetHealthSystemById), new { id = HS.Id }, getHDTO);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Policy = "AdministratorOrHealthSystemAdmin")]
        public async Task<IActionResult> UpdateHealthSystemById([FromRoute] Guid id,[FromBody] UpdateHealthSystemDTO updateHealthSystemDTO)
        {
            if (updateHealthSystemDTO == null)
            {
                return BadRequest("The update request body cannot be empty.");
            }
            //recieved dto to domain model
            var HS = mapper.Map<HealthSystem>(updateHealthSystemDTO);

            //existing domain model
            var ExistingHealthSystem = await healthSystemRepository.UpdateHealthSystemAsync(id, HS);

            if (ExistingHealthSystem == null)
                return NotFound(id);

            //after updating the existing domain model, converting back to dto to be send in response body
            var HSDTO=mapper.Map<UpdateHealthSystemDTO>(ExistingHealthSystem);

            return Ok(HSDTO);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Policy = "AdministratorOrHealthSystemAdmin")]
        public async Task<IActionResult> DeleteHealthSystemById([FromRoute] Guid id)
        {
            var healthSystem = await healthSystemRepository.GetHealthSystemByIdAsync(id);
            if (healthSystem == null)
            {
                return NotFound(id);
            }

            await healthSystemRepository.DeleteHealthSystemAsync(id);

            var deleted = mapper.Map<DeleteHealthSystemDTO>(healthSystem);
            return Ok(deleted);
        }


    }
}
