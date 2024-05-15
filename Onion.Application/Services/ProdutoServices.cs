using AutoMapper;
using Onion.Application.DTOs;
using Onion.Application.Interfaces;
using Onion.Domain.Entities;
using Onion.Domain.Interfaces;

namespace Onion.Application.Services;

public class ProdutoServices : IBaseServices<ProdutoDTO>
{
    private readonly IBaseRepository<ProdutoDTO> _produtoRepository;
    private readonly IMapper _mapper;

    public ProdutoServices(IBaseRepository<ProdutoDTO> produtoRepository, IMapper mapper)
    {
        _produtoRepository = produtoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProdutoDTO>> GetAllAsync()
    {
        var produtoEntity = await _produtoRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProdutoDTO>>(produtoEntity);
    }

    public async Task<ProdutoDTO> GetById(int id)
    {
        var produtoEntity = await _produtoRepository.GetById(id);
        return _mapper.Map<ProdutoDTO>(produtoEntity);
    }

    public async Task<ProdutoDTO> CreateAsync(ProdutoDTO entity)
    {
        var produtoEntity = await _produtoRepository.CreateAsync(entity);
        return _mapper.Map<ProdutoDTO>(produtoEntity);
    }

    public async Task<ProdutoDTO> UpdateAsync(int id, ProdutoDTO entity)
    {
        if (id != entity.ProdutoId)
            throw new ArgumentException("O id informado não pertence ao produto.");

        var produtoEntity = await _produtoRepository.GetById(id);

        if (produtoEntity is null)
            throw new NullReferenceException($"Produto não encontrado. Id: {id}.");
        
        var pedidoUpdatedEntity = await _produtoRepository.UpdateAsync(produtoEntity);
        return _mapper.Map<ProdutoDTO>(pedidoUpdatedEntity);
    }

    public async Task RemoveAsync(int id)
    {
        var produtoEntity = await _produtoRepository.GetById(id);

        if (produtoEntity is null)
            throw new NullReferenceException();
        
        await _produtoRepository.RemoveAsync(produtoEntity);
    }
}