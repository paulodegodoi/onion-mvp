using Onion.Domain.Entities;

namespace Onion.Domain.Interfaces;

public interface IBaseRepository<T> where T : Entity
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetById(int id);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(int id, T entity);
    Task<T> RemoveAsync(T entity);
}