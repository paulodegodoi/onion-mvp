using System.Text.Json.Serialization;

namespace Onion.Application.Models;

public class Endereco
{
    [JsonPropertyName("cep")]
    public string Cep { get; set; }
    [JsonPropertyName("uf")]
    public string UF { get; set; }
}