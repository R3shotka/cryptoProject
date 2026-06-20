using System.ComponentModel.DataAnnotations;

namespace api.Dtos.CryptoAsset;

public class CreateCryptoAssetRequesDto
{
    [Required]
    [StringLength(10, ErrorMessage = "Symbol cannot exceed 10 characters")]
    public string Symbol { get; set; } = string.Empty;

    [Required]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(100, ErrorMessage = "ExternalId cannot exceed 100 characters")]
    public string ExternalId { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
    public decimal Price { get; set; }

    [Range(-100, 10000, ErrorMessage = "Change24HPercent must be between -100 and 10000")]
    public decimal Change24HPercent { get; set; }
}