using Clients.Data.Models;
using Clients.Data.Repositories;
using Microsoft.Extensions.Options;

namespace Clients.Data.Testing
{
    public class MongoRepositoryTest
    {
        [Fact]
        public async Task CreateEntity_ReturnsGeneratedId()
        {
            MyEntity document = new MyEntity
            {
                Name = "Joe",
            };
            MongoDbSettings settings = new MongoDbSettings
            {
                ConnectionString = "mongodb://mongo-user:mongo-pwd@localhost:27018/",
                DatabaseName = "test"
            };
            var options = Options.Create(settings);
            var repo = new MongoRepository<MyEntity>(options, "TestEntities");

            var inserted = await repo.InsertAsync(document);

            Assert.NotNull(inserted.Id);

            await repo.DeleteByIdAsync(inserted.Id);
        }

        [Fact]
        public async Task FindEntity_Succeeds()
        {
            MyEntity document = new MyEntity
            {
                Name = "Joe",
            };
            var settings = GetMongoSettings();
            var options = Options.Create(settings);
            var repo = new MongoRepository<MyEntity>(options, "TestEntities");

            var inserted = await repo.InsertAsync(document);

            var readed = await repo.FindByIdAsync(inserted.Id);

            Assert.NotNull(readed);

            await repo.DeleteByIdAsync(inserted.Id);
        }

        [Fact]
        public async Task Update_Succeeds()
        {
            MyEntity document = new MyEntity
            {
                Name = "Joe",
            };
            var settings = GetMongoSettings();
            var options = Options.Create(settings);
            var repo = new MongoRepository<MyEntity>(options, "TestEntities");

            var inserted = await repo.InsertAsync(document);
            inserted.Name = "Joe Jones";
            await repo.UpdateAsync(inserted);

            var readed = await repo.FindByIdAsync(inserted.Id);

            Assert.NotNull(readed);
            Assert.Equal(inserted.Name, readed.Name);

            await repo.DeleteByIdAsync(inserted.Id);
        }

        [Fact]
        public async Task Delete_Succeeds()
        {
            MyEntity document = new MyEntity
            {
                Name = "Joe",
            };
            var settings = GetMongoSettings();
            var options = Options.Create(settings);
            var repo = new MongoRepository<MyEntity>(options, "TestEntities");

            var inserted = await repo.InsertAsync(document);
            await repo.DeleteByIdAsync(inserted.Id);

            var readed = await repo.FindByIdAsync(inserted.Id);

            Assert.Null(readed);
        }

        private MongoDbSettings GetMongoSettings()
        {
            return new MongoDbSettings
            {
                ConnectionString = "mongodb://mongo-user:mongo-pwd@localhost:27018/",
                DatabaseName = "test"
            };
        }
    }



    internal class MyEntity: Models.Document
    {
        public string Name { get; set; }
    }
}