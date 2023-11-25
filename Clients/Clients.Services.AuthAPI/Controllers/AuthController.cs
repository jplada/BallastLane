using AutoMapper;
using Clients.Services.AuthAPI.Models.Dto;
using Clients.Services.Business.Models.Auth;
using Clients.Services.Business.Models.Dto;
using Clients.Services.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Clients.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IMapper mapper;

        public AuthController(IAuthService authService,
            IMapper mapper)
        {
            this.authService = authService;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registration)
        {
            try
            {
                User newUser = mapper.Map<User>(registration);
                var createdUser = await authService.RegisterUser(newUser);
                var result = mapper.Map<UserDto>(createdUser);
                return Ok(ResponseDto<UserDto>.Ok(result));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseDto<UserDto>.Error(ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginData)
        {
            try
            {
                Login login = mapper.Map<Login>(loginData);
                var loginResult = await authService.Login(login);
                var result = mapper.Map<LoginResponseDto>(loginResult);
                return Ok(ResponseDto<LoginResponseDto>.Ok(result));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseDto<LoginResponseDto>.Error(ex.Message));
            }
        }
    }
}
