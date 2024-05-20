using Onion.Domain.Interfaces;

namespace Onion.Application.Services;

public class ShippingServices : IShippingServices
{
    public (decimal finalPrice, DateTime arrivedDate) ReturnProdutoValueWithTaxAndDaysToArrived(string uf, decimal createdPrice, DateTime createdDate)
    {
        if (string.IsNullOrEmpty(uf))
            throw new NullReferenceException("É necessário informar o UF");
        
        double tax = 0;
        int daysToArrived = 0;
        
        switch (uf.ToUpper())
        {
            // Norte/Nordeste 30% e 10 dias úteis
            case "AL": case "AP": case "BA": case "CE": case "MA": case "PB": case "PE": case "PI": 
            case "RN": case "SE": case "AC": case "AM": case "PA": case "RO": case "RR": case "TO":
                tax = 0.3;
                daysToArrived = 10;
                break;
            
            // Centro-Oeste/Sul 20% e 5 dias úteis
            case "DF": case "GO": case "MT": case "MS": case "PR": case "RS": case "SC":
                tax = 0.2;
                daysToArrived = 5;
                break;
            
            // Sudeste 10% e entrega em 1 dia útil
            case "ES": case "MG": case "RJ":
                tax = 0.1;
                daysToArrived = 1;
                break;
            
            // São Paulo Gratuito e entrega no mesmo dia
            case "SP":
                tax = 0;
                daysToArrived = 0;
                break;
            
            default: throw new ArgumentOutOfRangeException($"UF: {uf} desconhecida.");
        }

        // Data de início é a data de criação do pedido
        DateTime arrivedDate = createdDate;
        
        int i = 0;
        // loop por dias para entrega
        while (i < daysToArrived)
        {
            arrivedDate = arrivedDate.AddDays(1);

            if (IsDiaUtil(arrivedDate))
            {
                i++;
            }
        }

        decimal finalPrice = createdPrice + (createdPrice * (decimal)tax);
        return (finalPrice, arrivedDate);
    }

    // Verifica se o dia é sábado, domingo ou feriado
    private bool IsDiaUtil(DateTime date)
    {
        if (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday || IsHoliday(date))
            return false;
        
        return true;
    }
    
    // Verifica se o dia é feriado no brasil (ano 2024)
    private bool IsHoliday(DateTime date)
    {
        return Holidays2024.Contains(date.Date);
    }
    
    private static readonly HashSet<DateTime> Holidays2024 = new HashSet<DateTime>
    {
        new DateTime(2024, 1, 1),   // Confraternização Universal
        new DateTime(2024, 2, 12),  // Carnaval
        new DateTime(2024, 2, 13),  // Carnaval
        new DateTime(2024, 3, 29),  // Sexta-feira Santa
        new DateTime(2024, 4, 21),  // Tiradentes
        new DateTime(2024, 5, 1),   // Dia do Trabalho
        new DateTime(2024, 5, 30),  // Corpus Christi
        new DateTime(2024, 9, 7),   // Independência do Brasil
        new DateTime(2024, 10, 12), // Nossa Senhora Aparecida
        new DateTime(2024, 11, 2),  // Finados
        new DateTime(2024, 11, 15), // Proclamação da República
        new DateTime(2024, 12, 25)  // Natal
    };
}