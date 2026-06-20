using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class UserAssetsBalanceRepository : IUserAssetsBalanceRepository
{
    private readonly ApplicationDBContext _context;

    public UserAssetsBalanceRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<UserAssetBalance>> GetUserPortfolio(AppUser appUser)
    {
        return await _context.UserAssetBalances
            .Where(u => u.AppUserId == appUser.Id)
            .Include(u => u.CryptoAsset)
            .ToListAsync();
    }

    public async Task<UserAssetBalance?> AddToPortfolio(AppUser appUser, int cryptoAssetId, decimal quantity)
    {
        var existingAsset = await _context.UserAssetBalances
            .FirstOrDefaultAsync(u => u.AppUserId == appUser.Id && u.CryptoAssetId == cryptoAssetId);

        if (existingAsset != null)
        {
            existingAsset.Quantity += quantity;
        }
        else
        {
            var newAsset = new UserAssetBalance
            {
                AppUserId = appUser.Id,
                CryptoAssetId = cryptoAssetId,
                Quantity = quantity
            };
            await _context.UserAssetBalances.AddAsync(newAsset);
        }

        await _context.SaveChangesAsync();

        return await _context.UserAssetBalances
            .Include(u => u.CryptoAsset)
            .FirstOrDefaultAsync(u => u.AppUserId == appUser.Id && u.CryptoAssetId == cryptoAssetId);
    }

    public async Task<UserAssetBalance?> RemoveFromPortfolio(AppUser appUser, string symbol)
    {
        var assetToRemove = await _context.UserAssetBalances
            .Include(u => u.CryptoAsset)
            .FirstOrDefaultAsync(u => u.AppUserId == appUser.Id && u.CryptoAsset.Symbol.ToLower() == symbol.ToLower());

        if (assetToRemove == null)
        {
            return null;
        }

        _context.UserAssetBalances.Remove(assetToRemove);
        await _context.SaveChangesAsync();

        return assetToRemove;
    }

    public async Task<bool> AssetExistsInPortfolio(AppUser appUser, int cryptoAssetId)
    {
        return await _context.UserAssetBalances
            .AnyAsync(u => u.AppUserId == appUser.Id && u.CryptoAssetId == cryptoAssetId);
    }
}