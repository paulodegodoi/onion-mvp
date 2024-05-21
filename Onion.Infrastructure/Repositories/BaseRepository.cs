using Microsoft.EntityFrameworkCore;
using Onion.Domain.Entities;
using Onion.Domain.Interfaces;
using Onion.Infrastructure.Context;

namespace Onion.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : Entity
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

    public async Task<T> GetById(int id)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<T> CreateAsync(T entity)
    {
        try
        {
            var t = await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return t.Entity;
        }
        catch (Exception ex)
        {
            throw new Exception("Falha ao adicionar a entidade");
        }
    }

    public async Task<T> UpdateAsync(int id, T entity)
    {
        if (id != entity.Id)
            throw new Exception("Id informado é diferente do id da entidade");

        var t = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (t is null)
            throw new NullReferenceException($"Entidade com id {id} não foi encontrada");
        
        _context.Entry(entity).State = EntityState.Modified;
        
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task RemoveAsync(int id)
    {
        var t = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        if (t is null)
            throw new NullReferenceException("entidade não encontrada");
        
        _context.Set<T>().Remove(t);
        await _context.SaveChangesAsync();
    }
}