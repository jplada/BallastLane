using AutoMapper;
using Clients.Services.API.Controllers;
using Clients.Services.API.Models.Dto;
using Clients.Services.Business.Models.Clients;
using Clients.Services.Business.Models.Dto;
using Clients.Services.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Clients.Services.API.Testing
{
    public class ClientsControllerTest
    {
        [Fact]
        public async Task GetAll_Succeeds()
        {
            var clientService = new Mock<IClientService>();
            var client = CreateTestClient();
            clientService.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Client> { client}).Verifiable();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Client, ClientDto>();
            });
            var mapper = config.CreateMapper();
            var controller = new ClientsController(clientService.Object, mapper);

            var response = await controller.GetAll();

            Assert.NotNull(response);
            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var objectResult = okResult.Value as ResponseDto<IEnumerable<ClientDto>>;
            Assert.NotNull(objectResult);
            Assert.True(objectResult.Success);
            Assert.NotNull(objectResult.Result);
            clientService.Verify();
        }

        [Fact]
        public async Task GetById_Succeeds()
        {
            var clientService = new Mock<IClientService>();
            var client = CreateTestClient();
            clientService.Setup(x => x.FindByIdAsync(It.Is<string>(x => x == client.Id)))
                .ReturnsAsync( client ).Verifiable();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Client, ClientDto>();
            });
            var mapper = config.CreateMapper();
            var controller = new ClientsController(clientService.Object, mapper);

            var response = await controller.Get(client.Id);

            Assert.NotNull(response);
            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var objectResult = okResult.Value as ResponseDto<ClientDto>;
            Assert.NotNull(objectResult);
            Assert.True(objectResult.Success);
            Assert.NotNull(objectResult.Result);
            clientService.Verify();
        }

        [Fact]
        public async Task Create_Succeeds()
        {
            var clientService = new Mock<IClientService>();
            var clientDto = CreateTestDto();
            var client = CreateTestClient();
            clientService.Setup(x => x.InsertAsync(It.Is<Client>(x => x.Name == clientDto.Name)))
                .ReturnsAsync(client).Verifiable();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Client, ClientDto>().ReverseMap();
            });
            var mapper = config.CreateMapper();
            var controller = new ClientsController(clientService.Object, mapper);

            var response = await controller.Create(clientDto);

            Assert.NotNull(response);
            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var objectResult = okResult.Value as ResponseDto<ClientDto>;
            Assert.NotNull(objectResult);
            Assert.True(objectResult.Success);
            Assert.NotNull(objectResult.Result);
            clientService.Verify();
        }

        [Fact]
        public async Task Create_ExceptionThrown_ReturnsBadRequest()
        {
            var clientService = new Mock<IClientService>();
            var clientDto = CreateTestDto();
            var client = CreateTestClient();
            clientService.Setup(x => x.InsertAsync(It.Is<Client>(x => x.Name == clientDto.Name)))
                .ThrowsAsync(new ArgumentException("Duplicated")).Verifiable();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Client, ClientDto>().ReverseMap();
            });
            var mapper = config.CreateMapper();
            var controller = new ClientsController(clientService.Object, mapper);

            var response = await controller.Create(clientDto);

            Assert.NotNull(response);
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            var objectResult = badRequestResult.Value as ResponseDto<ClientDto>;
            Assert.NotNull(objectResult);
            Assert.False(objectResult.Success);
            clientService.Verify();
        }

        [Fact]
        public async Task Update_Succeeds()
        {
            var clientService = new Mock<IClientService>();
            var clientDto = CreateTestDto();
            clientService.Setup(x => x.UpdateAsync(It.Is<Client>(x => x.Name == clientDto.Name)))
                .Returns(Task.CompletedTask).Verifiable();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Client, ClientDto>().ReverseMap();
            });
            var mapper = config.CreateMapper();
            var controller = new ClientsController(clientService.Object, mapper);

            var response = await controller.Update(clientDto.Id, clientDto);

            Assert.NotNull(response);
            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var objectResult = okResult.Value as ResponseDto<bool>;
            Assert.NotNull(objectResult);
            Assert.True(objectResult.Success);
            clientService.Verify();
        }

        [Fact]
        public async Task Update_ExceptionThrown_ReturnsBadRequest()
        {
            var clientService = new Mock<IClientService>();
            var clientDto = CreateTestDto();
            clientService.Setup(x => x.UpdateAsync(It.Is<Client>(x => x.Name == clientDto.Name)))
                .ThrowsAsync(new ArgumentException("Duplicated")).Verifiable();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Client, ClientDto>().ReverseMap();
            });
            var mapper = config.CreateMapper();
            var controller = new ClientsController(clientService.Object, mapper);

            var response = await controller.Update(clientDto.Id, clientDto);           

            Assert.NotNull(response);
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            var objectResult = badRequestResult.Value as ResponseDto<bool>;
            Assert.NotNull(objectResult);
            Assert.False(objectResult.Success);
            clientService.Verify();
        }

        [Fact]
        public async Task Delete_Succeeds()
        {
            var clientService = new Mock<IClientService>();
            var clientId = "asdfhb";
            clientService.Setup(x => x.DeleteByIdAsync(It.Is<string>(x => x == clientId)))
                .Returns(Task.CompletedTask).Verifiable();
            var config = new MapperConfiguration(cfg => {
            });
            var mapper = config.CreateMapper();
            var controller = new ClientsController(clientService.Object, mapper);

            var response = await controller.Delete(clientId);

            Assert.NotNull(response);
            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var objectResult = okResult.Value as ResponseDto<bool>;
            Assert.NotNull(objectResult);
            Assert.True(objectResult.Success);
            clientService.Verify();
        }

        private Client CreateTestClient()
        {
            return new Client
            {
                Id = "abcd",
                Name = "Company A",
                Address = "123 Main St",
                Phone = "555 123123"
            };
        }

        private ClientDto CreateTestDto()
        {
            return new ClientDto
            {
                Name = "Company A",
                Address = "123 Main St",
                Phone = "555 123123"
            };
        }
    }
}