namespace api.Dtos.CryptoAsset;

public class UpdateCryptoAssetDto
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Change24HPercent { get; set; }
}