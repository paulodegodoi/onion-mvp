using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Onion.Application.DTOs;
using Onion.Application.Interfaces;
using Onion.Application.Models;
using Onion.Application.Services;
using Onion.Domain.Entities;

namespace Onion.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PedidosController : ControllerBase
{
    private readonly IBaseServices<Pedido> _pedidoServices;

    public PedidosController(IBaseServices<Pedido> produtoServices)
    {
        _pedidoServices = produtoServices;
    }

    // /pedidos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoDTO>>> GetAll()
    {
        var pedidos = await _pedidoServices.GetAllAsync();
        return Ok(pedidos);
    }
    
    // /pedidos/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PedidoDTO>> Get(int id)
    {
        var pedido = await _pedidoServices.GetById(id);

        if (pedido is null)
            return NotFound($"Produto com id: {id} não encontrado.");

        return Ok(pedido);
    }
    
    [HttpPost("builddetailsfororder")]
    public async Task<ActionResult<List<PedidoDTO>>> BuildDetailsForOrder(
        IFormFile file, [FromServices] IClienteServices clienteServices, 
        [FromServices] ViaCepServices viaCepServices, [FromServices] IProdutoServices produtoServices)
    {
        if (file == null || file.Length <= 0)
        {
            return BadRequest("File not selected or file is empty.");
        }

        // Check file extension
        if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
        {
            return BadRequest("Invalid file format. Please upload a .xlsx file.");
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
                        string? documento = ws.Cells[i, 1].Value.ToString();
                        string? razaoSocial = ws.Cells[i, 2].Value.ToString();
                        string? cep = ws.Cells[i, 3].Value.ToString();
                        string? produtoNome = ws.Cells[i, 4].Value.ToString();
                        int numeroPedido = int.Parse(ws.Cells[i, 5].Value.ToString());
                        string? dataOA = ws.Cells[i, 6].Value.ToString();

                        double dataOADouble;

                        if (string.IsNullOrEmpty(dataOA) || double.TryParse(dataOA, out dataOADouble) == false)
                        {
                            return BadRequest(
                                $"Não foi possível converter a data informada no worksheet: {ws.Name} linha {i}");
                        }

                        if (string.IsNullOrEmpty(documento) &&
                            string.IsNullOrEmpty(razaoSocial) &&
                            string.IsNullOrEmpty(cep) &&
                            string.IsNullOrEmpty(produtoNome) &&
                            numeroPedido == 0 &&
                            string.IsNullOrEmpty(dataOA))
                        {
                            continue;
                        }
                        var documentoNumbers = new string(documento.Where(char.IsDigit).ToArray());

                        // var cliente = await clienteServices.GetClienteByDocument(documentoNumbers);

                        var cliente = new ClienteDTO()
                        {
                            Documento = documentoNumbers,
                            RazaoSocial = razaoSocial
                        };

                        var endereco = await viaCepServices.SearchUFByCep(cep);
                        var (taxa, diasEntrega) = ReturnTaxAndDaysToArrived(endereco.UF);
                        
                        // TODO: Verificar dia útil
                        var dataCriacao = DateTime.FromOADate(dataOADouble);
                        
                        var produto = await produtoServices.GetProdutoByName(produtoNome);
                        
                        
                        pedidosDTOsList.Add(
                            new PedidoDTO()
                            {
                                PedidoId = numeroPedido,
                                Cliente = cliente,
                                UF = endereco.UF,
                                ValorFinal = produto.Valor + (produto.Valor * (decimal)taxa),
                                DataCriacao = dataCriacao,
                                DataEntrega = dataCriacao.AddDays(diasEntrega)
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

    private (double, int) ReturnTaxAndDaysToArrived(string uf)
    {
        double tax = 0;
        int daysToArrived = 0;
        
        switch (uf)
        {
            // Norte/Nordeste 30%
            case "AL": case "BA": case "CE": case "MA": case "PB": case "PE": case "PI": case "RN": 
            case "SE": case "AC": case "AM": case "PA": case "RO": case "RR": case "TO":
                tax = 0.3;
                daysToArrived = 10;
                break;
            
            // Centro-Oeste/Sul 20%
            case "DF": case "GO": case "MT": case "MS": case "PR": case "RS": case "SC":
                tax = 0.2;
                daysToArrived = 5;
                break;
            
            // Sudeste 10%
            case "ES": case "MG": case "RJ":
                tax = 0.1;
                daysToArrived = 1;
                break;
            // São Paulo Gratuito e entrega no mesmo dia
        }

        return (tax, daysToArrived);
    }
}