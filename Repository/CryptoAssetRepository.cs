using api.Data;
using api.Dtos.CryptoAsset;
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
    public async Task<List<CryptoAsset>> GetAllAsync()
    {
        return await _context.CryptoAssets.Include(c => c.Comments).ToListAsync();
    }

    public async Task<CryptoAsset?> GetByIdAsync(int id)
    {
        return await _context.CryptoAssets.Include(c => c.Comments).FirstOrDefaultAsync(c => c.Id == id);
        
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