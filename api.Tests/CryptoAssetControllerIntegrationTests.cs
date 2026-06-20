using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using api.Dtos.Account;
using api.Dtos.CryptoAsset;
using Microsoft.AspNetCore.Mvc.Testing;

namespace api.Tests;

public class CryptoAssetControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public CryptoAssetControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenAssetDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/CryptoAsset/99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WithValidData()
    {
        // Arrange
        var createDto = new CreateCryptoAssetRequesDto
        {
            Symbol = "TEST",
            Name = "Test Coin",
            ExternalId = "test-coin",
            Price = 100,
            Change24HPercent = 5
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/CryptoAsset", createDto);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var returnedDto = await response.Content.ReadFromJsonAsync<CryptoAssetDto>();
        Assert.NotNull(returnedDto);
        Assert.Equal("TEST", returnedDto.Symbol);
        Assert.Equal("Test Coin", returnedDto.Name);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WithInvalidData()
    {
        // Arrange - empty symbol violates [Required] validation
        var createDto = new CreateCryptoAssetRequesDto
        {
            Symbol = "",
            Name = "Test Coin",
            ExternalId = "test-coin",
            Price = 100,
            Change24HPercent = 5
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/CryptoAsset", createDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
