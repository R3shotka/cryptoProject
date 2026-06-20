using api.Models;
using api.Dtos.Comment;

namespace api.Dtos.CryptoAsset;

public class CryptoAssetDto
{
    public int Id { get; set; } 
    public string Symbol { get; set; } = string.Empty; // btc
    public string Name { get; set; } = string.Empty; // bitcoin
    public string ExternalId { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? LogoUrl { get; set; }
    public string Change24HPercent { get; set; }  = string.Empty;

    public List<CommentDto?> Comments { get; set; }


}