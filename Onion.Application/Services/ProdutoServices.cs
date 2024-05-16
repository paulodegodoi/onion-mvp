using AutoMapper;
using Onion.Application.DTOs;
using Onion.Application.Interfaces;
using Onion.Domain.Entities;
using Onion.Domain.Interfaces;

namespace Onion.Application.Services;

public class ProdutoServices : BaseServices<Produto>
{
    public ProdutoServices(IBaseRepository<Produto> repository) : base(repository) {}
    
    // private readonly IBaseRepository<Produto> _produtoRepository;
    // private readonly IMapper _mapper;

    // public ProdutoServices(IBaseRepository<Produto> produtoRepository, IMapper mapper)
    // {
    //     _produtoRepository = produtoRepository;
    //     _mapper = mapper;
    // }

    // public async Task<IEnumerable<ProdutoDTO>> GetAllAsync()
    // {
    //     var produtoEntity = await _produtoRepository.GetAllAsync();
    //     return _mapper.Map<IEnumerable<ProdutoDTO>>(produtoEntity);
    // }
    //
    // public async Task<ProdutoDTO> GetById(int id)
    // {
    //     var produtoEntity = await _produtoRepository.GetById(id);
    //     return _mapper.Map<ProdutoDTO>(produtoEntity);
    // }
    //
    // public async Task<ProdutoDTO> CreateAsync(ProdutoDTO entity)
    // {
    //     try
    //     {
    //         var pedidoEntity = _mapper.Map<Produto>(entity);
    //         var produtoEntity = await _produtoRepository.CreateAsync(pedidoEntity);
    //         return _mapper.Map<ProdutoDTO>(produtoEntity);
    //     }
    //     catch (Exception ex)
    //     {
    //         throw new Exception($"Não foi possível criar o produto. Erro: {ex.Message}");
    //     }
    //     
    // }
    //
    // public async Task<ProdutoDTO> UpdateAsync(int id, ProdutoDTO entity)
    // {
    //     if (id != entity.ProdutoId)
    //         throw new ArgumentException("O id informado não pertence ao produto.");
    //
    //     var produtoEntity = await _produtoRepository.GetById(id);
    //
    //     if (produtoEntity is null)
    //         throw new NullReferenceException($"Produto não encontrado. Id: {id}.");
    //     
    //     var produtoToUpdate = _mapper.Map<Produto>(entity);
    //     var pedidoUpdated = await _produtoRepository.UpdateAsync(id, produtoToUpdate);
    //     return _mapper.Map<ProdutoDTO>(pedidoUpdated);
    // }
    //
    // public async Task RemoveAsync(int id)
    // {
    //     var produtoEntity = await _produtoRepository.GetById(id);
    //
    //     if (produtoEntity is null)
    //         throw new NullReferenceException($"Produto não encontrado. Id: {id}.");
    //     
    //     await _produtoRepository.RemoveAsync(produtoEntity);
    // }
}