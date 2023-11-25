using Clients.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Clients.Data.Repositories
{
    public class MongoRepository<TDocument> : IRepository<TDocument> where TDocument : Document
    {
        private readonly IMongoCollection<TDocument> _collection;

        public MongoRepository(IOptions<MongoDbSettings>  settings, string collectionName)
        {
            var database = new MongoClient(settings.Value.ConnectionString).GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<TDocument>(collectionName);
        }


        public async Task<IEnumerable<TDocument>> GetAllAsync()
        {
            return await (await _collection.FindAsync(_ => true)).ToListAsync();
        }

        public async Task<TDocument> FindByIdAsync(string id)
        {
            return (await _collection.FindAsync<TDocument>(doc => doc.Id.Equals(id))).FirstOrDefault();
        }

        public async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return (await _collection.FindAsync<TDocument>(filterExpression)).FirstOrDefault();
        }

        public async Task<TDocument> InsertAsync(TDocument document)
        {
            await _collection.InsertOneAsync(document);
            return document;
        }

        public async Task UpdateAsync(TDocument document)
        {
            await _collection.ReplaceOneAsync(rec => rec.Id == document.Id, document);
        }

        public async Task DeleteByIdAsync(string id)
        {
            await _collection.FindOneAndDeleteAsync(rec => rec.Id == id);
        }
    }
}
