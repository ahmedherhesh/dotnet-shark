using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetAsync(int id);
        Task<Comment> CreateAsync(CommentRequestDto commentRequestDto);
        Task<Comment?> UpdateAsync(int id, CommentRequestDto commentRequestDto);
        Task<bool?> DeleteAsync(int id);
    }
}