using Clients.Services.Business.Models.Auth;

namespace Clients.Services.Business.Services.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
