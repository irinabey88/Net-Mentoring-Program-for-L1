using System.Collections.Generic;

namespace Common.Models
{
    public class CustomerDemographic
    {
        public string CustomerTypeID { get; set; }

        public string CustomerDesc { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}