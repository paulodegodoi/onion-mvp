namespace Onion.Domain.Entities;

public sealed class Pedido : Entity
{
    public int Numero { get; private set; }
    public string CEP { get; private set; } = string.Empty;
    public int ClienteId { get; private set; }
    public Cliente Cliente { get; private set; } = new();
    public Produto Produto { get; private set; } = new();
    public DateTime DataCriacao { get; private set; }
}