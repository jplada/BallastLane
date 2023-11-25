using Clients.Data.Repositories;
using Clients.Services.Business.Models.Auth;
using Clients.Services.Business.Services;
using Clients.Services.Business.Services.Interfaces;
using Moq;
using System.Linq.Expressions;

namespace Client.Services.Business.Testing
{
    public class AuthServiceTest
    {
        [Fact]
        public async Task RegisterUser_Succeeds()
        {
            var user = TestUser();
            Mock<IRepository<User>> repository = new Mock<IRepository<User>>();
            repository.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync((User)null).Verifiable();
            repository.Setup(x => x.InsertAsync(It.Is<User>(u => u.Name == user.Name))).ReturnsAsync(user).Verifiable();            
            Mock<IJwtTokenGenerator> tokenGenerator = new Mock<IJwtTokenGenerator>();
            var service = new AuthService(repository.Object, tokenGenerator.Object);

            var created = await service.RegisterUser(user);

            Assert.NotNull(created);
            repository.VerifyAll();
        }

        [Fact]
        public async Task RegisterUser_DuplicatedEmail_ThrowsException()
        {
            var user = TestUser();
            var existingUser = TestUser();
            Mock<IRepository<User>> repository = new Mock<IRepository<User>>();
            repository.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(existingUser).Verifiable();
            Mock<IJwtTokenGenerator> tokenGenerator = new Mock<IJwtTokenGenerator>();
            var service = new AuthService(repository.Object, tokenGenerator.Object);

            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await service.RegisterUser(user);
            });
            Assert.NotNull(exception);
            repository.VerifyAll();
        }

        [Fact]
        public async Task Login_Succeeds()
        {
            byte[] salt = new byte[] { 61, 119, 170, 59, 201, 48, 69, 142, 243, 206, 251, 137, 169, 5, 4, 164 };
            var user = TestUser();
            user.Id = "abcd1234";
            user.Salt = salt;
            user.Password = "vzAOw18nQKehblPRxlNwOIumGL2HqlH9noqvu1URGdw=";
            var login = new Login
            {
                Email = user.Email,
                Password = "password"
            };
            string token = "sdfhkdfbvdhjvfbkjdfsbvkjdfsbiremiewcvmervmo2354834norevtmnio";
            Mock<IRepository<User>> repository = new Mock<IRepository<User>>();
            repository.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(user).Verifiable();
            Mock<IJwtTokenGenerator> tokenGenerator = new Mock<IJwtTokenGenerator>();
            tokenGenerator.Setup(x => x.GenerateToken(It.Is<User>(u => u.Id == user.Id))).Returns(token).Verifiable();
            var service = new AuthService(repository.Object, tokenGenerator.Object);

            var loginResponse = await service.Login(login);

            Assert.NotNull(loginResponse);
            Assert.NotNull(loginResponse.User);
            Assert.Equal(token, loginResponse.Token);
            repository.VerifyAll();
            tokenGenerator.VerifyAll();
        }

        User TestUser()
        {
            User user = new User();
            user.Name = "Mary";
            user.Email = "mary@mymail.com";
            user.Password = "password";
            return user;
        }
    }
}