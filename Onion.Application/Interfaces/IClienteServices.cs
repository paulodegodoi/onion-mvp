using Onion.Application.DTOs;

namespace Onion.Application.Interfaces;

public interface IClienteServices
{
    Task<ClienteDTO> GetClienteByDocument(string document);
}