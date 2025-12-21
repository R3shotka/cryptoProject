using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;


namespace api.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDBContext _context;
    
    public CommentRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<List<Comment>> GetAllAsync()
    {
        var comments = await _context.Comments.ToListAsync();
        return  comments;
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        return comment;
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return comment;
    }
}