using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Common.Interfaces;
using Common.Models;
using OfficeOpenXml;

namespace HttpHandler.Converter
{
    public class RequestConvrter: IConverter
    {
        public void ConvertToXml(IEnumerable<OrderDto> orders, Stream stream)
        {
            var serializer = new XmlSerializer(typeof(List<OrderDto>));
            serializer.Serialize(stream, orders.ToList());
        }

        public void ConvertToExcel(IEnumerable<OrderDto> orders, Stream stream)
        {
            using (var excelPackage = new ExcelPackage())
            {
                var writer = excelPackage.Workbook.Worksheets.Add("Orders");
                writer.Cells.LoadFromCollection(orders, true);
                writer.Cells.AutoFitColumns();
                excelPackage.SaveAs(stream);
            }
        }
    }
}