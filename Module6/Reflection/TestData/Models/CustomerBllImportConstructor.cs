using Attributes;
using TestData.Interfaces;

namespace TestData.Models
{
    [ImportConstructor]
    public class CustomerBllImportConstructor
    {

        public CustomerBllImportConstructor(ICustomerDal customerDal, Logger logger)
        {
            CustomerDal = customerDal;
            Logger = logger;
        }

        public ICustomerDal CustomerDal { get; set; }

        public Logger Logger { get; set; }
    }
}
