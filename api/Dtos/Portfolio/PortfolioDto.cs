namespace api.Dtos.Portfolio;

public class PortfolioDto
{
    public int CryptoAssetId { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal TotalValue { get; set; }
    public string Change24HPercent { get; set; } = string.Empty;
    public string? LogoUrl { get; set; }
}
