using AutoMapper;
using Onion.Application.DTOs;
using Onion.Application.Interfaces;
using Onion.Domain.Interfaces;
using Onion.Domain.Entities;

namespace Onion.Application.Services;

public class ClienteServices : BaseServices<ClienteDTO, Cliente>, IClienteServices
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;
    public ClienteServices(
        IBaseRepository<Cliente> baseRepository, 
        IMapper mapper, 
        IClienteRepository clienteRepository) : base(baseRepository, mapper)
    {
        _mapper = mapper;
        _clienteRepository = clienteRepository;
    }

    public async Task<ClienteDTO> GetClienteByDocument(string document)
    {
        var cliente = await _clienteRepository.GetClienteByDocument(document);
        return _mapper.Map<ClienteDTO>(cliente);
    }
}