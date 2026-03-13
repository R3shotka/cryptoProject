using Microsoft.AspNetCore.Identity;

namespace api.Models;

public class AppUser : IdentityUser
{
    public List<UserAssetBalance> UserAssetBalances { get; set; } = new List<UserAssetBalance>();
}