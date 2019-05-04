using Attributes;
using TestData.Interfaces;

namespace Reflection.Models
{
    [Export(typeof(ICustomerDal))]
    public class CustomerDal : ICustomerDal
    {
        
    }
}