using System.Collections.Generic;

namespace Common.Models
{
    public class Territory
    {
        public string TerritoryID { get; set; }

        public int RegionID { get; set; }

        public string TerritoryDescription { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}