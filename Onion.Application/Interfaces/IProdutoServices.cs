using Onion.Application.DTOs;
using Onion.Domain.Entities;

namespace Onion.Application.Interfaces;

public interface IProdutoServices
{
    Task<ProdutoDTO> GetProdutoByName(string name);
}