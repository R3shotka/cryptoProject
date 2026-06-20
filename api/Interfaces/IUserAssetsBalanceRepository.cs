using api.Models;

namespace api.Interfaces;

public interface IUserAssetsBalanceRepository
{
    Task<List<UserAssetBalance>> GetUserPortfolio(AppUser appUser);
    Task<UserAssetBalance?> AddToPortfolio(AppUser appUser, int cryptoAssetId, decimal quantity);
    Task<UserAssetBalance?> RemoveFromPortfolio(AppUser appUser, string symbol);
    Task<bool> AssetExistsInPortfolio(AppUser appUser, int cryptoAssetId);
}