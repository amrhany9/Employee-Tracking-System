using back_end.Entities;
using System.Security.Claims;

namespace back_end.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UserAccount userAccount);
    }
}
