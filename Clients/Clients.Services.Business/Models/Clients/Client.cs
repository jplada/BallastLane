using Clients.Data.Models;

namespace Clients.Services.Business.Models.Clients
{
    public class Client: Document
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
