using System.ComponentModel.DataAnnotations;
using Onion.Domain.Entities;

namespace Onion.Application.DTOs;

public class PedidoDTO
{
    public int Id { get; set; }
    /// <summary>
    /// Indica o número de registro do pedido
    /// </summary>
    [Required]
    public int Numero { get; set; }
    [Required]
    [StringLength(8, ErrorMessage = "Informe um cep válido.")]
    public string Cep
    {
        get => _cep;
        set
        {
            if (value != null)
            {
                var cep = value;
                cep = new string(cep.Where(char.IsDigit).ToArray());

                if (cep.Length != 8)
                    throw new ArgumentOutOfRangeException($"O cep {cep} não é válido");

                _cep = cep;
            }
        }
    }
    private string _cep { get; set; }
    public ClienteDTO Cliente
    {
        get;
        set;
    }
    public ProdutoDTO Produto { get; set; } = new();
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