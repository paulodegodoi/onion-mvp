using Microsoft.EntityFrameworkCore;
using Onion.Domain.Entities;
using Onion.Domain.Interfaces;
using Onion.Infrastructure.Context;

namespace Onion.Infrastructure.Repositories;

public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
{
    private readonly AppDbContext _context;
    private readonly IBaseRepository<Cliente> _clienteRepository;

    public ClienteRepository(
        AppDbContext context, 
        IBaseRepository<Cliente> clienteRepository) : base(context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }

    public async Task<Cliente> GetClienteByDocument(string document)
    {
        if (string.IsNullOrEmpty(document))
            throw new NullReferenceException("Informe um documento.");

        return await _context.Clientes.FirstOrDefaultAsync(c => c.Documento == document);
    }
}