using System;
using api.Data;
using api.Models;



namespace api.Interfaces;


public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync();
    Task<List<Comment>> GetAllAsync(int page, int pageSize, int? cryptoAssetId = null);
    Task<int> GetTotalCountAsync(int? cryptoAssetId = null);
    Task<Comment?> GetByIdAsync(int id);
    Task<Comment> CreateAsync(Comment comment);
    Task<Comment?> UpdateAsync(int id, Comment comment);
    Task<Comment?> DeleteAsync(int id);
}