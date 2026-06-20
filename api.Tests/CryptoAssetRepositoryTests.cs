using api.Data;
using api.Models;
using api.Repository;
using api.Dtos.CryptoAsset;
using api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace api.Tests;

public class CryptoAssetRepositoryTests
{
    private ApplicationDBContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDBContext(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllAssets()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var repository = new CryptoAssetRepository(context);

        var asset1 = new CryptoAsset { Symbol = "BTC", Name = "Bitcoin", ExternalId = "bitcoin", Price = 50000 };
        var asset2 = new CryptoAsset { Symbol = "ETH", Name = "Ethereum", ExternalId = "ethereum", Price = 3000 };

        await context.CryptoAssets.AddRangeAsync(asset1, asset2);
        await context.SaveChangesAsync();

        var query = new QueryObject { PageNumber = 1, PageSize = 10 };

        // Act
        var result = await repository.GetAllAsync(query);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, a => a.Symbol == "BTC");
        Assert.Contains(result, a => a.Symbol == "ETH");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectAsset()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var repository = new CryptoAssetRepository(context);

        var asset = new CryptoAsset { Symbol = "BTC", Name = "Bitcoin", ExternalId = "bitcoin", Price = 50000 };
        await context.CryptoAssets.AddAsync(asset);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(asset.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("BTC", result.Symbol);
        Assert.Equal("Bitcoin", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var repository = new CryptoAssetRepository(context);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_AddsNewAsset()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var repository = new CryptoAssetRepository(context);

        var newAsset = new CryptoAsset
        {
            Symbol = "BTC",
            Name = "Bitcoin",
            ExternalId = "bitcoin",
            Price = 50000,
            Change24HPercent = 5.5m
        };

        // Act
        var result = await repository.CreateAsync(newAsset);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal("BTC", result.Symbol);

        var dbAsset = await context.CryptoAssets.FindAsync(result.Id);
        Assert.NotNull(dbAsset);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesExistingAsset()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var repository = new CryptoAssetRepository(context);

        var asset = new CryptoAsset { Symbol = "BTC", Name = "Bitcoin", ExternalId = "bitcoin", Price = 50000 };
        await context.CryptoAssets.AddAsync(asset);
        await context.SaveChangesAsync();

        var updateDto = new UpdateCryptoAssetDto
        {
            Symbol = "BTC",
            Name = "Bitcoin Updated",
            Price = 55000,
            Change24HPercent = 10.5m
        };

        // Act
        var result = await repository.UpdateAsync(asset.Id, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Bitcoin Updated", result.Name);
        Assert.Equal(55000, result.Price);
        Assert.Equal(10.5m, result.Change24HPercent);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var repository = new CryptoAssetRepository(context);

        var updateDto = new UpdateCryptoAssetDto { Symbol = "BTC", Name = "Bitcoin", Price = 50000 };

        // Act
        var result = await repository.UpdateAsync(999, updateDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_RemovesAsset()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var repository = new CryptoAssetRepository(context);

        var asset = new CryptoAsset { Symbol = "BTC", Name = "Bitcoin", ExternalId = "bitcoin", Price = 50000 };
        await context.CryptoAssets.AddAsync(asset);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.DeleteAsync(asset.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("BTC", result.Symbol);

        var dbAsset = await context.CryptoAssets.FindAsync(asset.Id);
        Assert.Null(dbAsset);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var repository = new CryptoAssetRepository(context);

        // Act
        var result = await repository.DeleteAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_FiltersBy_Symbol()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var repository = new CryptoAssetRepository(context);

        await context.CryptoAssets.AddRangeAsync(
            new CryptoAsset { Symbol = "BTC", Name = "Bitcoin", ExternalId = "bitcoin", Price = 50000 },
            new CryptoAsset { Symbol = "ETH", Name = "Ethereum", ExternalId = "ethereum", Price = 3000 }
        );
        await context.SaveChangesAsync();

        var query = new QueryObject { Symbol = "BTC", PageNumber = 1, PageSize = 10 };

        // Act
        var result = await repository.GetAllAsync(query);

        // Assert
        Assert.Single(result);
        Assert.Equal("BTC", result[0].Symbol);
    }

    [Fact]
    public async Task GetAllAsync_SortsBy_Price_Descending()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var repository = new CryptoAssetRepository(context);

        await context.CryptoAssets.AddRangeAsync(
            new CryptoAsset { Symbol = "BTC", Name = "Bitcoin", ExternalId = "bitcoin", Price = 50000 },
            new CryptoAsset { Symbol = "ETH", Name = "Ethereum", ExternalId = "ethereum", Price = 3000 },
            new CryptoAsset { Symbol = "BNB", Name = "Binance Coin", ExternalId = "binancecoin", Price = 400 }
        );
        await context.SaveChangesAsync();

        var query = new QueryObject { SortBy = "Price", IsDescending = true, PageNumber = 1, PageSize = 10 };

        // Act
        var result = await repository.GetAllAsync(query);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("BTC", result[0].Symbol);
        Assert.Equal("ETH", result[1].Symbol);
        Assert.Equal("BNB", result[2].Symbol);
    }
}
