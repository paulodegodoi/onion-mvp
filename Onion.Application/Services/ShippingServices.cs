using Onion.Domain.Interfaces;

namespace Onion.Application.Services;

public class ShippingServices : IShippingServices
{
    public (decimal finalPrice, DateTime arrivedDate) CalculateTaxAndDaysToArrived(string uf, decimal createdPrice, DateTime createdDate)
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

        DateTime arrivedDate = createdDate;
        
        for (int i = 0; i < daysToArrived; i++)
        {
            arrivedDate = arrivedDate.AddDays(1);

            if (IsDiaUtil(arrivedDate) == false)
                i--;
        }

        decimal finalPrice = createdPrice + (createdPrice * (decimal)tax);
        return (finalPrice, arrivedDate);
    }

    private bool IsDiaUtil(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return false;
        
        // TODO: Verificar feriados brasileiros
        return true;
    }
}