using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Data;
using api.Dtos.Comment;
using api.Dtos.CryptoAsset;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace api.Controllers;

[Route("api/[controller]")]
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
    public async Task<ActionResult> GetAll(
        [FromQuery] int? cryptoAssetId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > 100) pageSize = 100; // максимум 100 за раз

        var comments = await _commentRepo.GetAllAsync(page, pageSize, cryptoAssetId);
        var totalCount = await _commentRepo.GetTotalCountAsync(cryptoAssetId);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var commentsDto = comments.Select(c => c.ToCommentDto());

        return Ok(new
        {
            data = commentsDto,
            page,
            pageSize,
            totalCount,
            totalPages
        });
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