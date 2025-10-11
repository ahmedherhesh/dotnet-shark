using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    public class StockController(ApplicationDBContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? symbol)
        {
            var stocks = context.Stocks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(symbol))
            {
                stocks = stocks.Where(x => x.Symbol.ToLower().Contains(symbol.ToLower()));
            }

            await stocks.OrderByDescending(x => x.Id).ToListAsync();

            return Ok(stocks.Select(x => x.ToStockDto()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var stock = await context.Stocks.FindAsync(id);
            if (stock is null)
                return NotFound();
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StockRequestDto stockRequestDto)
        {
            var stock = stockRequestDto.ToStockRequestDto();
            await context.Stocks.AddAsync(stock);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] StockRequestDto stockRequestDto)
        {
            var stock = await context.Stocks.FindAsync(id);
            if (stock is null)
                return NotFound();
            stock.Symbol = stockRequestDto.Symbol;
            stock.CompanyName = stockRequestDto.CompanyName;
            stock.Purchase = stockRequestDto.Purchase;
            stock.LastDiv = stockRequestDto.LastDiv;
            stock.Industry = stockRequestDto.Industry;
            stock.MarketCap = stockRequestDto.MarketCap;
            await context.SaveChangesAsync();
            return Ok(stock.ToStockDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await context.Stocks.FindAsync(id);
            if (stock is null)
                return NotFound();
            context.Stocks.Remove(stock);
            await context.SaveChangesAsync();
            return Ok("Stock deleted successfully");
        }
    }

}