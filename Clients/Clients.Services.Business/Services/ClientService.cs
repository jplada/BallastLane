using Clients.Data.Repositories;
using Clients.Services.Business.Models.Clients;
using Clients.Services.Business.Services.Interfaces;
using System.Linq.Expressions;

namespace Clients.Services.Business.Services
{
    public class ClientService: IClientService
    {
        private readonly IRepository<Client> repository;

        public ClientService(IRepository<Client> repository)
        {
            this.repository = repository;
        }

        public async Task<Client> FindByIdAsync(string id)
        {
            return await repository.FindByIdAsync(id);
        }

        public async Task<Client> FindOneAsync(Expression<Func<Client, bool>> filterExpression)
        {
            return await repository.FindOneAsync(filterExpression);
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<Client> InsertAsync(Client document)
        {
            var duplicated = await this.FindOneAsync(c => c.Name.ToUpper() == document.Name.ToUpper());
            if(duplicated is not null)
            {
                throw new ArgumentException($"Client with name {document.Name} already exists");
            }
            return await repository.InsertAsync(document);
        }

        public async Task UpdateAsync(Client document)
        {
            var duplicated = await this.FindOneAsync(c => c.Id != document.Id && c.Name.ToUpper() == document.Name.ToUpper());
            if (duplicated is not null)
            {
                throw new ArgumentException($"Other Client with name {document.Name} already exists");
            }
            await repository.UpdateAsync(document);
        }

        public async Task DeleteByIdAsync(string id)
        {
            await repository.DeleteByIdAsync(id);
        }
    }
}
