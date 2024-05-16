namespace Onion.Domain.Entities;

public sealed class Produto : Entity
{
    public Produto() {}
    public Produto(int id, string nome, decimal valor)
    {
        Id = id;
        Nome = nome;
        Valor = valor;
    }
    // public int ProdutoId { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public decimal Valor { get; private set; }
}