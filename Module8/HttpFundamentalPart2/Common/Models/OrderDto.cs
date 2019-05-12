using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common.Models
{
    [XmlRoot(ElementName = "orders")]
    public class OrderDto
    {
        [XmlAttribute(AttributeName = "Id")]
        public int OrderID { get; set; }

        [XmlElement("CustomerId")]
        public string CustomerID { get; set; }

        [XmlElement("EmployeeID")]
        public int? EmployeeID { get; set; }

        [XmlElement("OrderDate")]
        public DateTime? OrderDate { get; set; }

        [XmlElement("RequiredDate")]
        public DateTime? RequiredDate { get; set; }

        [XmlElement("ShippedDate")]
        public DateTime? ShippedDate { get; set; }

        [XmlElement("ShipVia")]
        public int? ShipVia { get; set; }

        [XmlElement("Freight")]
        public decimal? Freight { get; set; }

        [XmlElement("ShipName")]
        public string ShipName { get; set; }

        [XmlElement("ShipAddress")]
        public string ShipAddress { get; set; }

        [XmlElement("ShipCity")]
        public string ShipCity { get; set; }

        [XmlElement("ShipRegion")]
        public string ShipRegion { get; set; }

        [XmlElement("ShipPostalCode")]
        public string ShipPostalCode { get; set; }

        [XmlElement("ShipCountry")]
        public string ShipCountry { get; set; }
    }
}
