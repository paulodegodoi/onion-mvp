namespace Onion.Domain.Entities;

public sealed class Produto
{
    public int ProdutoId { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public decimal Valor { get; private set; }
}