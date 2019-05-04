using Attributes;
using TestData.Interfaces;

namespace TestData.Models
{
    [Export(typeof(ICustomerDal))]
    public class CustomerDal : ICustomerDal
    {
        
    }
}