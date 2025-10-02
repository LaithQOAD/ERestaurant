using ERestaurant.Application.Materials.DTOs;
using ERestaurant.Application.Materials.MaterialServices;
using ERestaurant.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ERestaurant.API.Controllers.MaterialController
{
    [ApiController]
    [Route("API/Material")]
    public sealed class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MaterialDTO>>> FindAllAsync(
            FindAllMaterialParameterDTO findAllMaterialParameterDTO)
        {
            var items = await _materialService.FindAllAsync(findAllMaterialParameterDTO);

            return Ok(items);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MaterialDTO>> FindByIdAsync(Guid id)
        {
            var materialResult = await _materialService.FindByIdAsync(id);
            if (materialResult is null)
                return NotFound("Details: material not found");

            return Ok(materialResult);
        }

        [HttpPost]
        public async Task<ActionResult<MaterialDTO>> CreateAsync(CreateMaterialDTO newMaterialRequest)
        {
            var createdMaterial = await _materialService.CreateAsync(newMaterialRequest);
            return Created();
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<MaterialDTO>> UpdateAsync(Guid id, UpdateMaterialDTO updatedMaterialRequest)
        {
            if (id != updatedMaterialRequest.Id)
                return BadRequest("Details: route id != body id");

            var updatedMaterial = await _materialService.UpdateAsync(updatedMaterialRequest);
            return Ok(updatedMaterial);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _materialService.DeleteAsync(id);
            return NoContent();
        }
    }
}
