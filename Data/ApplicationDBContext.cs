using Microsoft.EntityFrameworkCore;
using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace api.Data;

public class ApplicationDBContext : IdentityDbContext<AppUser>
{
    public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {
        
    }

    public DbSet<CryptoAsset>  CryptoAssets { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<UserAssetBalance> UserAssetBalances { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<UserAssetBalance>(x => x.HasKey(u => new { u.AppUserId, u.CryptoAssetId }));
        
        builder.Entity<UserAssetBalance>()
            .HasOne(u => u.AppUser)
            .WithMany(u => u.UserAssetBalances)
            .HasForeignKey(u => u.AppUserId);
        
        builder.Entity<UserAssetBalance>()
            .HasOne(u => u.CryptoAsset)
            .WithMany(u => u.UserAssetBalances)
            .HasForeignKey(u => u.CryptoAssetId);

        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },

            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER"
            }

        };
        
        builder.Entity<IdentityRole>().HasData(roles);
    } 
}