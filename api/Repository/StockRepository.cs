using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository(ApplicationDBContext context) : IStockRepository
    {
        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = context.Stocks.AsQueryable();

            // string[] allowedSortBy = {"Id", "Symbol", "CompanyName", "CreatedAt" };

            if (!string.IsNullOrWhiteSpace(query.Symbol))
                stocks = stocks.Where(x => x.Symbol.Contains(query.Symbol, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
                stocks = stocks.Where(x => x.CompanyName.Contains(query.CompanyName, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Id", StringComparison.OrdinalIgnoreCase))
                    stocks = query.IsDecsending ? stocks.OrderByDescending(x => x.Id) : stocks.OrderBy(x => x.Id);
                // foreach (string sortBy in allowedSortBy)
                // {
                //     if (query.SortBy.Equals(sortBy, StringComparison.OrdinalIgnoreCase))
                //         stocks = query.IsDecsending ? stocks.OrderByDescending(x => x.GetType().GetProperty(sortBy).GetValue(x)) : stocks.OrderBy(x => x.GetType().GetProperty(sortBy).GetValue(x));
                // }
            }
            
            int skipNumber = (query.PageNumber - 1) * query.PageSize;
            return await stocks.Skip(skipNumber).Take(query.PageSize).Include(x => x.Comments).ToListAsync();
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