namespace Onion.Domain.Entities;

public sealed class Produto
{
    public Produto() {}
    public Produto(int id, string nome, decimal valor)
    {
        ProdutoId = id;
        Nome = nome;
        Valor = valor;
    }
    public int ProdutoId { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public decimal Valor { get; private set; }
}