using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Common.Interfaces;
using Common.Models;

namespace Dataccess
{
    public class OrdersRepository: IRepository<Order>
    {
        private readonly NorthwindContext _context;

        public OrdersRepository(NorthwindContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<OrderDto> GetOrders(SearchData searchData)
        {
            return FilterData(searchData);
        }

        private IEnumerable<OrderDto> FilterData(SearchData searchData)
        {
            if (searchData == null)
            {
                return _context.Orders
                               .Select(order => order.ConvertToOrderDto())
                               .ToList();
            }

            var result = _context.Orders.Where(order => order.OrderID > 0);

            if (!string.IsNullOrWhiteSpace(searchData.CustomerId))
            {
                result = result.Where(order => order.CustomerID == searchData.CustomerId);
            }
            if (searchData.DateFrom.HasValue)
            {
                result = result.Where(order => order.OrderDate >= searchData.DateFrom);
            }
            if (searchData.DateTo.HasValue)
            {
                result = result.Where(order => order.OrderDate <= searchData.DateTo);
            }
            if (searchData.Skip.HasValue)
            {
                result = result
                    .OrderBy(order => order.OrderID)
                    .Skip((int)searchData.Skip);
            }
            if (searchData.Take.HasValue)
            {
                result = result.Take((int) searchData.Take);
            }
            
            var mappedResult = result.ToList()
                                     .Select(order => order.ConvertToOrderDto())
                                     .OrderBy(order => order.OrderID);
            return mappedResult;
        }
    }
}