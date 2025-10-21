using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    public class StockController(IStockRepository stockRepo) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            var stocks = await stockRepo.GetAllAsync(query);

            return Ok(stocks.Select(x => x.ToStockDto()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var stock = await stockRepo.GetAsync(id);
            if (stock is null)
                return NotFound();
            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StockRequestDto stockRequestDto)
        {
            var stock = await stockRepo.CreateAsync(stockRequestDto);
            return CreatedAtAction(nameof(Get), new { id = stock.Id }, stock.ToStockDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] StockRequestDto stockRequestDto)
        {
            var stock = await stockRepo.UpdateAsync(id, stockRequestDto);
            if (stock is null)
                return NotFound();
            return Ok(stock.ToStockDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await stockRepo.DeleteAsync(id);
            if (stock is null)
                return NotFound();
            return Ok("Stock deleted successfully");
        }
    }

}