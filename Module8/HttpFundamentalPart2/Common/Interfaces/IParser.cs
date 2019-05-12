using System.Collections.Specialized;
using System.IO;
using Common.Models;

namespace Common.Interfaces
{
    public interface IParser
    {
        SearchData ParseFromQueryString(NameValueCollection queryString);

        SearchData ParseFromBody(Stream body);
    }
}