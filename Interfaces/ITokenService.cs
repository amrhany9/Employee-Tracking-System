using back_end.Models;
using System.Security.Claims;

namespace back_end.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Account userAccount);
    }
}
