namespace api.Dtos.CryptoAsset;

public class CryptoSearchResultDto
{
    public string Symbol { get; set; } = string.Empty;     // "btc"
    public string Name { get; set; } = string.Empty;       // "Bitcoin"
    public string ExternalId { get; set; } = string.Empty; // "bitcoin" (CoinGecko id)
    public string? LogoUrl { get; set; }                   // маленька іконка
}