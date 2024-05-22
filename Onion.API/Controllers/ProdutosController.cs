using Microsoft.AspNetCore.Mvc;
using Onion.Application.DTOs;
using Onion.Application.Interfaces;

namespace Onion.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoServices _produtoServices;

    public ProdutosController(IProdutoServices produtoServices)
    {
        _produtoServices = produtoServices;
    }

    // /produtos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetAll()
    {
        var produtos = await _produtoServices.GetAllAsync();
        return Ok(produtos.OrderBy(p => p.Valor));
    }
    
    // /produtos/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProdutoDTO>> GetProdutoById(int id)
    {
        var produto = await _produtoServices.GetById(id);

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
            
            var produto = await _produtoServices.CreateAsync(produtoDTO);
        
            return CreatedAtAction(nameof(GetProdutoById), new { id = produto.Id }, produto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Houve um erro interno");
        }
    }
    
    // /produtos/{id}
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProdutoDTO>> Put(int id, ProdutoDTO produto)
    {
        if (id != produto.Id)
            return BadRequest("O id informado é diferente do produto");

        try
        {
            produto.Nome = produto.Nome.ToUpper();
            return await _produtoServices.UpdateAsync(id, produto);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Não foi possível atualizar o produto");
        }
    }
}