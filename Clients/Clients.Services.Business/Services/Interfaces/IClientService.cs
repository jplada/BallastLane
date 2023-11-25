using Clients.Services.Business.Models.Clients;
using System.Linq.Expressions;

namespace Clients.Services.Business.Services.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client> FindOneAsync(Expression<Func<Client, bool>> filterExpression);
        Task<Client> FindByIdAsync(string id);
        Task<Client> InsertAsync(Client document);
        Task UpdateAsync(Client document);
        Task DeleteByIdAsync(string id);
    }
}
