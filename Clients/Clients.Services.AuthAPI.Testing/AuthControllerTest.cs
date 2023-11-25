using AutoMapper;
using Clients.Services.AuthAPI.Controllers;
using Clients.Services.AuthAPI.Models.Dto;
using Clients.Services.Business.Models.Auth;
using Clients.Services.Business.Models.Dto;
using Clients.Services.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Clients.Services.AuthAPI.Testing
{
    public class AuthControllerTest
    {
        [Fact]
        public async Task Register_Succeeds()
        {
            var authService = new Mock<IAuthService>();
            var registerRequest = new UserRegistrationDto
            {
                Email = "tom@mymail.com",
                Name = "Tom",
                Password = "tom321"
            };
            var userResult = new User
            {
                Email = "tom@mymail.com",
                Name = "Tom"
            };
            authService.Setup(x => x.RegisterUser(It.Is<User>(u => u.Email == registerRequest.Email)))
                .ReturnsAsync(userResult).Verifiable();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserRegistrationDto, User>();
                cfg.CreateMap<User, UserDto>();
            });
            var mapper = config.CreateMapper();
            var controller = new AuthController(authService.Object, mapper);

            var response = await controller.Register(registerRequest);

            Assert.NotNull(response);
            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            authService.Verify();
        }

        [Fact]
        public async Task Register_OnArgumentException_ReturnsBadRequest()
        {
            var authService = new Mock<IAuthService>();
            var registerRequest = new UserRegistrationDto
            {
                Email = "tom@mymail.com",
                Name = "Tom",
                Password = "tom321"
            };
            authService.Setup(x => x.RegisterUser(It.Is<User>(u => u.Email == registerRequest.Email)))
                .ThrowsAsync(new ArgumentException("Duplicated Email")).Verifiable();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserRegistrationDto, User>();
                cfg.CreateMap<User, UserDto>();
            });
            var mapper = config.CreateMapper();
            var controller = new AuthController(authService.Object, mapper);

            var response = await controller.Register(registerRequest);

            Assert.NotNull(response);
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            authService.Verify();
        }

        [Fact]
        public async Task Login_Succeeds()
        {
            var authService = new Mock<IAuthService>();
            string token = "sdfkjhdasjfdlsgf235423gerw";
            var loginRequest = new LoginDto
            {
                Email = "tom@mymail.com",
                Password = "tom321"
            };
            var loginResult = new LoginResponse
            {
                User = new User
                {
                    Email = "tom@mymail.com",
                    Name = "Tom"
                },
                Token = token
            };
            authService.Setup(x => x.Login(It.Is<Login>(l => l.Email == loginRequest.Email)))
                .ReturnsAsync(loginResult).Verifiable();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<LoginDto, Login>();
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<LoginResponse, LoginResponseDto>();
            });
            var mapper = config.CreateMapper();
            var controller = new AuthController(authService.Object, mapper);

            var response = await controller.Login(loginRequest);

            Assert.NotNull(response);
            var okResult = response as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var objectResult = okResult.Value as ResponseDto<LoginResponseDto>;
            Assert.NotNull(objectResult);
            Assert.True(objectResult.Success);
            Assert.NotNull(objectResult.Result);
            Assert.NotNull(objectResult.Result.User);
            Assert.Equal(token, objectResult.Result.Token);
            authService.Verify();
        }

        [Fact]
        public async Task Login_IsInvalid_ReturnsBadRequest()
        {
            var authService = new Mock<IAuthService>();
            string token = "sdfkjhdasjfdlsgf235423gerw";
            var loginRequest = new LoginDto
            {
                Email = "tom@mymail.com",
                Password = "tom321"
            };
            authService.Setup(x => x.Login(It.Is<Login>(l => l.Email == loginRequest.Email)))
                .ThrowsAsync(new ArgumentException("Invalid Login")).Verifiable();
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<LoginDto, Login>();
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<LoginResponse, LoginResponseDto>();
            });
            var mapper = config.CreateMapper();
            var controller = new AuthController(authService.Object, mapper);

            var response = await controller.Login(loginRequest);

            Assert.NotNull(response);
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            var objectResult = badRequestResult.Value as ResponseDto<LoginResponseDto>;
            Assert.NotNull(objectResult);
            Assert.False(objectResult.Success);
            Assert.NotNull(objectResult.Message);
            authService.Verify();
        }
    }
}