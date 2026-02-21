using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using api.Models;
using api.Dtos.Account;
using api.Interfaces;
using api.InterfacesService;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController] 
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)  
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };
            
            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                if (roleResult.Succeeded)
                {
                    var newUser =  new NewUserDto
                    {
                        Name = appUser.UserName,
                        Email = appUser.Email,
                        Token = _tokenService.CreateToken(appUser)
                    };
                    
                    return Ok(newUser);

                }
                else
                {
                    
                    return StatusCode(500, roleResult.Errors );
                }
            }
            else
            {
                return StatusCode(500, createdUser.Errors );
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
    }
    
}