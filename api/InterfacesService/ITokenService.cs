using api.Models;

namespace api.InterfacesService;

public interface ITokenService
{
    string CreateToken(AppUser user);   
}