namespace Onion.Domain.Interfaces;

public interface IShippingServices
{
    /// <summary>
    /// Calcula com base no estado informado o valor final
    /// (valor do pedido + taxa de entrega (frete)) e a data que o pedido será entregue.
    /// </summary>
    /// <param name="uf">sigla do estado</param>
    /// <param name="createdPrice">indica o valor do pedido sem as taxa de entrega</param>
    /// <param name="createdDate">indica quando o pedido foi criado, essa data é a base para o calculo do dia de entrega</param>
    /// <returns></returns>
    (decimal finalPrice, DateTime arrivedDate) CalculateTaxAndDaysToArrived(string uf, decimal createdPrice, DateTime createdDate);
}