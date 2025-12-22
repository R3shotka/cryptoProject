
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment;

public class UpdateCommentDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Content must be at least 1 character long")]
    public string Content { get; set; } =  string.Empty;
    [Required]
    [MinLength(1, ErrorMessage = "Title must be at least 1 character long")]
    public string Title { get; set; } =  string.Empty;
  
}