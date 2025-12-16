using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class CryptoAsset
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty; // btc
    public string Name { get; set; } = string.Empty; // bitcoin
    public string ExternalId { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    
    [Column(TypeName = "decimal(5,2)")]
    public decimal Change24HPercent { get; set; }
    
    public List<Comment> Comments { get; set; } = new List<Comment>();
}