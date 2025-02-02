using back_end.Models;
using System.Security.Claims;

namespace back_end.Services.Token
{
    public interface ITokenService
    {
        string GenerateToken(Account userAccount);
        bool ValidateToken(string token);
    }
}
