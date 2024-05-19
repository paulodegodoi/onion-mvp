using Onion.Tests.Helpers.TestDoubles.Dummy;

namespace Onion.Tests.UnitTests.Services;


public class ShippingServicesTests
{
    [Fact]
    public void Validar_Calculo_ValorFinal_E_DataEntrega()
    {
        // Arrange
        var dateCreated = new DateTime(2024, 12, 13);
        var uf = "SP";
        var price = 3000;
        var shippingServices = new ShippingServicesDummy();
        
        // Act
        var (value, arrivedDate) = shippingServices
            .ReturnProdutoValueWithTaxAndDaysToArrived(uf, price, dateCreated);
        
        // Assert
        var expectedDate = new DateTime(2024, 12, 13);
        var expectedValue = 3000;
        
        Assert.Equal(expectedValue, value);
        Assert.Equal(expectedDate, arrivedDate);
    }
}