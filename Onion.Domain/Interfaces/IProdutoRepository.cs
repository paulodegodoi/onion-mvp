using Onion.Domain.Entities;

namespace Onion.Domain.Interfaces;

public interface IProdutoRepository : IBaseRepository<Produto>
{
    Task<Produto> GetProdutoByName(string name);
}