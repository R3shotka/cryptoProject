using api.Data;
using api.Dtos.CryptoAsset;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api.Repository;
using api.Interfaces;
using api.Repository;

namespace api.Controllers;
[Route("api/CryptoAsset")]
[ApiController]
public class CryptoAssetController : ControllerBase
{
    
    private readonly ApplicationDBContext _context;
    private readonly ICryptoAssetRepository _cryptoAssetRepo;
    public CryptoAssetController(ApplicationDBContext context,  ICryptoAssetRepository cryptoAssetRepo)
    {
        _context = context;
        _cryptoAssetRepo = cryptoAssetRepo;
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