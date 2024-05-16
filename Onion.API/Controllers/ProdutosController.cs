using Microsoft.AspNetCore.Mvc;
using Onion.Application.DTOs;
using Onion.Application.Interfaces;
using Onion.Domain.Entities;

namespace Onion.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IBaseServices<Produto> _produtoServices;

    public ProdutosController(IBaseServices<Produto> produtoServices)
    {
        _produtoServices = produtoServices;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetAll()
    {
        var produtos = await _produtoServices.GetAllAsync();
        return Ok(produtos);
    }
}