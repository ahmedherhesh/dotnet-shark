using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository(ApplicationDBContext context) : IStockRepository
    {
        public async Task<List<Stock>> GetAllAsync(string? symbol)
        {
            var stocks = context.Stocks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(symbol))
            {
                stocks = stocks.Where(x => x.Symbol.Contains(symbol, StringComparison.OrdinalIgnoreCase));
            }

            return await stocks.OrderByDescending(x => x.Id).Include(x => x.Comments).ToListAsync();
        }

        public async Task<Stock?> GetAsync(int id)
        {
            var stock = await context.Stocks.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
            return stock;
        }
        
        public async Task<Stock> CreateAsync(StockRequestDto stockRequestDto)
        {
            var stock = stockRequestDto.ToStockRequestDto();
            await context.Stocks.AddAsync(stock);
            await context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> UpdateAsync(int id, StockRequestDto stockRequestDto)
        {
            var stock = await GetAsync(id);
            if (stock is null)
                return null;
            stock.Symbol = stockRequestDto.Symbol;
            stock.CompanyName = stockRequestDto.CompanyName;
            stock.Purchase = stockRequestDto.Purchase;
            stock.LastDiv = stockRequestDto.LastDiv;
            stock.Industry = stockRequestDto.Industry;
            stock.MarketCap = stockRequestDto.MarketCap;
            await context.SaveChangesAsync();
            return stock;
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            var stock = await context.Stocks.FindAsync(id);
            if (stock is null)
                return null;
            context.Stocks.Remove(stock);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool?> StockExists(int id)
        {
            return await context.Stocks.AnyAsync(x => x.Id == id);
        }
    }
}