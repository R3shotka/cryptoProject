namespace api.Dtos.CryptoAsset;

public class CryptoMarketDto
{
    public string ExternalId { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
    public decimal Price { get; set; }
    public decimal Change24HPercent { get; set; }
}