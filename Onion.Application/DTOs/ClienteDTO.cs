using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.RegularExpressions;

namespace Onion.Application.DTOs;

public class ClienteDTO
{
    public ClienteDTO(string documento, string razaoSocial)
    {
        if (string.IsNullOrEmpty(razaoSocial))
        {
            throw new NoNullAllowedException("É preciso informar a razão social");
        }
        Documento = documento;
        RazaoSocial = razaoSocial;
    }
    public int Id { get; set; }
    /// <summary>
    /// CNPJ ou CPF
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="NoNullAllowedException"></exception>
    [Required(ErrorMessage = "O documento é obrigatório")]
    public string Documento
    {
        get => _documento;
        set
        {
            if (value != null)
            {
                string document = value;
                document = new string(document.Where(char.IsDigit).ToArray());
                
                // Verifica se o documento contém apenas números e tem 11 ou 14 caracteres
                if (!Regex.IsMatch(document, @"^\d{11}$|^\d{14}$"))
                {
                    throw new InvalidDocumentException($"O documento: {document} deve conter 11 ou 14 números.");
                }

                _documento = value;
            }
            else
                throw new InvalidDocumentException("É necessário informar um documento");
        }
    }
    private string _documento { get; set; }
    
    /// <summary>
    /// Nome da empresa ou do cliente
    /// </summary>
    [Required(ErrorMessage = "A razão social é obrigatória")]
    [StringLength(150, ErrorMessage = "Reduza o tamanho do nome")]
    public string RazaoSocial { get; set; } = string.Empty;
    
    public class InvalidDocumentException : Exception
    {
        public InvalidDocumentException(string message) : base(message)
        {
        }
    }
}