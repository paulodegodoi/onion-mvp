using AutoMapper;
using Onion.Application.DTOs;
using Onion.Application.Interfaces;
using Onion.Domain.Interfaces;

namespace Onion.Application.Services;

public class PedidoServices : IBaseServices<PedidoDTO>
{
    private readonly IBaseRepository<PedidoDTO> _pedidoRepository;
    private readonly IMapper _mapper;

    public PedidoServices(IBaseRepository<PedidoDTO> pedidoRepository, IMapper mapper)
    {
        _pedidoRepository = pedidoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PedidoDTO>> GetAllAsync()
    {
        var pedidosEntity = await _pedidoRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PedidoDTO>>(pedidosEntity);
    }

    public async Task<PedidoDTO> GetById(int id)
    {
        var pedidoEntity = await _pedidoRepository.GetById(id);
        return _mapper.Map<PedidoDTO>(pedidoEntity);
    }

    public async Task<PedidoDTO> CreateAsync(PedidoDTO entity)
    {
        var pedidoEntity = await _pedidoRepository.CreateAsync(entity);
        return _mapper.Map<PedidoDTO>(pedidoEntity);
    }

    public async Task<PedidoDTO> UpdateAsync(int id, PedidoDTO entity)
    {
        if (id != entity.PedidoId)
            throw new ArgumentException("O id informado não pertence ao pedido.");

        var pedidoEntity = await _pedidoRepository.GetById(id);

        if (pedidoEntity is null)
            throw new NullReferenceException($"Pedido não encontrado. Id: {id}.");
        
        var pedidoUpdatedEntity = await _pedidoRepository.UpdateAsync(pedidoEntity);
        return _mapper.Map<PedidoDTO>(pedidoUpdatedEntity);
    }

    public async Task RemoveAsync(int id)
    {
        var pedidoEntity = await _pedidoRepository.GetById(id);

        if (pedidoEntity is null)
            throw new NullReferenceException();
        
        await _pedidoRepository.RemoveAsync(pedidoEntity);
    }
}