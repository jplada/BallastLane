using System.ComponentModel.DataAnnotations;

namespace Clients.Services.API.Models.Dto
{
    public class ClientDto
    {
        public string? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
