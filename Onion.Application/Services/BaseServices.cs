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

    public Task RemoveAsync(int id)
    {
        throw new NotImplementedException();
    }
}