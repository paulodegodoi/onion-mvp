using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Onion.Application.DTOs;

public class ProdutoDTO
{
    public int Id { get; private set; }
    
    [Required(ErrorMessage = "O nome do produto é obrigatório.")]
    [MaxLength(80)]
    public string Nome { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Informe o preço do produto.")]
    [Column(TypeName = "decimal(18,2)")]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal Valor { get; set; }
}