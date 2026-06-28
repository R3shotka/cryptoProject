using Microsoft.AspNetCore.Mvc;
using api.InterfacesService;
using api.Interfaces;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CryptoController : ControllerBase
{
    private readonly ICoinGeckoService _coinGeckoService;
    private readonly ICryptoAssetRepository _cryptoAssetRepo;

    public CryptoController(ICoinGeckoService coinGeckoService, ICryptoAssetRepository cryptoAssetRepo)
    {
        _coinGeckoService = coinGeckoService;
        _cryptoAssetRepo = cryptoAssetRepo;
    }
    
    // GET /api/crypto/search?query=btc
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("Query is required");
        }

        var results = await _coinGeckoService.SearchAsync(query);

        // Збагачуємо результати ID з бази даних (один запит замість N)
        var externalIds = results.Select(r => r.ExternalId).ToList();
        var existingAssets = await _cryptoAssetRepo.GetIdsByExternalIdsAsync(externalIds);

        foreach (var result in results)
        {
            if (existingAssets.TryGetValue(result.ExternalId, out var id))
            {
                result.Id = id;
            }
        }

        // Якщо CoinGecko не знайшов результатів або знайшов мало, шукаємо в локальній БД
        if (results.Count < 5)
        {
            var localResults = await _cryptoAssetRepo.SearchAsync(query);
            foreach (var localAsset in localResults)
            {
                // Додаємо тільки якщо ще немає в результатах
                if (!results.Any(r => r.ExternalId == localAsset.ExternalId))
                {
                    results.Add(new Dtos.CryptoAsset.CryptoSearchResultDto
                    {
                        Id = localAsset.Id,
                        Symbol = localAsset.Symbol,
                        Name = localAsset.Name,
                        ExternalId = localAsset.ExternalId,
                        LogoUrl = localAsset.LogoUrl
                    });
                }
            }
        }

        return Ok(results);
    }

    // GET /api/crypto/chart/{externalId}?days=7
    [HttpGet("chart/{externalId}")]
    public async Task<IActionResult> GetChart([FromRoute] string externalId, [FromQuery] int days = 7)
    {
        if (string.IsNullOrWhiteSpace(externalId))
        {
            return BadRequest("External ID is required");
        }

        var data = await _coinGeckoService.GetHistoricalDataAsync(externalId, days);

        if (data == null || data.Count == 0)
        {
            return NotFound($"Historical data for '{externalId}' not found");
        }

        return Ok(data);
    }

    // GET /api/crypto/top?limit=10
    [HttpGet("top")]
    public async Task<IActionResult> GetTopCryptos([FromQuery] int limit = 10)
    {
        var data = await _coinGeckoService.GetTopCryptosAsync(limit);
        return Ok(data);
    }
}