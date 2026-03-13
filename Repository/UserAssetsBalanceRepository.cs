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
    public async Task<List<CryptoAsset>> GetAssetsUserBalance(AppUser appUser)
    {
        return await _context.UserAssetBalances.Where(u => u.AppUserId == appUser.Id)
            .Select(cryptoAsset => new CryptoAsset
                {
                    Id = cryptoAsset.CryptoAssetId,
                    Name = cryptoAsset.CryptoAsset.Name,
                    Symbol = cryptoAsset.CryptoAsset.Symbol,
                    Price = cryptoAsset.CryptoAsset.Price,
                    Change24HPercent = cryptoAsset.CryptoAsset.Change24HPercent,
                    LogoUrl = cryptoAsset.CryptoAsset.LogoUrl
                    
                }).ToListAsync();
    }
}