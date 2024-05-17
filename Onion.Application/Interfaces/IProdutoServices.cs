using Onion.Domain.Entities;

namespace Onion.Application.Interfaces;

public interface IProdutoServices : IBaseServices<Produto>
{
    Task<Produto> GetProdutoByName(string name);
}