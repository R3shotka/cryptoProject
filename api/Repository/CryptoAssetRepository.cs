using api.Data;
using api.Dtos.CryptoAsset;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CryptoAssetRepository : ICryptoAssetRepository
{
    private readonly ApplicationDBContext _context;
    public CryptoAssetRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<List<CryptoAsset>> GetAllAsync(QueryObject query)
    {
        var assets = _context.CryptoAssets.Include(c => c.Comments).AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(query.Symbol))
        {
            assets = assets.Where(c => c.Symbol.Contains(query.Symbol));
        }

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            assets = assets.Where(c => c.Name.Contains(query.Name));
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                assets = query.IsDescending ? assets.OrderByDescending(c => c.Symbol) : assets.OrderBy(c => c.Symbol);
            }
            else if (query.SortBy.Equals("Change24HPercent", StringComparison.OrdinalIgnoreCase))
            {
                assets = query.IsDescending ? assets.OrderByDescending(c => c.Change24HPercent) : assets.OrderBy(c => c.Change24HPercent);
            }
            else if (query.SortBy.Equals("Price", StringComparison.OrdinalIgnoreCase))
            {
                assets = query.IsDescending ?  assets.OrderByDescending(c => c.Price) : assets.OrderBy(c => c.Price);
            }
        }
        
        var skipNumber = (query.PageNumber - 1) * query.PageSize;
        
        return await assets.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async Task<CryptoAsset?> GetByIdAsync(int id)
    {
        return await _context.CryptoAssets.Include(c => c.Comments).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

    }

    public async Task<CryptoAsset?> GetByExternalIdAsync(string externalId)
    {
        return await _context.CryptoAssets.AsNoTracking().FirstOrDefaultAsync(c => c.ExternalId == externalId);
    }

    public async Task<Dictionary<string, int>> GetIdsByExternalIdsAsync(List<string> externalIds)
    {
        return await _context.CryptoAssets
            .Where(a => externalIds.Contains(a.ExternalId))
            .AsNoTracking()
            .ToDictionaryAsync(a => a.ExternalId, a => a.Id);
    }

    public async Task<List<CryptoAsset>> SearchAsync(string query)
    {
        var lowerQuery = query.ToLower();
        return await _context.CryptoAssets
            .Where(c => c.Symbol.ToLower().Contains(lowerQuery) ||
                        c.Name.ToLower().Contains(lowerQuery) ||
                        c.ExternalId.ToLower().Contains(lowerQuery))
            .AsNoTracking()
            .Take(10)
            .ToListAsync();
    }

    public async Task<CryptoAsset> CreateAsync(CryptoAsset cryptoAsset)
    {
        await _context.CryptoAssets.AddAsync(cryptoAsset);
        await _context.SaveChangesAsync();
        return cryptoAsset;
    }

    public async Task<CryptoAsset?> UpdateAsync(int id, UpdateCryptoAssetDto updateCryptoAssetDto)
    {
        var cryptoAsserModel = await _context.CryptoAssets.FirstOrDefaultAsync(c => c.Id == id);
        if (cryptoAsserModel == null)
        {
            return null;
        }
        cryptoAsserModel.Symbol = updateCryptoAssetDto.Symbol;
        cryptoAsserModel.Name = updateCryptoAssetDto.Name;
        cryptoAsserModel.Price = updateCryptoAssetDto.Price;
        cryptoAsserModel.Change24HPercent = updateCryptoAssetDto.Change24HPercent;
        await _context.SaveChangesAsync();
        
        return cryptoAsserModel;
    }

    public async Task<CryptoAsset?> DeleteAsync(int id)
    {
        var cryptoAssetModel = await _context.CryptoAssets.FirstOrDefaultAsync(c => c.Id == id);
        if (cryptoAssetModel == null)
        {
            return null;
        }

        _context.CryptoAssets.Remove(cryptoAssetModel);
        await _context.SaveChangesAsync();
        return cryptoAssetModel;
    }

    public Task<bool> CryptoAssetExists(int id)
    {
        return _context.CryptoAssets.AnyAsync(c => c.Id == id);
    }
}