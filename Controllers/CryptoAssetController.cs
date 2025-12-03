using api.Data;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;
[Route("api/CryptoAsset")]
[ApiController]
public class CryptoAssetController : ControllerBase
{
    private readonly ApplicationDBContext _context;
    public CryptoAssetController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var cryptoAssets = _context.CryptoAssets.ToList();
        return Ok(cryptoAssets);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var cryptoAsset = _context.CryptoAssets.Find(id);
        if (cryptoAsset == null)
        {
            return NotFound();
        }
        return Ok(cryptoAsset);
    }
}