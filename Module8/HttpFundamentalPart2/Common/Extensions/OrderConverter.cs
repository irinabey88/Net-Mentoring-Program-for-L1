using Common.Models;

namespace Common.Extensions
{
    public static class OrderConverter
    {
        public static OrderDto ConvertToOrderDto(this Order order)
        {
            return new OrderDto
            {
                CustomerID = order.CustomerID,
                EmployeeID = order.EmployeeID,
                Freight = order.Freight,
                OrderDate = order.OrderDate,
                ShippedDate = order.ShippedDate,
                OrderID = order.OrderID,
                RequiredDate = order.RequiredDate,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipName = order.ShipName,
                ShipCountry = order.ShipCountry,
                ShipVia = order.ShipVia,
                ShipPostalCode = order.ShipPostalCode,
                ShipRegion = order.ShipRegion
            };
        }
    }
}