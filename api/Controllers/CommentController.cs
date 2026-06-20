using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Dtos.Comment;
using api.Dtos.CryptoAsset;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Extentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace api.Controllers;

[Route("api/comment")]
[ApiController]
[Authorize]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepo;
    private readonly ICryptoAssetRepository _cryptoAssetRepo;
    private readonly UserManager<AppUser> _userManager;

    public CommentController(ICommentRepository commentRepo,  ICryptoAssetRepository cryptoAssetRepo, UserManager<AppUser> userManager)
    {
        _commentRepo = commentRepo;
        _cryptoAssetRepo = cryptoAssetRepo;
        _userManager = userManager;
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

        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);

        var commentModel = createCommentDto.ToCommentFromCreateDto(cryptoAssetId, appUser.Id);

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

        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);

        var existingComment = await _commentRepo.GetByIdAsync(id);
        if (existingComment is null)
        {
            return NotFound();
        }

        if (existingComment.AppUserId != appUser.Id)
        {
            return Forbid();
        }

        var commentModel = updateCommentDto.ToCommentFromUpdateDto();

        var comment = await _commentRepo.UpdateAsync(id, commentModel);
        return Ok(comment.ToCommentDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);

        var existingComment = await _commentRepo.GetByIdAsync(id);
        if (existingComment is null)
        {
            return NotFound();
        }

        if (existingComment.AppUserId != appUser.Id)
        {
            return Forbid();
        }

        await _commentRepo.DeleteAsync(id);
        return NoContent();
    }
}