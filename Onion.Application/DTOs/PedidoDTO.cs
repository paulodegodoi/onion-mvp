using Onion.Domain.Entities;

namespace Onion.Application.DTOs;

public class PedidoDTO
{
    public int PedidoId { get; private set; }
    public int Numero { get; private set; }
    public string CEP { get; private set; } = string.Empty;
    public Cliente Cliente { get; private set; } = new();
    public Produto Produto { get; private set; } = new();
    public DateTime DataCriacao { get; private set; }
}