using ERestaurant.Application.DTOs.AdditionalMaterialDTOs;
using ERestaurant.Application.Services.AdditionalMaterialServices;
using ERestaurant.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ERestaurant.API.Controllers.AdditionalMaterialController
{
    [ApiController]
    [Route("API/AdditionalMaterial")]
    public sealed class AdditionalMaterialController : ControllerBase
    {
        private readonly IAdditionalMaterialServices _additionalMaterialServices;

        public AdditionalMaterialController(IAdditionalMaterialServices additionalMaterialServices)
        {
            _additionalMaterialServices = additionalMaterialServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<AdditionalMaterialDTO>>> FindAllAsync(
            string? searchQuery,
            MaterialUnit? unit,
            bool? isActive,
            decimal? priceFrom,
            decimal? priceTo,
            string? orderBy = "Name",
            string? orderByDirection = "ASC",
            int? pageNumber = 1,
            int? pageSize = 10)
        {
            var result = await _additionalMaterialServices.FindAllAsync(
                searchQuery, unit, isActive, priceFrom, priceTo,
                orderBy, orderByDirection, pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AdditionalMaterialDTO>> FindByIdAsync(Guid id)
        {
            var additionalMaterialEntity = await _additionalMaterialServices.FindByIdAsync(id);
            if (additionalMaterialEntity is null) return NotFound();

            return Ok(additionalMaterialEntity);
        }


        [HttpPost]
        public async Task<ActionResult<AdditionalMaterialDTO>> CreateAsync(CreateAdditionalMaterialDTO newAdditionalMaterialRequest)
        {
            var createdAdditionalMaterial = await _additionalMaterialServices.AddAsync(newAdditionalMaterialRequest);
            return CreatedAtAction(nameof(FindByIdAsync), new { id = createdAdditionalMaterial.Id }, createdAdditionalMaterial);
        }


        [HttpPut("{id:guid}")]
        public async Task<ActionResult<AdditionalMaterialDTO>> UpdateAsync(Guid id, UpdateAdditionalMaterialDTO updateAdditionalMaterialRequest)
        {
            if (id != updateAdditionalMaterialRequest.Id)
                return BadRequest("Route id and body id must match.");

            var updatedAdditionalMaterial = await _additionalMaterialServices.UpdateAsync(updateAdditionalMaterialRequest);
            return Ok(updatedAdditionalMaterial);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _additionalMaterialServices.DeleteAsync(id);
            return NoContent();
        }
    }
}
