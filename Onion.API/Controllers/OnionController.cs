using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Onion.Application.DTOs;
using Onion.Application.Interfaces;
using Onion.Application.Services;
using Onion.Domain.Interfaces;

namespace Onion.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OnionController : ControllerBase
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IShippingServices _shippingServices;

    public OnionController(IWebHostEnvironment hostingEnvironment, IShippingServices shippingServices)
    {
        _hostingEnvironment = hostingEnvironment;
        _shippingServices = shippingServices;
    }
    
    /// <summary>
    /// Download da planilha modelo para ser utilizada em /api/carregar-dados
    /// </summary>
    /// <returns></returns>
    [HttpGet("planilha-modelo")]
    public async Task<IActionResult> DownloadModelSheet()
    {
        var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Files", "Planilha-Modelo-Onion.xlsx");

        if (!System.IO.File.Exists(filePath))
            return NotFound("Desculpe, arquivo indiponível");

        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        
        Response.Headers.Add("Content-Disposition", $"attachment; filename=\"Planilha-Modelo-Onion\"");

        return File(fileBytes, "application/octet-stream");
    }
    
    [HttpPost("carregar-dados")]
    public async Task<ActionResult<List<PedidoDTO>>> BuildDetailsForOrder(
        IFormFile file, [FromServices] IClienteServices clienteServices, 
        [FromServices] ViaCepServices viaCepServices, [FromServices] IProdutoServices produtoServices)
    {
        #region Validar arquivo
        if (file == null || file.Length <= 0)
        {
            return BadRequest("Faça o upload de uma planilha válida.");
        }

        // Check file extension
        if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
        {
            return BadRequest("O formato do arquivo precisa ser .xlsx");
        }
        #endregion
        
        List<PedidoDTO> pedidosDTOsList = new List<PedidoDTO>();

        try
        {
            using (var stream = new MemoryStream())
            {
                // Copy the uploaded file into a memory stream
                file.CopyTo(stream);
                stream.Position = 0;
                
                // Set the LicenseContext
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                
                using (var package = new ExcelPackage(stream))
                {
                    if (package.Workbook == null || package.Workbook.Worksheets.Count == 0)
                    {
                        return BadRequest("Não há dados na planilha.");
                    }

                    ExcelWorksheet ws = package.Workbook.Worksheets.First();

                    int startRow = 2; // considera a partir da linha 2 pois a 1 é o cabeçalho
                    int endRow = ws.Cells.End.Row;

                    for (var i = startRow; i <= endRow; i++)
                    {
                        string? documento = ws.Cells[i, 1].Value?.ToString();
                        string? razaoSocial = ws.Cells[i, 2].Value?.ToString();
                        string? cep = ws.Cells[i, 3].Value?.ToString();
                        string? produtoNome = ws.Cells[i, 4].Value?.ToString();
                        string? numeroPedido = ws.Cells[i, 5].Value?.ToString();
                        string? dataOA = ws.Cells[i, 6].Value?.ToString();
                        
                        if (string.IsNullOrEmpty(documento) &&
                            string.IsNullOrEmpty(razaoSocial) &&
                            string.IsNullOrEmpty(cep) &&
                            string.IsNullOrEmpty(produtoNome) &&
                            string.IsNullOrEmpty(numeroPedido) &&
                            string.IsNullOrEmpty(dataOA))
                        {
                            continue;
                        }

                        double dataOADouble;
                        DateTime dataCriacao;

                        try
                        {
                            // Primeiro tenta converter a string para um double (data OLE)
                            if (!string.IsNullOrEmpty(dataOA) && double.TryParse(dataOA, out dataOADouble))
                            {
                                // Se a conversão para double foi bem-sucedida, converte o double para DateTime
                                dataCriacao = DateTime.FromOADate(dataOADouble);
                            }
                            else
                            {
                                // Se a conversão para double falhou, tenta converter a string diretamente para DateTime
                                dataCriacao = DateTime.Parse(dataOA);
                            }
                        }
                        catch (FormatException)
                        {
                            // Se houver erro na conversão, retorna um BadRequest com a mensagem de erro apropriada
                            return BadRequest($"Não foi possível converter a data informada no worksheet: {ws.Name} na linha: {i}");
                        }

                        // validar campos da planilha
                        var (isSucces, message) = ValidateFields(
                            wsName: ws.Name,
                            row: i,
                            documento: documento,
                            razaoSocial: razaoSocial,
                            cep: cep,
                            produtoNome: produtoNome,
                            numeroPedido: numeroPedido);
                        
                        // falha na validação dos campos, retorna a mensagem de erro
                        if (isSucces == false)
                        {
                            return BadRequest(message);
                        }
                        
                        // TODO: no futuro pode obter o cliente com o documento caso não tenha a razão social?
                        // var cliente = await clienteServices.GetClienteByDocument(documentoNumbers);

                        try
                        {
                            var cliente = new ClienteDTO(documento, razaoSocial);
                        
                            var endereco = await viaCepServices.GetEnderecoByCep(cep);
                        
                            var produto = await produtoServices.GetProdutoByName(produtoNome);
                            
                            if (produto is null)
                                throw new NullReferenceException($"Produto com nome {produtoNome} no worksheet {ws.Name} na linha {i} não foi encontrado no sistema");

                            var (valorFinal, dataEntrega) = _shippingServices
                                .ReturnProdutoValueWithTaxAndDaysToArrived(endereco.UF, produto.Valor, dataCriacao);
                        
                            pedidosDTOsList.Add(
                                new PedidoDTO()
                                {
                                    Id = pedidosDTOsList.Count + 1,
                                    Numero = int.Parse(numeroPedido),
                                    Cep = cep,
                                    Cliente = cliente,
                                    Produto = produto,
                                    UF = endereco.UF,
                                    ValorFinal = valorFinal,
                                    DataCriacao = dataCriacao,
                                    DataEntrega = dataEntrega
                                }
                            );
                        }
                        catch (Exception ex)
                        {
                            return BadRequest("Não foi possível carregar os dados. " + ex.Message);
                        }
                    }
                }
            }

            return Ok(pedidosDTOsList.OrderBy(p => p.Cliente.RazaoSocial));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Houve um erro interno");
        }
    }

    /// <summary>
    /// Validação dos campos informados na planilha modelo
    /// </summary>
    /// <param name="wsName"></param>
    /// <param name="row"></param>
    /// <param name="documento"></param>
    /// <param name="razaoSocial"></param>
    /// <param name="cep"></param>
    /// <param name="produtoNome"></param>
    /// <param name="numeroPedido"></param>
    /// <returns></returns>
    private (bool success, string errorMessage) ValidateFields(
        string wsName, 
        int row, 
        string documento, 
        string razaoSocial,
        string cep,
        string produtoNome,
        string numeroPedido)
    {
        // documento (apenas números)
        if (string.IsNullOrEmpty(documento))
        {
            return (false, $"O documento não foi informado no worksheet: {wsName} na linha: {row}");
        }
        
        if (string.IsNullOrEmpty(razaoSocial))
        {
            return (false, $"A razão social não foi informada no worksheet: {wsName} na linha: {row}");  
        }
        
        if (string.IsNullOrEmpty(cep))
        {
            return (false, $"O cep não foi informado no worksheet: {wsName} na linha: {row}");  
        }

        if (string.IsNullOrEmpty(produtoNome))
        {
            return (false, $"O produto não foi informado no worksheet: {wsName} na linha: {row}");  
        }
        
        if (string.IsNullOrEmpty(numeroPedido))
        {
            return (false, $"O número do pedido não foi informado no worksheet: {wsName} na linha: {row}");
        }
        else
        {
            try
            {
                int.Parse(numeroPedido);
            }
            catch (Exception)
            {
                return (false, $"O número do pedido: {numeroPedido} não foi informado corretamente no worksheet: {wsName} na linha: {row}");
            }
        }

        return (true, "tudo ok");
    }
}