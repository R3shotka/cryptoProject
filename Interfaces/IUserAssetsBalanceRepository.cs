using api.Models;

namespace api.Interfaces;

public interface IUserAssetsBalanceRepository
{
     Task <List<CryptoAsset>> GetAssetsUserBalance(AppUser appUser); 
}