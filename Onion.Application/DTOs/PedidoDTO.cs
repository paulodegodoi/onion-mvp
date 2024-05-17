using System.ComponentModel.DataAnnotations;
using Onion.Domain.Entities;

namespace Onion.Application.DTOs;

public class PedidoDTO
{
    public int Id { get; set; }
    [Required]
    public int Numero { get; set; }
    [Required]
    [StringLength(7, ErrorMessage = "Informe um cep válido.")]
    public string Cep { get; set; } = string.Empty;
    [Required]
    public string ClienteDocumento { get; set; } = string.Empty;

    private ClienteDTO _cliente;
    public ClienteDTO Cliente
    {
        get => _cliente;
        set
        {
            _cliente = value;
            if (value != null && string.IsNullOrEmpty(value.Documento) == false)
            {
                ClienteDocumento = value.Documento;
            }
        }
    }
    public Produto Produto { get; set; } = new();
    [Required]
    public DateTime DataCriacao { get; set; }
    [Required]
    public string UF { get; set; }
    public DateTime DataEntrega { get; set; }
    
    /// <summary>
    /// Indica o valor final após aplicado taxa de entrega (frete)
    /// </summary>
    public decimal ValorFinal { get; set; }
}