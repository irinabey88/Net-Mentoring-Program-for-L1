using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Common.Models;

namespace Common.Interfaces
{
    public interface IRepository<T> where T: class
    {
        IEnumerable<OrderDto> GetOrders(SearchData searchData);
    }
}