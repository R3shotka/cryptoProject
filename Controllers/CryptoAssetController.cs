using api.Data;
using api.Dtos.CryptoAsset;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Repository;
using api.Interfaces;
using api.Repository;
using api.Services;
using api.InterfacesService;

namespace api.Controllers;
[Route("api/CryptoAsset")]
[ApiController]
public class CryptoAssetController : ControllerBase
{
    
    private readonly ApplicationDBContext _context;
    private readonly ICryptoAssetRepository _cryptoAssetRepo;
    private readonly ICoinGeckoService _coinGeckoService;
    public CryptoAssetController(ApplicationDBContext context,  ICryptoAssetRepository cryptoAssetRepo, ICoinGeckoService coinGeckoService)
    {
        _context = context;
        _cryptoAssetRepo = cryptoAssetRepo;
        _coinGeckoService = coinGeckoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cryptoAssets = await _cryptoAssetRepo.GetAllAsync();
        var cryptoAssetDto = cryptoAssets.Select(c => c.ToCryptoAssetDto()).ToList();
        return Ok(cryptoAssetDto);
    }
    
    

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var cryptoAsset = await _cryptoAssetRepo.GetByIdAsync(id);
        if (cryptoAsset == null)
        {
            return NotFound();
        }
        return Ok(cryptoAsset.ToCryptoAssetDto());
    }
    
    
    [HttpGet("{id}/live")]
    public async Task<IActionResult> GetByIdWithCoinGeckoMarket([FromRoute] int id)
    {
        var cryptoAsset = await _cryptoAssetRepo.GetByIdAsync(id);
        if (cryptoAsset == null)
        {
            return NotFound();
        }
        
        var dto = cryptoAsset.ToCryptoAssetDto();
        if (!String.IsNullOrWhiteSpace(dto.ExternalId))
        {
            
            var market = await _coinGeckoService.GetMarketAsync(dto.ExternalId);
            
            if (market != null)
            {
                var changeConvert = market.Change24HPercent > 0 ? "+" : "";
                dto.Price = market.Price;
                dto.Change24HPercent = $"{changeConvert}{market.Change24HPercent:0.00}%";
            }
        }
        
        
        return Ok(dto);
    }
    
    

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCryptoAssetRequesDto createCryptoAssetRequesDto)
    {
        var cryptoAssetModel = createCryptoAssetRequesDto.ToCryptoAssetFromCreateDto();
        await _cryptoAssetRepo.CreateAsync(cryptoAssetModel);
        return CreatedAtAction(nameof(GetById), new { id = cryptoAssetModel.Id }, cryptoAssetModel.ToCryptoAssetDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCryptoAssetDto updateCryptoAssetDto)
    {
        var cryptoAssetModel = await _cryptoAssetRepo.UpdateAsync(id,  updateCryptoAssetDto);
        if (cryptoAssetModel == null)
        {
            return NotFound();
        }
        return Ok(cryptoAssetModel.ToCryptoAssetDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var cryptoAssetModel = await _cryptoAssetRepo.DeleteAsync(id);
        if (cryptoAssetModel == null)
        {
            return NotFound();
        }
        return NoContent();
    }

}