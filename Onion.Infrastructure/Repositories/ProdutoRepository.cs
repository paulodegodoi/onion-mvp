using Microsoft.EntityFrameworkCore;
using Onion.Domain.Entities;
using Onion.Domain.Interfaces;
using Onion.Infrastructure.Context;

namespace Onion.Infrastructure.Repositories;

public class ProdutoRepository : IBaseRepository<Produto>
{
    private readonly AppDbContext _db;

    public ProdutoRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Produto>> GetAllAsync()
    {
        return await _db.Produtos.ToListAsync();
    }

    public Task<Produto> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Produto> CreateAsync(Produto entity)
    {
        throw new NotImplementedException();
    }

    public Task<Produto> UpdateAsync(int id, Produto entity)
    {
        throw new NotImplementedException();
    }

    public Task<Produto> RemoveAsync(Produto entity)
    {
        throw new NotImplementedException();
    }
}