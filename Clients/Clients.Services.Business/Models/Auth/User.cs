using Clients.Data.Models;

namespace Clients.Services.Business.Models.Auth
{
    /// <summary>
    /// User Entity
    /// </summary>
    public class User : Document
    {
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public byte[] Salt { get; set; }
    }
}
