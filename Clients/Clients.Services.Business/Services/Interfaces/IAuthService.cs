using Clients.Services.Business.Models.Auth;

namespace Clients.Services.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterUser(User registerUser);
        Task<LoginResponse> Login(Login login);
    }
}
