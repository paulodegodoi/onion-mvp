using Microsoft.EntityFrameworkCore;
using Onion.Domain.Interfaces;
using Onion.Infrastructure.Context;

namespace Onion.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public Task<T> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<T> CreateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<T> UpdateAsync(int id, T entity)
    {
        throw new NotImplementedException();
    }

    public Task<T> RemoveAsync(T entity)
    {
        throw new NotImplementedException();
    }
}