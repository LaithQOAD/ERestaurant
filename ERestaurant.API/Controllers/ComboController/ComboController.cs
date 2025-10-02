using ERestaurant.Application.Combos.ComboServices;
using ERestaurant.Application.Combos.DTOs;
using ERestaurant.Application.DTOs.ComboDTOs;
using Microsoft.AspNetCore.Mvc;

namespace ERestaurant.API.Controllers
{
    [ApiController]
    [Route("API/Combo")]
    public sealed class ComboController : ControllerBase
    {
        private readonly IComboService _comboService;

        public ComboController(IComboService comboService)
        {
            _comboService = comboService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComboDTO>>> FindAllAsync(
            [FromQuery] FindAllComboParameterDTO findAllParameter)
        {
            var listOfCombos = await _comboService.FindAllAsync(findAllParameter);

            return Ok(listOfCombos);
        }

        [HttpGet("WithAddition")]
        public async Task<ActionResult<ComboWithAdditionalMaterialDTO>> FindAllWithAdditionsAsync(
            [FromQuery] FindAllComboParameterDTO findAllParameter)
        {
            var listOfCombosWithAdditions =
                await _comboService.FindAllWithAdditionsAsync(findAllParameter);

            return Ok(listOfCombosWithAdditions);
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ComboDTO>> FindByIdAsync(Guid id)
        {
            var comboResult = await _comboService.FindByIdAsync(id);

            return Ok(comboResult);
        }

        [HttpPost]
        public async Task<ActionResult<ComboDTO>> CreateAsync(CreateComboDTO newComboRequest)
        {
            await _comboService.CreateAsync(newComboRequest);
            return Created();
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ComboDTO>> UpdateAsync(Guid id, UpdateComboDTO updatedComboRequest)
        {
            var updatedCombo = await _comboService.UpdateAsync(updatedComboRequest);
            return Ok(updatedCombo);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _comboService.DeleteAsync(id);
            return NoContent();
        }
    }
}
