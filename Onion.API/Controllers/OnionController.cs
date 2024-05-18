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
    [HttpGet("planilha-modelo")]
    public async Task<IActionResult> DownloadModelSheet()
    {
        var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "Files", "Planilha-Modelo-Onion.xlsx");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound(); // File not found
        }

        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

        // Set the response headers for file download
        Response.Headers.Add("Content-Disposition", $"attachment; filename=\"Planilha-Modelo-Onion\"");

        // Return the file bytes as a FileContentResult
        return File(fileBytes, "application/octet-stream");
    }
    
    [HttpPost("builddetailsfororder")]
    public async Task<ActionResult<List<PedidoDTO>>> BuildDetailsForOrder(
        IFormFile file, [FromServices] IClienteServices clienteServices, 
        [FromServices] ViaCepServices viaCepServices, [FromServices] IProdutoServices produtoServices)
    {
        if (file == null || file.Length <= 0)
        {
            return BadRequest("Faça o upload de uma planilha válida.");
        }

        // Check file extension
        if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
        {
            return BadRequest("O formato do arquivo precisa ser .xlsx");
        }

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

                    int startRow = 2;
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

                        // converter data OLE para double
                        if (string.IsNullOrEmpty(dataOA) || double.TryParse(dataOA, out dataOADouble) == false)
                        {
                            // se não deu certo, tenta converter de string direto para DateTime
                            try
                            {
                                dataCriacao = DateTime.Parse(dataOA);
                            }
                            catch (Exception)
                            {
                                return BadRequest(
                                    $"Não foi possível converter a data informada no worksheet: {ws.Name} linha {i}");
                            }
                        }
                        else
                        {
                            dataCriacao = DateTime.FromOADate(dataOADouble);
                        }
                        
                        // documento (apenas números)
                        var documentoNumbers = new string(documento.Where(char.IsDigit).ToArray());

                        // var cliente = await clienteServices.GetClienteByDocument(documentoNumbers);

                        var cliente = new ClienteDTO()
                        {
                            Documento = documentoNumbers,
                            RazaoSocial = razaoSocial
                        };
                        
                        var endereco = await viaCepServices.GetEnderecoByCep(cep);
                        
                        var produto = await produtoServices.GetProdutoByName(produtoNome);

                        var (valorFinal, dataEntrega) = _shippingServices
                            .CalculateTaxAndDaysToArrived(endereco.UF, produto.Valor, dataCriacao);
                        
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
                }
            }

            return Ok(pedidosDTOsList);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        }
    }
}