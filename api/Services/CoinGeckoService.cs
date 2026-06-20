using api.Dtos.CryptoAsset;
using api.InterfacesService;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

namespace api.Services;

public class CoinGeckoService : ICoinGeckoService
{
    private readonly ILogger<CoinGeckoService> _logger;
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(5, 5);

    public CoinGeckoService(HttpClient httpClient, ILogger<CoinGeckoService> logger, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _logger = logger;
        _cache = cache;
        _httpClient.BaseAddress = new Uri("https://api.coingecko.com/api/v3/");
    }
    
    public async Task<List<CryptoSearchResultDto>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<CryptoSearchResultDto>();

        var cacheKey = $"search_{query.ToLower()}";

        if (_cache.TryGetValue(cacheKey, out List<CryptoSearchResultDto>? cachedResult) && cachedResult != null)
        {
            _logger.LogInformation("Returning cached search results for query: {Query}", query);
            return cachedResult;
        }

        var url = $"search?query={Uri.EscapeDataString(query)}";
        CoinGeckoSearchResponse? response = null;

        await _semaphore.WaitAsync();
        try
        {
            response = await _httpClient.GetFromJsonAsync<CoinGeckoSearchResponse>(url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling CoinGecko search");
            return new List<CryptoSearchResultDto>();
        }
        finally
        {
            _semaphore.Release();
        }

        if (response == null || response.coins == null)
            return new List<CryptoSearchResultDto>();

        var results = response.coins.Select(c => new CryptoSearchResultDto
        {
            Symbol = c.symbol.ToUpperInvariant(),
            Name = c.name,
            ExternalId = c.id,
            LogoUrl = c.large // Використовуємо large замість thumb для кращої якості
        }).ToList();

        _cache.Set(cacheKey, results, TimeSpan.FromMinutes(30));

        return results;
    }

    public async Task<CryptoMarketDto?> GetMarketAsync(string externalId, string vsCurrency = "usd")
    {
        if (string.IsNullOrWhiteSpace(externalId))
            return null;

        var cacheKey = $"market_{externalId}_{vsCurrency}";

        if (_cache.TryGetValue(cacheKey, out CryptoMarketDto? cachedResult) && cachedResult != null)
        {
            _logger.LogInformation("Returning cached market data for: {ExternalId}", externalId);
            return cachedResult;
        }

        var url = $"coins/markets?vs_currency={vsCurrency}&ids={Uri.EscapeDataString(externalId)}";

        await _semaphore.WaitAsync();
        try
        {
            var resp = await _httpClient.GetAsync(url);

            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync();
                _logger.LogWarning("CoinGecko market failed. Status={Status}. Url={Url}. Body={Body}",
                    resp.StatusCode, url, body);
                return null;
            }

            var response = await resp.Content.ReadFromJsonAsync<List<CoinGeckoMarketItem>>();
            var item = response?.FirstOrDefault();
            if (item == null) return null;

            var result = new CryptoMarketDto
            {
                ExternalId = item.id,
                Symbol = item.symbol,
                Name = item.name,
                LogoUrl = item.image,
                Price = (decimal)item.current_price,
                Change24HPercent = (decimal)(item.price_change_percentage_24h ?? 0)
            };

            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling CoinGecko market data. Url={Url}", url);
            return null;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<List<PricePoint>> GetHistoricalDataAsync(string externalId, int days = 7)
    {
        if (string.IsNullOrWhiteSpace(externalId))
            return new List<PricePoint>();

        var cacheKey = $"historical_{externalId}_{days}";

        if (_cache.TryGetValue(cacheKey, out List<PricePoint>? cachedResult) && cachedResult != null)
        {
            _logger.LogInformation("Returning cached historical data for: {ExternalId}", externalId);
            return cachedResult;
        }

        var url = $"coins/{Uri.EscapeDataString(externalId)}/market_chart?vs_currency=usd&days={days}";

        await _semaphore.WaitAsync();
        try
        {
            var resp = await _httpClient.GetAsync(url);

            if (!resp.IsSuccessStatusCode)
            {
                _logger.LogWarning("CoinGecko historical data failed. Status={Status}. Url={Url}",
                    resp.StatusCode, url);
                return new List<PricePoint>();
            }

            var response = await resp.Content.ReadFromJsonAsync<CoinGeckoHistoricalResponse>();
            if (response?.prices == null || response.prices.Count == 0)
                return new List<PricePoint>();

            var result = response.prices.Select(p => new PricePoint
            {
                Timestamp = (long)p[0],
                Price = (decimal)p[1]
            }).ToList();

            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(30));

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling CoinGecko historical data. Url={Url}", url);
            return new List<PricePoint>();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<List<CryptoMarketDto>> GetTopCryptosAsync(int limit = 10)
    {
        var cacheKey = $"top_cryptos_{limit}";

        if (_cache.TryGetValue(cacheKey, out List<CryptoMarketDto>? cachedResult) && cachedResult != null)
        {
            _logger.LogInformation("Returning cached top cryptos");
            return cachedResult;
        }

        var url = $"coins/markets?vs_currency=usd&order=market_cap_desc&per_page={limit}&page=1";

        await _semaphore.WaitAsync();
        try
        {
            var resp = await _httpClient.GetAsync(url);

            if (!resp.IsSuccessStatusCode)
            {
                _logger.LogWarning("CoinGecko top cryptos failed. Status={Status}. Url={Url}",
                    resp.StatusCode, url);
                return new List<CryptoMarketDto>();
            }

            var response = await resp.Content.ReadFromJsonAsync<List<CoinGeckoMarketItem>>();
            if (response == null || response.Count == 0)
                return new List<CryptoMarketDto>();

            var result = response.Select(item => new CryptoMarketDto
            {
                ExternalId = item.id,
                Symbol = item.symbol,
                Name = item.name,
                LogoUrl = item.image,
                Price = (decimal)item.current_price,
                Change24HPercent = (decimal)(item.price_change_percentage_24h ?? 0)
            }).ToList();

            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling CoinGecko top cryptos. Url={Url}", url);
            return new List<CryptoMarketDto>();
        }
        finally
        {
            _semaphore.Release();
        }
    }


    private class CoinGeckoSearchResponse
    {
        public List<CoinItem> coins { get; set; } = new();
    }

    private class CoinGeckoHistoricalResponse
    {
        public List<List<double>> prices { get; set; } = new();
    }


    private class CoinGeckoMarketItem
    {
        public string  id { get; set; } =  string.Empty;
        public string symbol { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string image { get; set; } = string.Empty;
        public double current_price { get; set; }
        public double? price_change_percentage_24h { get; set; }
    }

    private class CoinItem
    {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string symbol { get; set; } = string.Empty;
        public string thumb { get; set; } = string.Empty;
        public string large { get; set; } = string.Empty;
    }
}