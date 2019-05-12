using System.Collections.Generic;
using System.IO;
using Common.Models;

namespace Common.Interfaces
{
    public interface IConverter
    {
        void ConvertToXml(IEnumerable<OrderDto> orders, Stream stream);

        void ConvertToExcel(IEnumerable<OrderDto> orders, Stream stream);
    }
}