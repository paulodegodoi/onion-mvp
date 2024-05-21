using AutoMapper;
using Onion.Application.DTOs;
using Onion.Application.Interfaces;
using Onion.Domain.Entities;
using Onion.Domain.Interfaces;

namespace Onion.Application.Services;

public class ProdutoServices : BaseServices<ProdutoDTO, Produto>, IProdutoServices
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IMapper _mapper;
    public ProdutoServices(
        IBaseRepository<Produto> baseRepository, 
        IMapper mapper, 
        IProdutoRepository produtoRepository) : base(baseRepository, mapper)
    {
        _produtoRepository = produtoRepository;
        _mapper = mapper;
    }

    public async Task<ProdutoDTO> GetProdutoByName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;
        
        var produto = await _produtoRepository.GetProdutoByName(name.ToUpper());
        return _mapper.Map<ProdutoDTO>(produto);
    }
}