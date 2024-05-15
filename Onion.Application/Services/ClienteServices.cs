using AutoMapper;
using Onion.Application.DTOs;
using Onion.Application.Interfaces;
using Onion.Domain.Interfaces;

namespace Onion.Application.Services;

public class ClienteServices : IBaseServices<ClienteDTO>
{
    private readonly IBaseRepository<ClienteDTO> _clienteRepository;
    private readonly IMapper _mapper;

    public ClienteServices(IBaseRepository<ClienteDTO> clienteRepository, IMapper mapper)
    {
        _clienteRepository = clienteRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClienteDTO>> GetAllAsync()
    {
        var clientesEntity = await _clienteRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ClienteDTO>>(clientesEntity);
    }

    public async Task<ClienteDTO> GetById(int id)
    {
        var clienteEntity = await _clienteRepository.GetById(id);
        return _mapper.Map<ClienteDTO>(clienteEntity);
    }

    public async Task<ClienteDTO> CreateAsync(ClienteDTO entity)
    {
        var clienteEntity = await _clienteRepository.CreateAsync(entity);
        return _mapper.Map<ClienteDTO>(clienteEntity);
    }

    public async Task<ClienteDTO> UpdateAsync(int id, ClienteDTO entity)
    {
        if (id.ToString() != entity.Documento)
            throw new ArgumentException("O id informado não pertence ao cliente.");

        var clienteEntity = await _clienteRepository.GetById(id);

        if (clienteEntity is null)
            throw new NullReferenceException($"Cliente não encontrado. Id: {id}.");
        
        var clienteUpdatedEntity = await _clienteRepository.UpdateAsync(clienteEntity);
        return _mapper.Map<ClienteDTO>(clienteUpdatedEntity);
    }
    
    public async Task RemoveAsync(int id)
    {
        var clienteEntity = await _clienteRepository.GetById(id);

        if (clienteEntity is null)
            throw new NullReferenceException();
        
        await _clienteRepository.RemoveAsync(clienteEntity);
    }
}