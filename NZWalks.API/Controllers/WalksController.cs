using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Net;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        //Create walk
        //Post: /api/walks/
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDTO addWalkRequestDTO)
        {
            //Map Dto to Domain
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDTO);

            await walkRepository.CreateWalkAsync(walkDomainModel);

            //Map domain to model;

            return Ok(mapper.Map<WalkDTO>(walkDomainModel));

        }
        //Get All Walks
        //Get: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetWalk([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            

            var walksDomainModel = await walkRepository.GetAllWalksAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
           
            return Ok(mapper.Map<List<WalkDTO>>(walksDomainModel));

        }
        //Get All Walks
        //Get: /api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalkByID([FromRoute] Guid id)
        {
            var walksDomainModel = await walkRepository.GetWalkByIDAsync(id);

            if (walksDomainModel == null)
                return NotFound();


            return Ok(mapper.Map<WalkDTO>(walksDomainModel));
        }

        //Update Walk
        //PUT: /api/walk/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, UpdateWalkDTO updateWalkDTO)
        {
            var walksDomainModel = mapper.Map<Walk>(updateWalkDTO);

            walksDomainModel = await walkRepository.UpdateAsync(id, walksDomainModel);

            if (walksDomainModel == null)
                return NotFound();

            return Ok(mapper.Map<WalkDTO>(walksDomainModel));

        }
        //Delete Walk
        //Delete: /api/walks/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            var walksDomainModel = await walkRepository.DeleteAsync(id);
            if (walksDomainModel == null)
                return NotFound();

            return Ok(mapper.Map<WalkDTO>(walksDomainModel));
        }

    }
}
