using System.ComponentModel.DataAnnotations;

namespace Onion.Application.DTOs;

public class ClienteDTO
{
    [Required(ErrorMessage = "O documento é obrigatório")]
    [MinLength(11, ErrorMessage = "Informe um número de documento válido")]
    [MaxLength(18, ErrorMessage = "Informe um número de documento válido")]
    public string Documento { get; set; } = string.Empty;
    [Required(ErrorMessage = "A razão social é obrigatória")]
    [StringLength(150, ErrorMessage = "Reduza o tamanho do nome")]
    public string RazaoSocial { get; set; } = string.Empty;
}