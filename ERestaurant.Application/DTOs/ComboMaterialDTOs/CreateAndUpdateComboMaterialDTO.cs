using System.ComponentModel.DataAnnotations;

namespace ERestaurant.Application.DTOs.ComboMaterialDTOs
{
	public class CreateAndUpdateComboMaterialDTO
	{
		[Required]
		public Guid MaterialId { get; set; }

		[Range(1, int.MaxValue)]
		public int Quantity { get; set; } = 1;

	}
}
