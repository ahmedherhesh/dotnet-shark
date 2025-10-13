using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository(ApplicationDBContext context) : ICommentRepository
    {
        public async Task<List<Comment>> GetAllAsync()
        {
            return await context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetAsync(int id)
        {
            return await context.Comments.FindAsync(id);
        }

        public async Task<Comment> CreateAsync(CommentRequestDto commentRequestDto)
        {
            var comment =  commentRequestDto.ToCommentRequestDto();
            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> UpdateAsync(int id, CommentRequestDto commentRequestDto)
        {
            var comment = await GetAsync(id);
            if (comment is null)
                return null;
            comment.StockId = commentRequestDto.StockId;
            comment.Title = commentRequestDto.Title;
            comment.Content = commentRequestDto.Content;
            await context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            var comment = await GetAsync(id);
            if (comment is null)
                return null;
            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
            return true;
        }
    }
}