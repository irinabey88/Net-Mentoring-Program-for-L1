using System;
using System.Reflection;
using IocContainer;
using Reflection.Models;
using ReflectionObjectCreator;
using TestData.Interfaces;

namespace Reflection
{
    class Program
    {
        static void Main(string[] args)
        {         
            try
            {
                //var container = new Container(new ReflectionEmitActivator(Assembly.GetExecutingAssembly().FullName));
                var container = new Container(new ReflectionActivator());

                container.AddType(typeof(CustomerBllImportConstructor));
                container.AddType(typeof(CustomerBllImport));
                container.AddType(typeof(Logger));
                container.AddType(typeof(ICustomerDal), typeof(CustomerDal));

                var customerBllImportConstructor = (CustomerBllImportConstructor)container.CreateInstance(typeof(CustomerBllImportConstructor));
                var customerBllImportConstructorGen = container.CreateInstance<CustomerBllImportConstructor>();

                var customerBllImport = (CustomerBllImport)container.CreateInstance(typeof(CustomerBllImport));
                var customerBllImportGen = container.CreateInstance<CustomerBllImport>();

                var createdByInterfaceObj = container.CreateInstance<ICustomerDal>();
                var createdByInterfaceObjGen = container.CreateInstance(typeof(ICustomerDal));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
