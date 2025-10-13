using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                StockId = comment.StockId,
                Title = comment.Title,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            };
        }

        public static Comment ToCommentRequestDto(this CommentRequestDto commentRequestDto)
        {
            return new Comment
            {
                StockId = commentRequestDto.StockId,
                Title = commentRequestDto.Title,
                Content = commentRequestDto.Content,
            };
        }
    }
}