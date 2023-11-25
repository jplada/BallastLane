using AutoMapper;
using Clients.Services.API.Models.Dto;
using Clients.Services.Business.Models.Clients;
using Clients.Services.Business.Models.Dto;
using Clients.Services.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clients.Services.API.Controllers
{
    [Route("api/clients")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService clientService;
        private readonly IMapper mapper;

        public ClientsController(IClientService clientService,
            IMapper mapper)
        {
            this.clientService = clientService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await clientService.GetAllAsync();
            var result = mapper.Map<IEnumerable<ClientDto>>(clients);
            return Ok(new ResponseDto<IEnumerable<ClientDto>>(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var client = await clientService.FindByIdAsync(id);
            var result = mapper.Map<ClientDto>(client);
            return Ok(new ResponseDto<ClientDto>(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientDto clientDto)
        {
            try
            {
                var client = mapper.Map<Client>(clientDto);
                var clientCreated = await clientService.InsertAsync(client);
                var result = mapper.Map<ClientDto>(clientCreated);
                return Ok(new ResponseDto<ClientDto>(result));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseDto<ClientDto>.Error(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] ClientDto clientDto)
        {
            try
            {
                var client = mapper.Map<Client>(clientDto);
                client.Id = id;
                await clientService.UpdateAsync(client);
                return Ok(new ResponseDto<bool>(true));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseDto<bool>.Error(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await clientService.DeleteByIdAsync(id);
            return Ok(new ResponseDto<bool>(true));
        }
    }
}
