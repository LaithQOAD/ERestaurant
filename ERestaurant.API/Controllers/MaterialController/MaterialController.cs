using ERestaurant.Application.DTOs.MaterialDTOs;
using ERestaurant.Application.Services.MaterialServices;
using ERestaurant.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ERestaurant.API.Controllers.MaterialController
{
    [ApiController]
    [Route("API/Material")]
    public sealed class MaterialController : ControllerBase
    {
        private readonly IMaterialServices _materialServices;

        public MaterialController(IMaterialServices materialServices)
        {
            _materialServices = materialServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<MaterialDTO>>> FindAllAsync(
            string? searchNameQuery,
            bool? isActive,
            MaterialUnit? unit,
            string? orderBy = "CreatedDate",
            string? orderByDirection = "DESC",
            int? pageNumber = 1,
            int? pageSize = 10)
        {
            var items = await _materialServices.FindAllAsync(
                searchNameQuery, isActive, unit,
                orderBy, orderByDirection,
                pageNumber, pageSize);

            return Ok(items);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MaterialDTO>> FindByIdAsync(Guid id)
        {
            var materialResult = await _materialServices.FindByIdAsync(id);
            if (materialResult is null)
                return NotFound("Details: material not found");

            return Ok(materialResult);
        }

        [HttpPost]
        public async Task<ActionResult<MaterialDTO>> CreateAsync(CreateMaterialDTO newMaterialRequest)
        {
            var createdMaterial = await _materialServices.CreateAsync(newMaterialRequest);
            return Created();
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<MaterialDTO>> UpdateAsync(Guid id, UpdateMaterialDTO updatedMaterialRequest)
        {
            if (id != updatedMaterialRequest.Id)
                return BadRequest("Details: route id != body id");

            var updatedMaterial = await _materialServices.UpdateAsync(updatedMaterialRequest);
            return Ok(updatedMaterial);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _materialServices.DeleteAsync(id);
            return NoContent();
        }
    }
}
