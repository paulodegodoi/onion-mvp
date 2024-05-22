using AutoMapper;
using Onion.Application.Interfaces;
using Onion.Domain.Entities;
using Onion.Domain.Interfaces;

namespace Onion.Application.Services;

public class BaseServices<T, TEntity> : IBaseServices<T, TEntity> where TEntity : Entity
{
    private readonly IBaseRepository<TEntity> _baseRepository;
    private readonly IMapper _mapper;

    public BaseServices(IBaseRepository<TEntity> baseRepository, IMapper mapper)
    {
        _baseRepository = baseRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var entities = await _baseRepository.GetAllAsync();
        return _mapper.Map<List<T>>(entities);
    }

    public async Task<T> GetById(int id)
    {
        var entity = await _baseRepository.GetById(id);
        return _mapper.Map<T>(entity);
    }

    public async Task<T> CreateAsync(T dtoObject)
    {
        var entity = _mapper.Map<TEntity>(dtoObject);
        var entityCreated = await _baseRepository.CreateAsync(entity);
        return _mapper.Map<T>(entityCreated);
    }

    public async Task<T> UpdateAsync(int id, T dtoObject)
    {
        var entity = _mapper.Map<TEntity>(dtoObject);
        var entityUpdated = await _baseRepository.UpdateAsync(id, entity);
        
        return _mapper.Map<T>(entityUpdated);
    }

    public async Task RemoveAsync(int id)
    {
        var entity = await _baseRepository.GetById(id);

        if (entity is null)
            throw new NullReferenceException($"Entidade não encontrada. Id {id}");
        
        await _baseRepository.RemoveAsync(id);
    }
}