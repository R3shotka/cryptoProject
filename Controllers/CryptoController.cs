using Microsoft.AspNetCore.Mvc;
using api.InterfacesService;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CryptoController : ControllerBase
{
    private readonly ICoinGeckoService _coinGeckoService;
    
    public CryptoController(ICoinGeckoService coinGeckoService)
    {
        _coinGeckoService = coinGeckoService;
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

        return Ok(results);
    }
}