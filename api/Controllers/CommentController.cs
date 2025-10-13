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
    public class CommentController(ICommentRepository commentRepo) : ControllerBase
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentRequestDto commentRequestDto)
        {
            var comment = await commentRepo.CreateAsync(commentRequestDto);
            return Ok(comment.ToCommentDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CommentRequestDto commentRequestDto)
        {
            var comment = await commentRepo.UpdateAsync(id, commentRequestDto);
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