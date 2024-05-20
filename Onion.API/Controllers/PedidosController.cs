using Microsoft.AspNetCore.Mvc;
using Onion.Application.DTOs;
using Onion.Application.Interfaces;
using Onion.Domain.Entities;

namespace Onion.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : ControllerBase
{
    private readonly IBaseServices<PedidoDTO, Pedido> _pedidoServices;

    public PedidosController(IBaseServices<PedidoDTO, Pedido> produtoServices)
    {
        _pedidoServices = produtoServices;
    }

    // /pedidos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoDTO>>> GetAll()
    {
        var pedidos = await _pedidoServices.GetAllAsync();
        return Ok(pedidos);
    }
    
    // /pedidos/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PedidoDTO>> Get(int id)
    {
        var pedido = await _pedidoServices.GetById(id);

        if (pedido is null)
            return NotFound($"Pedido com id: {id} não encontrado.");

        return Ok(pedido);
    }
}