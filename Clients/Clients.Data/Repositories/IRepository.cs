using Clients.Data.Models;
using System.Linq.Expressions;

namespace Clients.Data.Repositories
{
    public interface IRepository<TDocument> where TDocument : Document
    {
        Task<IEnumerable<TDocument>> GetAllAsync();
        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);
        Task<TDocument> FindByIdAsync(string id);
        Task<TDocument> InsertAsync(TDocument document);
        Task UpdateAsync(TDocument document);
        Task DeleteByIdAsync(string id);
    }
}
