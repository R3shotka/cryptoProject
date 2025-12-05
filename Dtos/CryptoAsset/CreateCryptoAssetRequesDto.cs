namespace api.Dtos.CryptoAsset;

public class CreateCryptoAssetRequesDto
{
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    
    
    public decimal Price { get; set; }
    
    public decimal Change24HPercent { get; set; }
}