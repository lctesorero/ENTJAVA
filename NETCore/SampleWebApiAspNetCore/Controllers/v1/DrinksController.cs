using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Services;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Repositories;
using System.Text.Json;

namespace SampleWebApiAspNetCore.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DrinksController : ControllerBase
    {
        private readonly IDrinkRepository _drinkRepository;
        private readonly IMapper _mapper;
        private readonly ILinkService<DrinksController> _linkService;

        public DrinksController(
            IDrinkRepository drinkRepository,
            IMapper mapper,
            ILinkService<DrinksController> linkService)
        {
            _drinkRepository = drinkRepository;
            _mapper = mapper;
            _linkService = linkService;
        }

        [HttpGet(Name = nameof(GetAllDrinks))]
        public ActionResult GetAllDrinks(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            List<DrinkEntity> drinkItems = _drinkRepository.GetAll(queryParameters).ToList();

            var allItemCount = _drinkRepository.Count();

            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = _linkService.CreateLinksForCollection(queryParameters, allItemCount, version);
            var toReturn = drinkItems.Select(x => _linkService.ExpandSingleDrinkItem(x, x.Id, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleDrink))]
        public ActionResult GetSingleDrink(ApiVersion version, int id)
        {
            DrinkEntity drinkItem = _drinkRepository.GetSingle(id);

            if (drinkItem == null)
            {
                return NotFound();
            }

            DrinkDto item = _mapper.Map<DrinkDto>(drinkItem);

            return Ok(_linkService.ExpandSingleDrinkItem(item, item.Id, version));
        }

        [HttpPost(Name = nameof(AddDrink))]
        public ActionResult<DrinkDto> AddDrink(ApiVersion version, [FromBody] DrinkCreateDto drinkCreateDto)
        {
            if (drinkCreateDto == null)
            {
                return BadRequest();
            }

            DrinkEntity toAdd = _mapper.Map<DrinkEntity>(drinkCreateDto);

            _drinkRepository.Add(toAdd);

            if (!_drinkRepository.Save())
            {
                throw new Exception("Creating a drinkitem failed on save.");
            }

            DrinkEntity newDrinkItem = _drinkRepository.GetSingle(toAdd.Id);
            DrinkDto drinkDto = _mapper.Map<DrinkDto>(newDrinkItem);

            return CreatedAtRoute(nameof(GetSingleDrink),
                new { version = version.ToString(), id = newDrinkItem.Id },
                _linkService.ExpandSingleDrinkItem(drinkDto, drinkDto.Id, version));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateDrink))]
        public ActionResult<DrinkDto> PartiallyUpdateDrink(ApiVersion version, int id, [FromBody] JsonPatchDocument<DrinkUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            DrinkEntity existingEntity = _drinkRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            DrinkUpdateDto drinkUpdateDto = _mapper.Map<DrinkUpdateDto>(existingEntity);
            patchDoc.ApplyTo(drinkUpdateDto);

            TryValidateModel(drinkUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(drinkUpdateDto, existingEntity);
            DrinkEntity updated = _drinkRepository.Update(id, existingEntity);

            if (!_drinkRepository.Save())
            {
                throw new Exception("Updating a drinkitem failed on save.");
            }

            DrinkDto drinkDto = _mapper.Map<DrinkDto>(updated);

            return Ok(_linkService.ExpandSingleDrinkItem(drinkDto, drinkDto.Id, version));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveDrink))]
        public ActionResult RemoveDrink(int id)
        {
            DrinkEntity drinkItem = _drinkRepository.GetSingle(id);

            if (drinkItem == null)
            {
                return NotFound();
            }

            _drinkRepository.Delete(id);

            if (!_drinkRepository.Save())
            {
                throw new Exception("Deleting a drinkitem failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateDrink))]
        public ActionResult<DrinkDto> UpdateDrink(ApiVersion version, int id, [FromBody] DrinkUpdateDto drinkUpdateDto)
        {
            if (drinkUpdateDto == null)
            {
                return BadRequest();
            }

            var existingDrinkItem = _drinkRepository.GetSingle(id);

            if (existingDrinkItem == null)
            {
                return NotFound();
            }

            _mapper.Map(drinkUpdateDto, existingDrinkItem);

            _drinkRepository.Update(id, existingDrinkItem);

            if (!_drinkRepository.Save())
            {
                throw new Exception("Updating a drinkitem failed on save.");
            }

            DrinkDto drinkDto = _mapper.Map<DrinkDto>(existingDrinkItem);

            return Ok(_linkService.ExpandSingleDrinkItem(drinkDto, drinkDto.Id, version));
        }

        [HttpGet("GetRandomBeverages", Name = nameof(GetRandomBeverages))]
        public ActionResult GetRandomBeverages()
        {
            ICollection<DrinkEntity> drinkItems = _drinkRepository.GetRandomBeverages();

            IEnumerable<DrinkDto> dtos = drinkItems.Select(x => _mapper.Map<DrinkDto>(x));

            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(Url.Link(nameof(GetRandomBeverages), null), "self", "GET"));

            return Ok(new
            {
                value = dtos,
                links = links
            });
        }
    }
}
