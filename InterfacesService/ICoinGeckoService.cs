using api.Dtos.CryptoAsset;

namespace api.InterfacesService;

public interface ICoinGeckoService
{
    Task<List<CryptoSearchResultDto>> SearchAsync(string query);
    
    Task<CryptoMarketDto>  GetMarketAsync(string externalId, string vsCurrency = "usd");
}