
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Comment;

public class UpdateCommentDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Content must be at least 1 character long")]
    [MaxLength(5000, ErrorMessage = "Content cannot exceed 5000 characters")]
    public string Content { get; set; } =  string.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "Title must be at least 1 character long")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } =  string.Empty;
}