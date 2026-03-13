namespace api.Models;
using System.ComponentModel.DataAnnotations.Schema;
[Table("UserAssetBalances")]
public class UserAssetBalance
{
    public string AppUserId { get; set; }
    public int CryptoAssetId { get; set; }
    public AppUser AppUser { get; set; }
    public CryptoAsset CryptoAsset { get; set; }
}