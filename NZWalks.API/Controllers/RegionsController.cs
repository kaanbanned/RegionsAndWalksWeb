using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDBContext dBContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDBContext dBContext, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            this.dBContext = dBContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]

        //[Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAll()
        {
            //Get Data from db -domain models
            var regions = await regionRepository.GetAllAsync();

            var regionDtos = mapper.Map<List<RegionDTO>>(regions);
            logger.LogInformation($"Finished GetAllRegions with data: {JsonSerializer.Serialize(regions)}");
            return Ok(regionDtos);
        }
        [HttpGet]
        [MapToApiVersion("2.0")]
        //[Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAllV2()
        {
            //Get Data from db -domain models
            var regions = await regionRepository.GetAllAsync();

            var regionDtos = mapper.Map<List<RegionDTOV2>>(regions);
            logger.LogInformation($"Finished GetAllV2 with data: {JsonSerializer.Serialize(regions)}");
            return Ok(regionDtos);
        }

        // Get: /regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetByID([FromRoute] Guid id)
        {

            // var regions = dBContext.Regions.Find(id);
            var regions = await regionRepository.GetByIDAsync(id);
            if (regions == null)
                return NotFound();

            var regionDtos = mapper.Map<RegionDTO>(regions);
            return Ok(regionDtos);

        }

        //POST Create new Region
        //POST: ../api/regions/
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] CreateRegionDTO newRegion)
        {
            var regionDomainModel = mapper.Map<Region>(newRegion);

            await regionRepository.CreateRegionAsync(regionDomainModel);

            var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);

            return CreatedAtAction(nameof(GetByID), new { id = regionDTO.Id }, regionDTO);
        }


        //Update the region
        //PUT: ../api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDTO updateRegionDTO)
        {

            var region = mapper.Map<Region>(updateRegionDTO);
            var regionDomainModel = await regionRepository.UpdateAsync(id, region);

            if (regionDomainModel == null) return NotFound();
            var regionDto = mapper.Map<RegionDTO>(region);

            return Ok(regionDto);

        }


        //Delete the region
        //Delete: ../api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null) return NotFound();

            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionDto);
        }
    }
}
