using Onion.Domain.Entities;

namespace Onion.Application.Interfaces;

public interface IClienteServices : IBaseServices<Cliente>
{
    Task<Cliente> GetClienteByDocument(string document);
}