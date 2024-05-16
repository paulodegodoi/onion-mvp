using System.ComponentModel.DataAnnotations;
using Onion.Domain.Entities;

namespace Onion.Application.DTOs;

public class PedidoDTO
{
    public int PedidoId { get; set; }
    [Required]
    public int Numero { get; set; }
    [Required]
    [StringLength(7, ErrorMessage = "Informe um cep válido.")]
    public string CEP { get; set; } = string.Empty;
    [Required]
    public string ClienteDocumento { get; set; } = string.Empty;
    public Cliente Cliente { get; set; } = new();
    public Produto Produto { get; set; } = new();
    [Required]
    public DateTime DataCriacao { get; set; }
}