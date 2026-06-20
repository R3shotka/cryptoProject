using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Portfolio;

public class AddPortfolioDto
{
    [Required]
    public string Symbol { get; set; } = string.Empty;

    [Required]
    [Range(0.00000001, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public decimal Quantity { get; set; }
}
