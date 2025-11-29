using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class QueryObject
    {
        public string? Symbol { get; set; }
        public string? CompanyName { get; set; }
        public StockSortBy? SortBy { get; set; }
        public bool IsDecsending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public enum StockSortBy
    {
        Id,
        Symbol,
        CompanyName,
        Purchase,
        CreatedAt
    }
}