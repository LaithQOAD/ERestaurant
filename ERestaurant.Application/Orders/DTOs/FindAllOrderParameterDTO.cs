using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERestaurant.Application.Orders.DTOs
{
    public class FindAllOrderParameterDTO
    {
        public string? searchQuery { get; set; }
        public DateTimeOffset dateFrom { get; set; }
        public DateTimeOffset dateTo { get; set; }
        public bool? isActive { get; set; }
        public string? orderBy { get; set; } = "CreatedDate";
        public string? orderByDirection { get; set; } = "DESC";
        public int? pageNumber { get; set; } = 1;
        public int? pageSize { get; set; } = 10;
    }
}
