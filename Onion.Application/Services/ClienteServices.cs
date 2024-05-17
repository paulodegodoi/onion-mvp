using Onion.Application.Interfaces;
using Onion.Domain.Interfaces;
using Onion.Domain.Entities;

namespace Onion.Application.Services;

public class ClienteServices : IClienteServices
{
    private readonly IBaseServices<Cliente> _baseServices;
    private readonly IClienteRepository _clienteRepository;

    public ClienteServices(IBaseServices<Cliente> baseServices, IClienteRepository clienteRepository)
    {
        _baseServices = baseServices;
        _clienteRepository = clienteRepository;
    }

    public Task<Cliente> GetClienteByDocument(string document)
    {
        return _clienteRepository.GetClienteByDocument(document);
    }

    public Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return _baseServices.GetAllAsync();
    }

    public Task<Cliente> GetById(int id)
    {
        return _baseServices.GetById(id);
    }

    public Task<Cliente> CreateAsync(Cliente entity)
    {
        return _baseServices.CreateAsync(entity);
    }

    public Task<Cliente> UpdateAsync(int id, Cliente entity)
    {
        return _baseServices.UpdateAsync(id, entity);
    }

    public Task RemoveAsync(int id)
    {
        return _baseServices.RemoveAsync(id);
    }
}