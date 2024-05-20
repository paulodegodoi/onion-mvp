using System.Text.Json.Serialization;

namespace Onion.Application.Models;

public class Endereco
{
    [JsonPropertyName("cep")] public string Cep { get; set; } = string.Empty;

    [JsonPropertyName("uf")] public string UF { get; set; } = string.Empty;
}