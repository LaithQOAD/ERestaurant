using ERestaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERestaurant.Application.AdditionalMaterials.DTOs
{
    public class FindAllAdditionalMaterialParameterDTO
    {
        public string? searchQuery { get; set; }
        public MaterialUnit? unit { get; set; }
        public bool? isActive { get; set; }
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }
        public string? orderBy { get; set; } = "CreatedDate";
        public string? orderByDirection { get; set; } = "DESC";
        public int? pageNumber { get; set; } = 1;
        public int? pageSize { get; set; } = 10;
    }
}
