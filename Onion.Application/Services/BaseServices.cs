using Onion.Application.Interfaces;
using Onion.Domain.Interfaces;

namespace Onion.Application.Services;

public class BaseServices<T> : IBaseServices<T>
{
    private readonly IBaseRepository<T> _baseRepository;

    public BaseServices(IBaseRepository<T> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _baseRepository.GetAllAsync();
    }

    public async Task<T> GetById(int id)
    {
        return await _baseRepository.GetById(id);
    }

    public async Task<T> CreateAsync(T entity)
    {
        return await _baseRepository.CreateAsync(entity);
    }

    public async Task<T> UpdateAsync(int id, T entity)
    {
        return await _baseRepository.UpdateAsync(id, entity);
    }

    public async Task RemoveAsync(int id)
    {
        var entity = await _baseRepository.GetById(id);
        await _baseRepository.RemoveAsync(entity);
    }
}