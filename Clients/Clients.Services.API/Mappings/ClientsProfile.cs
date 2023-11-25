using AutoMapper;
using Clients.Data.Models;
using Clients.Services.API.Models.Dto;
using Clients.Services.Business.Models.Clients;

namespace Clients.Services.API.Mappings
{
    public class ClientsProfile: Profile
    {
        public ClientsProfile()
        {
            CreateMap<Client, ClientDto>().ReverseMap();
        }
    }
}
