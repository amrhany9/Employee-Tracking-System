using back_end.Models;
using System.Security.Claims;

namespace back_end.Services.Token
{
    public interface ITokenService
    {
        string GenerateJwtToken(Account userAccount);
        string GenerateRefreshToken();
        bool ValidateToken(string token);
    }
}
