using api.Models;

namespace api.Mappers;
using System.Collections.Generic;
using api.Dtos.CryptoAsset;

public static class CryptoAssetMappers
{
    public static CryptoAssetDto ToCryptoAssetDto(this CryptoAsset cryptoAssetModel)
    {
        var changeConvert = cryptoAssetModel.Change24HPercent > 0 ? "+" : "";
        return new CryptoAssetDto
        {
            Id = cryptoAssetModel.Id,
            Symbol = cryptoAssetModel.Symbol,
            Name = cryptoAssetModel.Name,
            Price = cryptoAssetModel.Price,
            LogoUrl =  cryptoAssetModel.LogoUrl,
            Change24HPercent = $"{changeConvert}{cryptoAssetModel.Change24HPercent:0.00}%"
        };
    }

    public static CryptoAsset ToCryptoAssetFromCreateDto(this CreateCryptoAssetRequesDto createCryptoAssetRequesDto)
    {
        return new CryptoAsset
        {
            Symbol = createCryptoAssetRequesDto.Symbol,
            Name = createCryptoAssetRequesDto.Name,
            Price = createCryptoAssetRequesDto.Price,
            
            Change24HPercent = createCryptoAssetRequesDto.Change24HPercent
        };
    }
}