using AutoMapper;
using Onion.Application.DTOs;
using Onion.Application.Interfaces;
using Onion.Domain.Entities;
using Onion.Domain.Interfaces;

namespace Onion.Application.Services;

public class PedidoServices : IBaseServices<PedidoDTO>
{
    private readonly IBaseRepository<Pedido> _pedidoRepository;
    private readonly IMapper _mapper;

    public PedidoServices(IBaseRepository<Pedido> pedidoRepository, IMapper mapper)
    {
        _pedidoRepository = pedidoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PedidoDTO>> GetAllAsync()
    {
        var pedidos = await _pedidoRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PedidoDTO>>(pedidos);
    }

    public async Task<PedidoDTO> GetById(int id)
    {
        var pedidoEntity = await _pedidoRepository.GetById(id);
        return _mapper.Map<PedidoDTO>(pedidoEntity);
    }

    public async Task<PedidoDTO> CreateAsync(PedidoDTO entity)
    {
        try
        {
            var pedidoEntity = _mapper.Map<Pedido>(entity);
            var pedido = await _pedidoRepository.CreateAsync(pedidoEntity);
            return _mapper.Map<PedidoDTO>(pedido);
        }
        catch (Exception ex)
        {
            throw new Exception($"Não foi possível criar o pedido. Erro: {ex.Message}");
        }
    }

    public async Task<PedidoDTO> UpdateAsync(int id, PedidoDTO entity)
    {
        if (id != entity.PedidoId)
            throw new ArgumentException("O id informado não pertence ao pedido.");

        var pedidoEntity = await _pedidoRepository.GetById(id);

        if (pedidoEntity is null)
            throw new NullReferenceException($"Pedido não encontrado. Id: {id}.");
        
        var pedidoToUpdate = _mapper.Map<Pedido>(entity);
        var pedidoUpdatedEntity = await _pedidoRepository.UpdateAsync(id, pedidoToUpdate);
        return _mapper.Map<PedidoDTO>(pedidoUpdatedEntity);
    }

    public async Task RemoveAsync(int id)
    {
        var pedidoEntity = await _pedidoRepository.GetById(id);

        if (pedidoEntity is null)
            throw new NullReferenceException($"Pedido não encontrado. Id: {id}.");
        
        await _pedidoRepository.RemoveAsync(pedidoEntity);
    }
}