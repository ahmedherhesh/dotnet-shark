using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class StockRequestDto
    {
        [Required, MaxLength(10)]
        public string Symbol { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string CompanyName { get; set; } = string.Empty;
        [Required, Range(1, double.MaxValue)]
        public decimal Purchase { get; set; }
        [Required, Range(0.001, 100)]
        public decimal LastDiv { get; set; }
        [Required, MaxLength(10, ErrorMessage = "Industry cannot be over 10 characters")]
        public string Industry { get; set; } = string.Empty;
        [Range(1, long.MaxValue)]
        public long MarketCap { get; set; }
    }
}