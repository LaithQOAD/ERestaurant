using Microsoft.AspNetCore.Mvc;
using ERestaurant.Application.DTOs.ComboDTOs;
using ERestaurant.Application.Services.ComboServices;

namespace ERestaurant.API.Controllers
{
    [ApiController]
    [Route("API/Combo")]
    public sealed class ComboController : ControllerBase
    {
        private readonly IComboServices _comboServices;

        public ComboController(IComboServices comboServices)
        {
            _comboServices = comboServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComboDTO>>> FindAllAsync(
            string? searchComboNameQuery,
            string? filterComboNameQuery,
            bool? isActive,
            decimal? minPrice,
            decimal? maxPrice,
            string? orderBy = "CreatedDate",
            string? orderByDirection = "DESC",
            int? pageNumber = 1,
            int? pageSize = 10)
        {
            var listOfCombos = await _comboServices.FindAllAsync(
                searchComboNameQuery, filterComboNameQuery, isActive,
                minPrice, maxPrice, orderBy, orderByDirection,
                pageNumber, pageSize);

            return Ok(listOfCombos);
        }

        [HttpGet("WithAddition")]
        public async Task<ActionResult<ComboWithAdditionalMaterialDTO>> FindAllWithAdditionsAsync(
            string? searchComboNameQuery,
            string? filterComboNameQuery,
            bool? isActive,
            decimal? minPrice,
            decimal? maxPrice,
            string? orderBy = "CreatedDate",
            string? orderByDirection = "DESC",
            int? pageNumber = 1,
            int? pageSize = 10)
        {
            var listOfCombosWithAdditions = await _comboServices.FindAllWithAdditionsAsync(
                searchComboNameQuery, filterComboNameQuery, isActive,
                minPrice, maxPrice, orderBy, orderByDirection, pageNumber, pageSize);

            return Ok(listOfCombosWithAdditions);
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ComboDTO>> FindByIdAsync(Guid id)
        {
            var comboResult = await _comboServices.FindByIdAsync(id);

            return Ok(comboResult);
        }

        [HttpPost]
        public async Task<ActionResult<ComboDTO>> CreateAsync(CreateComboDTO newComboRequest)
        {
            await _comboServices.CreateAsync(newComboRequest);
            return Created();
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ComboDTO>> UpdateAsync(Guid id, UpdateComboDTO updatedComboRequest)
        {
            var updatedCombo = await _comboServices.UpdateAsync(updatedComboRequest);
            return Ok(updatedCombo);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _comboServices.DeleteAsync(id);
            return NoContent();
        }
    }
}
