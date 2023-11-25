using Clients.Data.Models;

namespace Clients.Services.Business.Models.Auth
{
    public class LoginResponse
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
