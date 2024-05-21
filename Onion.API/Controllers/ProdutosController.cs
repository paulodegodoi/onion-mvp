using Microsoft.AspNetCore.Mvc;
using Onion.Application.DTOs;
using Onion.Application.Interfaces;
using Onion.Domain.Entities;

namespace Onion.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IBaseServices<ProdutoDTO, Produto> _baseServices;
    private readonly IProdutoServices _produtoServices;

    public ProdutosController(IBaseServices<ProdutoDTO, Produto> produtoServices, IProdutoServices produtoServices1)
    {
        _baseServices = produtoServices;
        _produtoServices = produtoServices1;
    }

    // /produtos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetAll()
    {
        var produtos = await _baseServices.GetAllAsync();
        return Ok(produtos.OrderBy(p => p.Valor));
    }
    
    // /produtos/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProdutoDTO>> GetProdutoById(int id)
    {
        var produto = await _baseServices.GetById(id);

        if (produto is null)
            return NotFound($"Produto com id: {id} não encontrado.");

        return Ok(produto);
    }
    
    // /produtos
    [HttpPost]
    public async Task<ActionResult<ProdutoDTO>> Post(ProdutoDTO produtoDTO)
    {
        try
        {
            produtoDTO.Nome = produtoDTO.Nome.ToUpper();
            // verifica se já existe um produto com o mesmo nome
            var isProdutoExists = await _produtoServices.GetProdutoByName(produtoDTO.Nome) != null;
            if (isProdutoExists)
                return BadRequest($"Já existe um produto com o nome: {produtoDTO.Nome}");
            
            var produto = await _baseServices.CreateAsync(produtoDTO);
        
            return CreatedAtAction(nameof(GetProdutoById), new { id = produto.Id }, produto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Houve um erro interno");
        }
        
    }
}