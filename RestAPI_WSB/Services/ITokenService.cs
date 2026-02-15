using RestAPI_WSB.Models;

namespace RestAPI_WSB.Services;

public interface ITokenService
{
    string GenerateToken(ApplicationUser user);
}