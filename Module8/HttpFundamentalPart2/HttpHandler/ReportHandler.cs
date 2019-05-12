using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Common.Interfaces;
using Common.Models;
using Dataccess;
using HttpHandler.Converter;
using HttpHandler.Parser;

namespace HttpHandler
{
    public class ReportHandler: IHttpHandler
    {
        private readonly IConverter _converter = new RequestConvrter();

        private readonly IParser _parser = new RequestParser();

        private readonly IRepository<Order> _repository = new OrdersRepository(new NorthwindContext());
        
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;

            if (request.Url.PathAndQuery == "/~close")
            {
                return;
            }

            var searchData = request.HttpMethod == "GET"
                ? _parser.ParseFromQueryString(request.QueryString)
                : _parser.ParseFromBody(request.InputStream);

            FormResponseHeader(context, searchData);
        }

        private void FormResponseHeader(HttpContext context, SearchData searchData)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (searchData == null)
            {
                return;
            }

            var request = context.Request;
            var response = context.Response;

            using (var memoryStream = new MemoryStream())
            {
                var acceptType = request.AcceptTypes?.FirstOrDefault();
                var convertedData = _repository.GetOrders(searchData);

                switch (acceptType)
                {
                    case "text/xml":
                    case "application/xml":
                    {
                        _converter.ConvertToXml(convertedData, memoryStream);
                        response.AppendHeader("Content-Type", acceptType);
                        response.AppendHeader("Content-Disposition", "attachment; filename=orders.xml;");
                        break;
                    }
                    default:
                        {
                            _converter.ConvertToExcel(convertedData, memoryStream);
                            response.AppendHeader("Content-Type", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                            response.AppendHeader("Content-Disposition", "attachment; filename=orders.xlsx;");
                            break;
                        }
                }

                response.StatusCode = (int)HttpStatusCode.OK;

                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.WriteTo(response.OutputStream);
                response.OutputStream.Flush();
                response.OutputStream.Close();
            }            
        }
    }
}