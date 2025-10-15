using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController(IStockRepository stockRepo, ICommentRepository commentRepo) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await commentRepo.GetAllAsync();
            return Ok(comments.Select(x => x.ToCommentDto()).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var comment = await commentRepo.GetAsync(id);
            if (comment is null)
                return NotFound();
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CommentRequestDto commentRequestDto)
        {
            var stock = await stockRepo.GetAsync(stockId);
            if (stock is null)
                return BadRequest("Stock not found");

            var comment = await commentRepo.CreateAsync(stock, commentRequestDto);
            return Ok(comment.ToCommentDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromQuery] int? stockId, [FromBody] CommentRequestDto commentRequestDto)
        {
            if (stockId is not null)
            {
                var stock = await stockRepo.GetAsync(stockId.Value);
                if (stock is null)
                    return BadRequest("Stock not found");
            }
            
            var comment = await commentRepo.UpdateAsync(id, stockId, commentRequestDto);
            if (comment is null)
                return NotFound();
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var comment = await commentRepo.DeleteAsync(id);
            if (comment is null)
                return NotFound();
            return Ok("Comment deleted successfully");
        }
    }
}