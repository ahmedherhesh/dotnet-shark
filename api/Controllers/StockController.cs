using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    public class StockController(ApplicationDBContext context) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll([FromQuery] string? symbol)
        {
            var stocks = context.Stocks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(symbol))
            {
                stocks = stocks.Where(x => x.Symbol.ToLower().Contains(symbol.ToLower()));
            }

            stocks
            .OrderByDescending(x => x.Id)
                .Select(x => x.ToStockDto())
                .ToList();

            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var stock = context.Stocks.Find(id)?.ToStockDto();
            if (stock is null)
                return NotFound();
            return Ok(stock);
        }

        [HttpPost]
        public IActionResult Create([FromBody] StockRequestDto stockRequestDto)
        {
            var stock = stockRequestDto.ToStockRequestDto();
            context.Stocks.Add(stock);
            context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] StockRequestDto stockRequestDto)
        {
            var stock = context.Stocks.Find(id);
            if (stock is null)
                return NotFound();
            stock.Symbol = stockRequestDto.Symbol;
            stock.CompanyName = stockRequestDto.CompanyName;
            stock.Purchase = stockRequestDto.Purchase;
            stock.LastDiv = stockRequestDto.LastDiv;
            stock.Industry = stockRequestDto.Industry;
            stock.MarketCap = stockRequestDto.MarketCap;
            context.SaveChanges();
            return Ok(stock.ToStockDto());
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var stock = context.Stocks.Find(id);
            if (stock is null)
                return NotFound();
            context.Stocks.Remove(stock);
            context.SaveChanges();
            return Ok("Stock deleted successfully");
        }
    }

}