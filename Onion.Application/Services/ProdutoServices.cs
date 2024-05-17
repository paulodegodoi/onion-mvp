using Onion.Application.Interfaces;
using Onion.Domain.Entities;
using Onion.Domain.Interfaces;

namespace Onion.Application.Services;

public class ProdutoServices : IProdutoServices
{
    private readonly IBaseServices<Produto> _baseServices;
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoServices(IBaseServices<Produto> baseServices, IProdutoRepository produtoRepository)
    {
        _baseServices = baseServices;
        _produtoRepository = produtoRepository;
    }

    public Task<Produto> GetProdutoByName(string name)
    {
        return _produtoRepository.GetProdutoByName(name);
    }
    public Task<IEnumerable<Produto>> GetAllAsync()
    {
        return _baseServices.GetAllAsync();
    }

    public Task<Produto> GetById(int id)
    {
        return _baseServices.GetById(id);
    }

    public Task<Produto> CreateAsync(Produto entity)
    {
        return _baseServices.CreateAsync(entity);
    }

    public Task<Produto> UpdateAsync(int id, Produto entity)
    {
        return _baseServices.UpdateAsync(id, entity);
    }

    public Task RemoveAsync(int id)
    {
        return _baseServices.RemoveAsync(id);
    }
}