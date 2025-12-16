using api.Dtos.CryptoAsset;
using api.InterfacesService;
using System.Net.Http.Json;
using api.Dtos.CryptoAsset;
using Microsoft.Extensions.Logging;

namespace api.Services;

public class CoinGeckoService : ICoinGeckoService
{
    private readonly ILogger<CoinGeckoService> _logger;
    private readonly HttpClient _httpClient;


    public CoinGeckoService(HttpClient httpClient, ILogger<CoinGeckoService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.BaseAddress = new Uri("https://api.coingecko.com/api/v3/");
    }
    
    public async Task<List<CryptoSearchResultDto>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<CryptoSearchResultDto>();

        // GET /search?query=btc
        var url = $"search?query={Uri.EscapeDataString(query)}";

        CoinGeckoSearchResponse? response = null;

        try
        {
            response = await _httpClient.GetFromJsonAsync<CoinGeckoSearchResponse>(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling CoinGecko search");
            return new List<CryptoSearchResultDto>();
        }

        if (response == null || response.coins == null)
            return new List<CryptoSearchResultDto>();

        return response.coins.Select(c => new CryptoSearchResultDto
        {
            Symbol = c.symbol.ToUpperInvariant(), // "btc" -> "BTC"
            Name = c.name,
            ExternalId = c.id,    // "bitcoin"
            LogoUrl = c.thumb
        }).ToList();
    
    }

    public async Task<CryptoMarketDto> GetMarketAsync(string externalId, string vsCurrency = "usd")
    {
        if (string.IsNullOrWhiteSpace(externalId))
            return new CryptoMarketDto();
        
        var url = $"coins/markets?vs_currency={vsCurrency}&ids={Uri.EscapeDataString(externalId)}";
        
        List<CoinGeckoMarketItem>? response = null;
        
        try
        {
            response = await _httpClient.GetFromJsonAsync<List<CoinGeckoMarketItem>>(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling CoinGecko market data");
            return null;
        }
        
        var item = response?.FirstOrDefault();
        if (item == null)
            return null;

        return new CryptoMarketDto
        {
            ExternalId = item.id,
            Price = (decimal)item.current_price,
            Change24HPercent = (decimal)(item.price_change_percentage_24h ?? 0)
        };
    }
    
    private class CoinGeckoSearchResponse
    {
        public List<CoinItem> coins { get; set; } = new();
    }

    private class CoinGeckoMarketItem
    {
        public string  id { get; set; } =  string.Empty;
        public double current_price { get; set; }
        public double? price_change_percentage_24h { get; set; }
    }

    private class CoinItem
    {
        public string id { get; set; } = string.Empty;     // "bitcoin"
        public string name { get; set; } = string.Empty;   // "Bitcoin"
        public string symbol { get; set; } = string.Empty; // "btc"
        public string thumb { get; set; } = string.Empty;  // url на іконку
    }
}