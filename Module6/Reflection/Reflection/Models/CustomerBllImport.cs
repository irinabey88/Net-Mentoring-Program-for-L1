using Attributes;
using TestData.Interfaces;

namespace Reflection.Models
{
    public class CustomerBllImport
    {
        [Import]
        public ICustomerDal CustomerDal { get; set; }

        [Import]
        public Logger Logger { get; set; }
    }
}