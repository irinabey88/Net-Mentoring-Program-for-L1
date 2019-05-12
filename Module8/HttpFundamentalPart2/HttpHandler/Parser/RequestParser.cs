using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Common.Interfaces;
using Common.Models;

namespace HttpHandler.Parser
{
    public class RequestParser : IParser
    {
        public SearchData ParseFromQueryString(NameValueCollection queryString)
        {
            if (queryString == null || queryString.Count == 0)
            {
                return null;
            }

            return new SearchData
            {
                CustomerId = queryString["customerId"],
                DateFrom = string.IsNullOrWhiteSpace(queryString["dateFrom"]) ? null : (DateTime?)Convert.ToDateTime(queryString["dateFrom"]),
                DateTo = string.IsNullOrWhiteSpace(queryString["dateTo"]) ? null : (DateTime?)Convert.ToDateTime(queryString["dateTo"]),
                Take = string.IsNullOrWhiteSpace(queryString["take"]) ? null : (int?)Convert.ToInt32(queryString["take"]),
                Skip = string.IsNullOrWhiteSpace(queryString["skip"]) ? null : (int?)Convert.ToInt32(queryString["skip"])
            };
        }

        public SearchData ParseFromBody(Stream body)
        {
            using (var reader = new StreamReader(body))
            {
                var inputData = reader.ReadToEnd().Split('&');

                return new SearchData
                {
                    CustomerId = GetStringParameter("customerId", inputData),
                    DateFrom = GetDateParameter("dateFrom", inputData),
                    DateTo = GetDateParameter("dateTo", inputData),
                    Take = GetNumberParameter("take", inputData),
                    Skip = GetNumberParameter("skip", inputData)
                };
            }
        }

        private string GetStringParameter(string parameterName, string[] data)
        {
            var parameter = data.FirstOrDefault(param => param.StartsWith(parameterName))?.Split('=');
            return parameter?[1];
        }

        private int? GetNumberParameter(string parameterName, string[] data)
        {
            var parameter = data.FirstOrDefault(param => param.StartsWith(parameterName))?.Split('=');
            return parameter == null ? null : (int?)Convert.ToInt32(parameter[1]);
        }

        private DateTime? GetDateParameter(string parameterName, string[] data)
        {
            var parameter = data.FirstOrDefault(param => param.StartsWith(parameterName))?.Split('=');
            return parameter == null ? null : (DateTime?) Convert.ToDateTime(parameter[1]);
        }
    }
}