using api.Dtos.CryptoAsset;
using api.Models;

namespace api.Interfaces;

public interface ICryptoAssetRepository
{
    Task<List<CryptoAsset>> GetAllAsync();
    Task <CryptoAsset?> GetByIdAsync(int id);
    Task<CryptoAsset> CreateAsync(CryptoAsset cryptoAsset);
    Task<CryptoAsset?> UpdateAsync(int  id, UpdateCryptoAssetDto updateCryptoAssetDto);
    Task<CryptoAsset?> DeleteAsync(int id);
}