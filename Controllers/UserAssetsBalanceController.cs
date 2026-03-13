using api.Interfaces;
using api.InterfacesService;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserAssetsBalanceController : ControllerBase
{
    private readonly ICryptoAssetRepository _cryptoAssetRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserAssetsBalanceRepository _userAssetsBalanceRepository;

    public UserAssetsBalanceController(UserManager<AppUser> userManager,ICryptoAssetRepository cryptoAssetRepository,
        IUserAssetsBalanceRepository userAssetBalanceRepository)
    {
        _userManager = userManager;
        _cryptoAssetRepository = cryptoAssetRepository;
        _userAssetsBalanceRepository = userAssetBalanceRepository;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAssetUser()
    {
        var userName = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(userName);
        var userAssetBalance =  await _userAssetsBalanceRepository.GetAssetsUserBalance(appUser); 
        return Ok(userAssetBalance);
    }
}