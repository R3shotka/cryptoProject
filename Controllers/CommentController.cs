using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Dtos.Comment;
using api.Dtos.CryptoAsset;
using api.Interfaces;
using api.Mappers;
using api.Models;


namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepo;
    private readonly ICryptoAssetRepository _cryptoAssetRepo;

    public CommentController(ICommentRepository commentRepo,  ICryptoAssetRepository cryptoAssetRepo)
    {
        _commentRepo = commentRepo;
        _cryptoAssetRepo = cryptoAssetRepo;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var comments = await _commentRepo.GetAllAsync();
        var commentsDto = comments.Select(c => c.ToCommentDto());
        return Ok(commentsDto);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(int id)
    {
        var comment = await _commentRepo.GetByIdAsync(id);

        if (comment is null)
        {
            return NotFound();
        }
        
        return Ok(comment.ToCommentDto());
    }

    [HttpPost("{cryptoAssetId:int}")]
    public async Task<ActionResult> Create([FromRoute] int cryptoAssetId, [FromBody] CreateCommentDto createCommentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (!await _cryptoAssetRepo.CryptoAssetExists(cryptoAssetId))
        {
            return BadRequest("the crypto is not exist");
        }
        
        var commentModel = createCommentDto.ToCommentFromCreateDto(cryptoAssetId);
        
        await _commentRepo.CreateAsync(commentModel);
        return CreatedAtAction(nameof(GetById), new {id =  commentModel.Id}, commentModel.ToCommentDto());
    }

    [HttpPut("{id:int}")]
    
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto updateCommentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var commentModel = updateCommentDto.ToCommentFromUpdateDto();
        
        var comment = await _commentRepo.UpdateAsync(id, commentModel);
        if (comment is null)
        {
            return NotFound();
        }
        return Ok(comment.ToCommentDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var comment = await _commentRepo.DeleteAsync(id);
        if (comment is null)
        {
            return NotFound();
        }
        return NoContent();
    }
}