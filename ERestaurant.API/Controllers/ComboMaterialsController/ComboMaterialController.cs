using ERestaurant.Application.ComboMaterials.ComboMaterialServices;
using ERestaurant.Application.DTOs.ComboDTOs;
using Microsoft.AspNetCore.Mvc;

namespace ERestaurant.API.Controllers.ComboMaterialController
{
    [ApiController]
    [Route("API/Combo/{comboId:guid}/Material")]
    public sealed class ComboMaterialController : ControllerBase
    {
        private readonly IComboMaterialService _comboMaterialService;

        public ComboMaterialController(IComboMaterialService comboMaterialService)
        {
            _comboMaterialService = comboMaterialService;
        }

        [HttpPost("{materialId:guid}")]
        public async Task<ActionResult<ComboDTO>> AddMaterialAsync(
            Guid comboId,
            Guid materialId,
            [FromQuery] int quantity = 1)
        {
            var updatedCombo = await _comboMaterialService.AddMaterialAsync(comboId, materialId, quantity);
            return Ok(updatedCombo);
        }


        [HttpPut("{materialId:guid}")]
        public async Task<ActionResult<ComboDTO>> UpdateQuantityAsync(
            Guid comboId,
            Guid materialId,
            [FromQuery] int quantity)
        {
            var updatedCombo = await _comboMaterialService.UpdateQuantityAsync(comboId, materialId, quantity);
            return Ok(updatedCombo);
        }

        [HttpDelete("{materialId:guid}")]
        public async Task<ActionResult<ComboDTO>> RemoveMaterialAsync(
            Guid comboId,
            Guid materialId)
        {
            var updatedCombo = await _comboMaterialService.RemoveMaterialAsync(comboId, materialId);
            return Ok(updatedCombo);
        }
    }
}
