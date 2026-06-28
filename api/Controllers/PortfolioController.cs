using api.Dtos.Portfolio;
using api.Extensions;
using api.Interfaces;
using api.InterfacesService;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PortfolioController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserAssetsBalanceRepository _portfolioRepo;
    private readonly ICryptoAssetRepository _cryptoRepo;
    private readonly ICoinGeckoService _coinGeckoService;

    public PortfolioController(
        UserManager<AppUser> userManager,
        IUserAssetsBalanceRepository portfolioRepo,
        ICryptoAssetRepository cryptoRepo,
        ICoinGeckoService coinGeckoService)
    {
        _userManager = userManager;
        _portfolioRepo = portfolioRepo;
        _cryptoRepo = cryptoRepo;
        _coinGeckoService = coinGeckoService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolio()
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);

        if (appUser == null)
        {
            return Unauthorized();
        }

        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

        // Паралельно отримуємо LIVE ціни для всіх активів з ExternalId
        var portfolioDtoTasks = userPortfolio.Select(async portfolioItem =>
        {
            var currentPrice = portfolioItem.CryptoAsset.Price;
            var change24h = portfolioItem.CryptoAsset.Change24HPercent;

            // Якщо є ExternalId - отримуємо live дані з CoinGecko
            if (!string.IsNullOrWhiteSpace(portfolioItem.CryptoAsset.ExternalId))
            {
                var liveMarketData = await _coinGeckoService.GetMarketAsync(portfolioItem.CryptoAsset.ExternalId);

                if (liveMarketData != null)
                {
                    currentPrice = liveMarketData.Price;
                    change24h = liveMarketData.Change24HPercent;
                }
            }

            return new PortfolioDto
            {
                CryptoAssetId = portfolioItem.CryptoAssetId,
                Symbol = portfolioItem.CryptoAsset.Symbol,
                Name = portfolioItem.CryptoAsset.Name,
                Quantity = portfolioItem.Quantity,
                CurrentPrice = currentPrice,
                TotalValue = portfolioItem.Quantity * currentPrice,
                Change24HPercent = change24h > 0 ? $"+{change24h:0.00}%" : $"{change24h:0.00}%",
                LogoUrl = portfolioItem.CryptoAsset.LogoUrl
            };
        });

        var portfolioDtos = await Task.WhenAll(portfolioDtoTasks);

        return Ok(portfolioDtos);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddToPortfolio([FromBody] AddPortfolioDto addDto)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);

        if (appUser == null)
        {
            return Unauthorized();
        }

        var cryptoAsset = await _cryptoRepo.GetAllAsync(new Helpers.QueryObject { Symbol = addDto.Symbol });
        var asset = cryptoAsset.FirstOrDefault();

        if (asset == null)
        {
            var searchResults = await _coinGeckoService.SearchAsync(addDto.Symbol);
            var coinData = searchResults.FirstOrDefault();

            if (coinData == null)
            {
                return NotFound($"Cryptocurrency with symbol '{addDto.Symbol}' not found");
            }

            asset = new CryptoAsset
            {
                Symbol = coinData.Symbol,
                Name = coinData.Name,
                ExternalId = coinData.ExternalId,
                LogoUrl = coinData.LogoUrl,
                Price = 0,
                Change24HPercent = 0
            };

            await _cryptoRepo.CreateAsync(asset);
        }

        var portfolioItem = await _portfolioRepo.AddToPortfolio(appUser, asset.Id, addDto.Quantity);

        if (portfolioItem == null)
        {
            return StatusCode(500, "Could not add to portfolio");
        }

        var portfolioDto = new PortfolioDto
        {
            CryptoAssetId = portfolioItem.CryptoAssetId,
            Symbol = portfolioItem.CryptoAsset.Symbol,
            Name = portfolioItem.CryptoAsset.Name,
            Quantity = portfolioItem.Quantity,
            CurrentPrice = portfolioItem.CryptoAsset.Price,
            TotalValue = portfolioItem.Quantity * portfolioItem.CryptoAsset.Price,
            Change24HPercent = portfolioItem.CryptoAsset.Change24HPercent.ToString(),
            LogoUrl = portfolioItem.CryptoAsset.LogoUrl
        };

        return Created($"/api/Portfolio", portfolioDto);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> RemoveFromPortfolio([FromQuery] string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);

        if (appUser == null)
        {
            return Unauthorized();
        }

        var portfolioItem = await _portfolioRepo.RemoveFromPortfolio(appUser, symbol);

        if (portfolioItem == null)
        {
            return NotFound($"Asset with symbol '{symbol}' not found in portfolio");
        }

        return NoContent();
    }
}
