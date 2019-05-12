using System;

namespace Common.Models
{
    public class SearchData
    {
        public string CustomerId { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public int? Take { get; set; }

        public int? Skip { get; set; }
    }
}