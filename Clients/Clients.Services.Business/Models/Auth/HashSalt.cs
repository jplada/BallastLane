namespace Clients.Services.Business.Models.Auth
{
    internal class HashSalt
    {
        public string Hash { get; set; }
        public byte[] Salt { get; set; }
    }
}
