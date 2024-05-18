using System.Text.Json;
using Onion.Application.Models;

namespace Onion.Application.Services;

public class ViaCepServices
{
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly string _defaultUrl = "https://viacep.com.br/ws/{0}/json/";

    public ViaCepServices()
    {
        
    }
    public async Task<Endereco> GetEnderecoByCep(string cep)
    {
        if (string.IsNullOrEmpty(cep))
            throw new NullReferenceException("É obrigatório informar o CEP.");

        var url = string.Format(_defaultUrl, cep);

        try
        {
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode == false)
            {
                throw new Exception($"Falha ao obter os dados. Verifique o cep informado. Cep: {cep}");
            }
            
            var content = await response.Content.ReadAsStringAsync();
            var endereco = JsonSerializer.Deserialize<Endereco>(content);
            return endereco;
        }
        catch (Exception ex)
        {
            throw new Exception($"Houve um erro ao obter os dados do endereco na api. {ex.Message}");
        }
        
    }
}