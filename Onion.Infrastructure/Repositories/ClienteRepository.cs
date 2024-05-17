using Microsoft.EntityFrameworkCore;
using Onion.Domain.Entities;
using Onion.Domain.Interfaces;
using Onion.Infrastructure.Context;

namespace Onion.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;
    private readonly IBaseRepository<Cliente> _clienteRepository;

    public ClienteRepository(AppDbContext context, IBaseRepository<Cliente> clienteRepository)
    {
        _context = context;
        _clienteRepository = clienteRepository;
        _context.Database.EnsureCreated();
    }

    public async Task<Cliente> GetClienteByDocument(string document)
    {
        if (string.IsNullOrEmpty(document))
            throw new NullReferenceException("Informe um documento.");

        return await _context.Clientes.FirstOrDefaultAsync(c => c.Documento == document);
    }
    public Task<IEnumerable<Cliente>> GetAllAsync()
    {
        return _clienteRepository.GetAllAsync();
    }

    public Task<Cliente> GetById(int id)
    {
        return _clienteRepository.GetById(id);
    }

    public Task<Cliente> CreateAsync(Cliente entity)
    {
        return _clienteRepository.CreateAsync(entity);
    }

    public Task<Cliente> UpdateAsync(int id, Cliente entity)
    {
        return _clienteRepository.UpdateAsync(id, entity);
    }

    public Task<Cliente> RemoveAsync(Cliente entity)
    {
        return _clienteRepository.RemoveAsync(entity);
    }
}