using api.Dtos.CryptoAsset;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using api.Helpers;

namespace api.Interfaces;

public interface ICryptoAssetRepository
{
    Task<List<CryptoAsset>> GetAllAsync(QueryObject query);
    Task <CryptoAsset?> GetByIdAsync(int id);
    Task<CryptoAsset> CreateAsync(CryptoAsset cryptoAsset);
    Task<CryptoAsset?> UpdateAsync(int  id, UpdateCryptoAssetDto updateCryptoAssetDto);
    Task<CryptoAsset?> DeleteAsync(int id);
    Task<bool> CryptoAssetExists(int id);
}