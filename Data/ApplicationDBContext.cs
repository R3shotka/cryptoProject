using Microsoft.EntityFrameworkCore;
using api.Models;

namespace api.Data;

public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {
        
    }

    public DbSet<CryptoAsset>  CryptoAssets { get; set; }
    public DbSet<Comment> Comments { get; set; }
}