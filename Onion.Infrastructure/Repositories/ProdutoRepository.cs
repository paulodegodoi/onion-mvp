using Microsoft.EntityFrameworkCore;
using Onion.Domain.Entities;
using Onion.Domain.Interfaces;
using Onion.Infrastructure.Context;

namespace Onion.Infrastructure.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly IBaseRepository<Produto> _baseRepository;
    private readonly AppDbContext _context;

    public ProdutoRepository(IBaseRepository<Produto> baseRepository, AppDbContext context)
    {
        _baseRepository = baseRepository;
        _context = context;
    }

    public async Task<Produto> GetProdutoByName(string name)
    {
        return await _context.Produtos.FirstOrDefaultAsync(p => p.Nome == name);
    }
    public Task<IEnumerable<Produto>> GetAllAsync()
    {
        return _baseRepository.GetAllAsync();
    }

    public Task<Produto> GetById(int id)
    {
        return _baseRepository.GetById(id);
    }

    public Task<Produto> CreateAsync(Produto entity)
    {
        return _baseRepository.CreateAsync(entity);
    }

    public Task<Produto> UpdateAsync(int id, Produto entity)
    {
        return _baseRepository.UpdateAsync(id, entity);
    }

    public Task<Produto> RemoveAsync(Produto entity)
    {
        return _baseRepository.RemoveAsync(entity);
    }
}