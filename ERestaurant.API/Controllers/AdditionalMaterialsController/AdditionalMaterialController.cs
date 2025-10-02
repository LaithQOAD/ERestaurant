using ERestaurant.Application.AdditionalMaterials.AdditionalMaterialDTOs;
using ERestaurant.Application.AdditionalMaterials.AdditionalMaterialServices;
using ERestaurant.Application.AdditionalMaterials.DTOs;
using ERestaurant.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ERestaurant.API.Controllers.AdditionalMaterialController
{
    [ApiController]
    [Route("API/AdditionalMaterial")]
    public sealed class AdditionalMaterialController : ControllerBase
    {
        private readonly IAdditionalMaterialService _additionalMaterialService;

        public AdditionalMaterialController(IAdditionalMaterialService additionalMaterialService)
        {
            _additionalMaterialService = additionalMaterialService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AdditionalMaterialDTO>>> FindAllAsync(
            [FromQuery] FindAllAdditionalMaterialParameterDTO findAllAdditionalMaterialParameterDTO)
        {
            var result = await _additionalMaterialService.FindAllAsync(findAllAdditionalMaterialParameterDTO);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AdditionalMaterialDTO>> FindByIdAsync(Guid id)
        {
            var additionalMaterialEntity = await _additionalMaterialService.FindByIdAsync(id);
            if (additionalMaterialEntity is null) return NotFound();

            return Ok(additionalMaterialEntity);
        }


        [HttpPost]
        public async Task<ActionResult<AdditionalMaterialDTO>> CreateAsync(CreateAdditionalMaterialDTO newAdditionalMaterialRequest)
        {
            var createdAdditionalMaterial = await _additionalMaterialService.AddAsync(newAdditionalMaterialRequest);
            return Created();
        }


        [HttpPut("{id:guid}")]
        public async Task<ActionResult<AdditionalMaterialDTO>> UpdateAsync(Guid id, UpdateAdditionalMaterialDTO updateAdditionalMaterialRequest)
        {
            if (id != updateAdditionalMaterialRequest.Id)
                return BadRequest("Route id and body id must match.");

            var updatedAdditionalMaterial = await _additionalMaterialService.UpdateAsync(updateAdditionalMaterialRequest);
            return Ok(updatedAdditionalMaterial);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _additionalMaterialService.DeleteAsync(id);
            return NoContent();
        }
    }
}
