using AutoMapper;
using Onion.Application.DTOs;
using Onion.Domain.Entities;

namespace Onion.Application.Mappings;

public class DomainToDTOMappingProfile : Profile
{
    public DomainToDTOMappingProfile()
    {
        CreateMap<Cliente, ClienteDTO>().ReverseMap();
        CreateMap<Pedido, PedidoDTO>().ReverseMap();
        CreateMap<Produto, ProdutoDTO>().ReverseMap();
    }
}