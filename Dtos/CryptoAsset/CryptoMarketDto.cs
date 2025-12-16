namespace api.Dtos.CryptoAsset;

public class CryptoMarketDto
{
    public string ExternalId { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Change24HPercent { get; set; }
}