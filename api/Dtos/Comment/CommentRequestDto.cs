using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class CommentRequestDto
    {
        [Required, MinLength(5, ErrorMessage = "The title field must be at least 5 characters"), MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required, MinLength(5, ErrorMessage = "The content field must be at least 5 characters"), MaxLength(255)]
        public string Content { get; set; } = string.Empty;
    }
}