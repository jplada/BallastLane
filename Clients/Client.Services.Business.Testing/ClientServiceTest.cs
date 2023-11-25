using Clients.Data.Repositories;
using Clients.Services.Business.Models.Clients;
using Clients.Services.Business.Services;
using Moq;
using System.Linq.Expressions;

namespace Clients.Services.Business.Testing
{
    public class ClientServiceTest
    {
        [Fact]
        public async Task FindByIdAsync_Succeeds()
        {
            var client = CreateTestClient();
            Mock<IRepository<Models.Clients.Client>> repository = new Mock<IRepository<Models.Clients.Client>>();
            repository.Setup(x => x.FindByIdAsync(It.Is<string>(x => x.Equals(client.Id)))).ReturnsAsync(client).Verifiable();
            var service = new ClientService(repository.Object);

            var result = await service.FindByIdAsync(client.Id);

            Assert.NotNull(result);
            repository.Verify();
        }

        [Fact]
        public async Task FindOneAsync_Succeeds()
        {
            var client = CreateTestClient();
            Mock<IRepository<Models.Clients.Client>> repository = new Mock<IRepository<Models.Clients.Client>>();
            repository.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<Models.Clients.Client, bool>>>()))
                .ReturnsAsync(client).Verifiable();
            var service = new ClientService(repository.Object);

            var result = await service.FindOneAsync(c => c.Name.Equals(client.Name));

            Assert.NotNull(result);
            repository.Verify();
        }

        [Fact]
        public async Task GetAllAsync_Succeeds()
        {
            var client = CreateTestClient();
            Mock<IRepository<Models.Clients.Client>> repository = new Mock<IRepository<Models.Clients.Client>>();
            repository.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Models.Clients.Client> { client }).Verifiable();
            var service = new ClientService(repository.Object);

            var result = await service.GetAllAsync();

            Assert.NotNull(result);
            repository.Verify();
        }

        [Fact]
        public async Task InsertAsync_Succeeds()
        {
            var client = CreateTestClient();
            Mock<IRepository<Models.Clients.Client>> repository = new Mock<IRepository<Models.Clients.Client>>();
            repository.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<Models.Clients.Client, bool>>>()))
                .ReturnsAsync((Models.Clients.Client)null).Verifiable();
            repository.Setup(x => x.InsertAsync(It.Is<Models.Clients.Client>(c => c.Id.Equals(client.Id))))
                .ReturnsAsync(client).Verifiable();
            var service = new ClientService(repository.Object);

            var result = await service.InsertAsync(client);

            Assert.NotNull(result);
            repository.Verify();
        }

        [Fact]
        public async Task InsertAsync_IsDuplicated_ThrowsException()
        {
            var client = CreateTestClient();
            var client2 = CreateDuplicatedTestClient();
            Mock<IRepository<Models.Clients.Client>> repository = new Mock<IRepository<Models.Clients.Client>>();
            repository.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<Models.Clients.Client, bool>>>()))
                .ReturnsAsync(client2).Verifiable();
            //repository.Setup(x => x.InsertAsync(It.Is<Models.Clients.Client>(c => c.Id.Equals(client.Id))))
            //    .ReturnsAsync(client);
            var service = new ClientService(repository.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await service.InsertAsync(client)
            );
            Assert.NotNull(exception);
            repository.Verify();
        }

        [Fact]
        public async Task UpdateAsync_Succeeds()
        {
            var client = CreateTestClient();
            Mock<IRepository<Models.Clients.Client>> repository = new Mock<IRepository<Models.Clients.Client>>();
            repository.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<Models.Clients.Client, bool>>>()))
                .ReturnsAsync((Models.Clients.Client)null).Verifiable();
            repository.Setup(x => x.UpdateAsync(It.Is<Models.Clients.Client>(c => c.Id.Equals(client.Id))))
                .Returns(Task.CompletedTask).Verifiable();
            var service = new ClientService(repository.Object);

            await service.UpdateAsync(client);

            repository.Verify();
        }

        [Fact]
        public async Task UpdateAsync_IsDuplicated_ThrowsException()
        {
            var client = CreateTestClient();
            var client2 = CreateDuplicatedTestClient();
            Mock<IRepository<Models.Clients.Client>> repository = new Mock<IRepository<Models.Clients.Client>>();
            repository.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<Models.Clients.Client, bool>>>()))
                .ReturnsAsync(client2).Verifiable();
            //repository.Setup(x => x.UpdateAsync(It.Is<Models.Clients.Client>(c => c.Id.Equals(client.Id))))
            //    .Returns(Task.CompletedTask).Verifiable();
            var service = new ClientService(repository.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await service.UpdateAsync(client)
            );
            
            Assert.NotNull(exception);
            repository.Verify();
        }

        [Fact]
        public async Task DeleteByIdAsync_Succeeds()
        {
            var client = CreateTestClient();
            Mock<IRepository<Models.Clients.Client>> repository = new Mock<IRepository<Models.Clients.Client>>();
            repository.Setup(x => x.DeleteByIdAsync(It.Is<string>(x => x.Equals(client.Id))))
                .Returns(Task.CompletedTask).Verifiable();
            var service = new ClientService(repository.Object);

            await service.DeleteByIdAsync(client.Id);

            repository.Verify();
        }

        private Models.Clients.Client CreateTestClient()
        {
            return new Models.Clients.Client
            {
                Id = "abcd",
                Name = "Company A",
                Address = "123 Main St",
                Phone = "555 123123"
            };
        }

        private Models.Clients.Client CreateDuplicatedTestClient()
        {
            return new Models.Clients.Client
            {
                Id = "fgjh",
                Name = "Company A",
                Address = "12 Main St",
                Phone = "555 222123"
            };
        }
    }
}
