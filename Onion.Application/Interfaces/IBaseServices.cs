namespace Onion.Application.Interfaces;

public interface IBaseServices<T, TEntity>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetById(int id);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(int id, T entity);
    Task RemoveAsync(int id);
}