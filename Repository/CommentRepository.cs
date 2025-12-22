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

    public async Task<Comment?> UpdateAsync(int id, Comment comment)
    {
        var existComment = await _context.Comments.FindAsync(id);
        if (existComment is null)
        {
            return null;
        }
        
        existComment.Title = comment.Title;
        existComment.Content = comment.Content;
        await _context.SaveChangesAsync();
        return existComment;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        var existComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        if (existComment is null)
        {
            return null;
        }

        _context.Comments.Remove(existComment);
        await _context.SaveChangesAsync();
        return existComment;
    }
}