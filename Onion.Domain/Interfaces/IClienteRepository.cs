using Onion.Domain.Entities;

namespace Onion.Domain.Interfaces;

public interface IClienteRepository : IBaseRepository<Cliente>
{
    Task<Cliente> GetClienteByDocument(string document);
}