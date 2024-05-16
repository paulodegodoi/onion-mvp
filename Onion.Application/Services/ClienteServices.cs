using AutoMapper;
using Onion.Application.DTOs;
using Onion.Application.Interfaces;
using Onion.Domain.Entities;
using Onion.Domain.Interfaces;

namespace Onion.Application.Services;

public class ClienteServices : IBaseServices<ClienteDTO>
{
    private readonly IBaseRepository<Cliente> _clienteRepository;
    private readonly IMapper _mapper;

    public ClienteServices(IBaseRepository<Cliente> clienteRepository, IMapper mapper)
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
        var cliente = await _clienteRepository.GetById(id);
        return _mapper.Map<ClienteDTO>(cliente);
    }

    public async Task<ClienteDTO> CreateAsync(ClienteDTO entity)
    {
        try
        {
            var clienteEntity = _mapper.Map<Cliente>(entity);
            var cliente = await _clienteRepository.CreateAsync(clienteEntity);
            return _mapper.Map<ClienteDTO>(cliente);
        }
        catch (Exception ex)
        {
            throw new Exception($"Não foi possível criar o cliente. Erro: {ex.Message}");
        }
    }

    public async Task<ClienteDTO> UpdateAsync(int id, ClienteDTO entity)
    {
        if (id.ToString() != entity.Documento)
            throw new ArgumentException("O id informado não pertence ao cliente.");

        var clienteEntity = await _clienteRepository.GetById(id);

        if (clienteEntity is null)
            throw new NullReferenceException($"Cliente não encontrado. Id: {id}.");

        var clienteToUpdate = _mapper.Map<Cliente>(entity);
        var clienteUpdated = await _clienteRepository.UpdateAsync(id, clienteToUpdate);
        
        return _mapper.Map<ClienteDTO>(clienteUpdated);
    }
    
    public async Task RemoveAsync(int id)
    {
        var clienteEntity = await _clienteRepository.GetById(id);

        if (clienteEntity is null)
            throw new NullReferenceException($"Cliente não encontrado. Id: {id}.");
        
        await _clienteRepository.RemoveAsync(clienteEntity);
    }
}