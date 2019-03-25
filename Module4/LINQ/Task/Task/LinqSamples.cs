// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using SampleSupport;
using System;
using System.Linq;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
    [Title("LINQ Module")]
	[Prefix("Linq")]
	public class LinqSamples : SampleHarness
	{

		private DataSource dataSource = new DataSource();

		[Category("Restriction Operators")]
		[Title("Where - Task example 1")]
		[Description("This sample uses the where clause to find all elements of an array with a value less than 5.")]
		public void Linq1()
		{
			int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

			var lowNums =
				from num in numbers
				where num < 5
				select num;

			Console.WriteLine("Numbers < 5:");
			foreach (var x in lowNums)
			{
				Console.WriteLine(x);
			}
		}

		[Category("Restriction Operators")]
		[Title("Where - Task example 2")]
		[Description("This sample return return all presented in market products")]
        public void Linq2()
		{
			var products =
				from p in dataSource.Products
				where p.UnitsInStock > 0
				select p;

			foreach (var p in products)
			{
				ObjectDumper.Write(p);
			}
		}

	    [Category("Restriction Operators")]
	    [Title("Where - Task 1 - .Net Notation")]
	    [Description(
	        "Get a list of all customers whose total turnover (the sum of all orders) exceeds some value of X. ")]
	    public void Linq3()
	    {
	        var checkSumValue = 3000;

	        var customers = dataSource.Customers
	            .Select(customer => new
	            {
	                Name = customer.CompanyName,
	                OrderAmout = customer.Orders.Sum(order => order.Total)
	            })
	            .Where(customer => customer.OrderAmout > checkSumValue);

	        foreach (var customer in customers)
	        {
	            Console.WriteLine(customer);
	        }
	    }

	    [Category("Restriction Operators")]
	    [Title("Where - Task 1 - QueryNotation")]
	    [Description(
	        "Get a list of all customers whose total turnover (the sum of all orders) exceeds some value of X. ")]
	    public void Linq31()
	    {
	        var checkSumValue = 3000;

	        var customers = from customer in dataSource.Customers
	            where customer.Orders.Sum(order => order.Total) > checkSumValue
	            select new
	            {
	                Name = customer.CompanyName,
	                OrderAmout = customer.Orders.Sum(order => order.Total)
	            };

	        foreach (var customer in customers)
	        {
	            Console.WriteLine(customer);
	        }
	    }

        [Category("Grouping Operators")]
	    [Title("GroupJoin - Task 2 - .Net Notation")]
	    [Description("Make a list of suppliers located in the same country and the same city for each client.")]
	    public void Linq4()
	    {
	        var customers = dataSource.Customers.GroupJoin(dataSource.Suppliers,
	                customer => new { customer.Country, customer.City },
	                suplier => new { suplier.Country, suplier.City },
	                (cust, supl) => new
	                {
	                    Name = cust.CompanyName,
	                    Country = cust.Country,
	                    City = cust.City,
	                    Supliers = supl.Select(supplier => new
	                    {
	                        Name = supplier.SupplierName,
	                    })
	                })
	            .SelectMany(suplier => suplier.Supliers, (customer, suplier) => new
	            {
	                CustomerName = customer.Name,
	                SuplierName = suplier.Name,
	                CustomerCountry = customer.Country,
	                CustomerCity = customer.City
	            });

	        foreach (var customer in customers)
	        {
	            Console.WriteLine(customer);
	        }
	    }

	    [Category("Grouping Operators")]
	    [Title("GroupJoin - Task 2 - Query Notation")]
	    [Description("Make a list of suppliers located in the same country and the same city for each client.")]
	    public void Linq41()
	    {
	        var customers = from customer in dataSource.Customers
	                        join suplier in dataSource.Suppliers
	                            on new
	                            {
	                                customer.Country,
	                                customer.City
	                            }
	                            equals new
	                            {
	                                suplier.Country,
	                                suplier.City
	                            }
                            into cs
	                        select new
	                        {
                                Name = customer.CompanyName,
	                            Country = customer.Country,
	                            City = customer.City,
	                            Supliers = cs
                            };

            var result = from customer in customers
                         from suplier in customer.Supliers
                         select new
                         {
                             CustomerName = customer.Name,
                             SuplierName = suplier.SupplierName,
                             CustomerCountry = customer.Country,
                             CustomerCity = customer.City
                         };

            foreach (var customer in result)
	        {
	            Console.WriteLine(customer);
	        }
	    }

        [Category("Joining Operators")]
        [Title("Join - Task 2 - .Net Notation")]
        [Description("Make a list of suppliers located in the same country and the same city for each client.")]
        public void Linq5()
        {
            var customers = dataSource.Customers.Join(dataSource.Suppliers,
                customer => new {customer.Country, customer.City},
                suplier => new {suplier.Country, suplier.City},
                (cust, supl) => new 
                {
                    CustomerName = cust.CompanyName,
                    SuplierName = supl.SupplierName,
                    Country = cust.Country,
                    City = cust.City
                });

            foreach (var x in customers)
            {
                Console.WriteLine(x);
            }
        }

	    [Category("Joining Operators")]
	    [Title("Join - Task 2 - Query Notation")]
	    [Description("Make a list of suppliers located in the same country and the same city for each client.")]
	    public void Linq51()
	    {
            var customers = from suplier in dataSource.Suppliers
	                         join customer in dataSource.Customers 
                             on new
	                             {
	                                 suplier.Country,
                                     suplier.City
	                             } 
                             equals new
	                             {
	                                 customer.Country,
                                     customer.City
	                             }
                             select new
                             {
                                 Customer = customer.CompanyName,
                                 Supplier = suplier.SupplierName,
                                 Country = customer.Country,
                                 City = customer.City
                             };

            foreach (var x in customers)
	        {
	            Console.WriteLine(x);
	        }
	    }

        [Category("Restriction Operators")]
	    [Title("Where - Task 3 - .Net Notation")]
	    [Description("Find all customers who have orders that exceed the sum of X")]
	    public void Linq6()
	    {
	        int orderPrice = 8000;

            var customers = dataSource.Customers
	            .Where(customer => customer.Orders.Any(order => order.Total > orderPrice))
                .Select(customer => customer.CompanyName);

            foreach (var x in customers)
	        {
	            Console.WriteLine(x);
	        }
	    }

	    [Category("Restriction Operators")]
	    [Title("Where - Task 3 - Query Notation")]
	    [Description("Find all customers who have orders that exceed the sum of X")]
	    public void Linq61()
	    {
	        int orderPrice = 8000;

	        var customers = from customer in dataSource.Customers
                            where customer.Orders.Any(order => order.Total > orderPrice)
                            select new
                            {
                                Name = customer.CompanyName
                            };

	        foreach (var x in customers)
	        {
	            Console.WriteLine(x);
	        }
        }

        [Category("Selecting Operators")]
	    [Title("Select - Task 4 - .Net Notation")]
	    [Description(
	        "Get a list of customers including the month of the year they became clients (consider client first order month as a required date).")]
	    public void Linq7()
	    {
	        var customers = dataSource.Customers
	            .Select(customer => new
	                {
	                    Name = customer.CompanyName,
                        StartDate = customer.Orders.Length > 0 ? 
                            customer.Orders.Min(order => order.OrderDate) : DateTime.MinValue
	                });

	        foreach (var x in customers)
	        {
	            Console.WriteLine(x);
	        }
	    }

	    [Category("Selecting Operators")]
	    [Title("Select - Task 4 - Query Notation")]
	    [Description(
	        "Get a list of customers including the month of the year they became clients (consider client first order month as a required date).")]
	    public void Linq71()
	    {
	        var customers = from customer in dataSource.Customers
                            select new
                            {
                                Name = customer.CompanyName,
                                StartDate = customer.Orders.Length > 0 ?
                                customer.Orders.Min(order => order.OrderDate) : DateTime.MinValue
                            };

	        foreach (var x in customers)
	        {
	            Console.WriteLine(x);
	        }
        }

	    [Category("Ordering Operators")]
	    [Title("OrderBy, ThenBy - Task 5 - .Net Notation")]
	    [Description(
	        "Do the previous task, but get the list sorted by year, month, customer turnover (from the maximum to the minimum), and the client's name.")]
	    public void Linq8()
	    {
	        var customers = dataSource.Customers
	            .Select(customer => new
	            {
	                Name = customer.CompanyName,
	                StartDate = customer.Orders.Length > 0 ?
	                    customer.Orders.Min(order => order.OrderDate) : DateTime.MinValue,
	                OrderAmout = customer.Orders.Sum(order => order.Total)
	            })
	            .OrderBy(order => order.StartDate.Year)
	            .ThenBy(order => order.StartDate.Month)
	            .ThenByDescending(order => order.OrderAmout)
	            .ThenBy(order => order.Name);


	        foreach (var x in customers)
	        {
	            Console.WriteLine(x);
	        }
	    }

        
        [Category("Ordering Operators")]
	    [Title("OrderBy, ThenBy - Task 5 - Query Notation")]
	    [Description(
	        "Do the previous task, but get the list sorted by year, month, customer turnover (from the maximum to the minimum), and the client's name.")]
	    public void Linq81()
	    {
	        var customers = from customer in dataSource.Customers
	                        select new
	                        {
	                            Name = customer.CompanyName,
	                            StartDate = customer.Orders.Length > 0 ?
	                                customer.Orders.Min(order => order.OrderDate) : DateTime.MinValue,
	                            OrderAmout = customer.Orders.Sum(order => order.Total)
                            };

	        var result = from customer in customers
                         orderby customer.StartDate.Year,
                                 customer.StartDate.Month,
                                 customer.OrderAmout descending,
                                 customer.Name
                         select customer;

	        foreach (var x in result)
	        {
	            Console.WriteLine(x);
	        }
        }

        [Category("Selecting Operators")]
	    [Title("Select - Task 6 - .Net Notation")]
	    [Description(
	        "Specify all customers who have a non-digital postal code, or the region is not filled, or the operator code is not specified in the phone (for simplicity, consider that this means there are no round brackets at the beginning)")]
	    public void Linq9()
	    {
	        var customers = dataSource.Customers.Where(customer =>
	                !int.TryParse(customer.PostalCode, out _) ||
	                string.IsNullOrEmpty(customer.Region) ||
	                !customer.Phone.StartsWith("("))
	            .Select(customer => new
	            {
	                Name = customer.CompanyName,
	                Phone = customer.Phone,
	                Region = customer.Region,
	                PostalCode = customer.PostalCode
	            });

	        foreach (var x in customers)
	        {
	            Console.WriteLine(x);
	        }
	    }

	    [Category("Selecting Operators")]
	    [Title("Select - Task 6 - Query Notation")]
	    [Description(
	        "Specify all customers who have a non-digital postal code, or the region is not filled, or the operator code is not specified in the phone (for simplicity, consider that this means there are no round brackets at the beginning)")]
	    public void Linq91()
	    {
	        var customers = from customer in dataSource.Customers
                            where !int.TryParse(customer.PostalCode, out _) 
                                  || string.IsNullOrEmpty(customer.Region)
                                  || !customer.Phone.StartsWith("(")
                            select new
                            {
                                Name = customer.CompanyName,
                                Phone = customer.Phone,
                                Region = customer.Region,
                                PostalCode = customer.PostalCode
                            };


	        foreach (var x in customers)
	        {
	            Console.WriteLine(x);
	        }
        }

        [Category("Grouping Operators")]
	    [Title("GroupBy - Task 7 - .Net Notation")]
	    [Description("Group all products by category, inside - by stock, within the last group sort by cost.")]
	    public void Linq10()
	    {
	        var products = dataSource.Products
	            .GroupBy(product => product.Category,
	                (category, items) => new
	                {
	                    Category = category,
	                    StockAvaliable = items.GroupBy(itemsInStock => itemsInStock.UnitsInStock,
	                    (itemsNumber, units) => new 
	                    {
                            Units = units.OrderBy(unit => unit.UnitPrice)
	                    })
	                });

	        foreach (var x in products)
	        {
                Console.WriteLine(x.Category);
	            foreach (var y in x.StockAvaliable)
	            {
	                foreach (var z in y.Units)
	                {
	                    Console.WriteLine($"Product = {z.ProductName} InStock = {z.UnitsInStock} Price = {z.UnitPrice}");
	                }
	            }
	        }
	    }

	    [Category("Grouping Operators")]
	    [Title("GroupBy - Task 7 - Query Notation")]
	    [Description("Group all products by category, inside - by stock, within the last group sort by cost.")]
	    public void Linq101()
	    {
	        var products = from product in dataSource.Products
                           group product by product.Category into categoriesGroup
                           select new
                           {
                               Category = categoriesGroup.Key,
                               StockAvaliable = from item in categoriesGroup
                                                group item by item.UnitsInStock into stockGroup
                                                select new
                                                {
                                                    Units = stockGroup
                                                }
                           };

	        foreach (var x in products)
	        {
	            Console.WriteLine(x.Category);
	            foreach (var y in x.StockAvaliable)
	            {
	                foreach (var z in y.Units)
	                {
	                    Console.WriteLine($"Product = {z.ProductName} InStock = {z.UnitsInStock} Price = {z.UnitPrice}");
	                }
	            }
	        }
        }

        [Category("Grouping Operators")]
	    [Title("GroupBy - Task 8 - .Net Notation")]
	    [Description(
	        "Group the goods into groups cheap, average price, expensive. The boundaries of each group set yourself.")]
	    public void Linq11()
	    {
	        int cheap = 20;
	        int expencive = 50;
	        var products = dataSource.Products.Select(product =>
	            {
	                string groupName;
	                if (product.UnitPrice < cheap)
	                {
	                    groupName = "Cheap";
	                }
	                else if (product.UnitPrice > expencive)
	                {
	                    groupName = "Expencive";
	                }
	                else
	                {
	                    groupName = "Average price";
	                }
	                return new
	                {
	                    ProductName = product.ProductName,
	                    Price = product.UnitPrice,
	                    Category = groupName
	                };
	            })
	            .GroupBy(product => product.Category,
	                (category, items) => new
	                {
                        Category = category,
                        Products = items
                    });

	        foreach (var x in products)
	        {
	            Console.WriteLine(x.Category);
	            foreach (var y in x.Products)
	            {
	                Console.WriteLine($"Name = {y.ProductName} Price = {y.Price}");
	            }
	        }
	    }

	    [Category("Grouping Operators")]
	    [Title("GroupBy - Task 8 - Query Notation")]
	    [Description(
	        "Group the goods into groups cheap, average price, expensive. The boundaries of each group set yourself.")]
	    public void Linq111()
	    {
	        int cheap = 20;
	        int expencive = 50;
	        var products = from product in dataSource.Products
	                       let groupName = product.UnitPrice < cheap
	                           ? "Cheap": product.UnitPrice > expencive
	                           ? "Expencive" : "Average price"
                           select new
                               {
                                   ProductName = product.ProductName,
                                   Price = product.UnitPrice,
                                   Category = groupName
                               };
	        var result = from product in products
	            group product by product.Category
	            into categoriesGroup
	            select new
	            {
	                Category = categoriesGroup.Key,
	                Products = categoriesGroup
	            };

	        foreach (var x in result)
	        {
	            Console.WriteLine(x.Category);
	            foreach (var y in x.Products)
	            {
	                Console.WriteLine($"Name = {y.ProductName} Price = {y.Price}");
	            }
	        }
        }

        [Category("Grouping Operators")]
	    [Title("GroupBy - Task 9 - .Net Notation")]
	    [Description(
	        "Calculate the average profitability of each city (the average amount of the order for all customers from a given city) and the average intensity (the average number of orders per customer from each city).")]
	    public void Linq12()
	    {
	        var customers = dataSource.Customers
	            .Select(customer => new
	            {
	                customer.City,
	                customer.CompanyName,
	                Total = customer.Orders.Sum(order => order.Total),
	                OrdersAmount = customer.Orders.Count()
	            })
	            .GroupBy(customer => customer.City)
	            .Select(city => new
	            {
	                City = city.Key,
	                AvgOrderPrice = city.Sum(order => order.Total) / city.Sum(order => order.OrdersAmount),
	                AvgOrderCount = city.Average(order => order.OrdersAmount) / dataSource.Customers.Count(customer => customer.City.Equals(city.Key)),
	            });

	        foreach (var x in customers)
	        {
	            Console.WriteLine(x);
	        }
	    }

	    [Category("Grouping Operators")]
	    [Title("GroupBy - Task 9 - Query Notation")]
	    [Description(
	        "Calculate the average profitability of each city (the average amount of the order for all customers from a given city) and the average intensity (the average number of orders per customer from each city).")]
	    public void Linq121()
	    {
	        var customers = from customer in dataSource.Customers
	                        select new
	                        {
	                            customer.City,
	                            customer.CompanyName,
	                            Total = customer.Orders.Sum(order => order.Total),
	                            OrdersAmount = customer.Orders.Count()
	                        };

	        var result = from customer in customers
	            group customer by customer.City
	            into customersGroup
	            select new
	            {
	                City = customersGroup.Key,
	                AvgOrderPrice = customersGroup.Sum(order => order.Total) /
	                                customersGroup.Sum(order => order.OrdersAmount),
	                AvgOrderCount = customersGroup.Average(order => order.OrdersAmount) /
	                                dataSource.Customers.Count(customer => customer.City.Equals(customersGroup.Key))
	            };

	        foreach (var x in result)
	        {
	            Console.WriteLine(x);
	        }
        }

        [Category("Grouping Operators")]
	    [Title("GroupBy - Task 10(month) - .Net Notation")]
	    [Description("Make the average annual activity statistics of clients by month (excluding the year)")]
	    public void Linq13()
	    {
	        var result = dataSource.Customers
                .SelectMany(customer => customer.Orders, (customer, order) => new
	                {
                        Date = order.OrderDate,
	                    OrdersAmount = customer.Orders.Count()
	                })
                    .GroupBy(order => order.Date.Month, (month, orders) => new
	                {
	                    Month = month,
                        Total = orders.Sum(order => order.OrdersAmount)
	                });

	        foreach (var x in result)
	        {
	            Console.WriteLine(x);
	        }
	    }

	    [Category("Grouping Operators")]
        [Title("GroupBy - Task 10(month) - Query Notation")]
	    [Description("Make the average annual activity statistics of clients by month (excluding the year)")]
	    public void Linq131()
	    {
	       var orders = from customer in dataSource.Customers
                            from order in customer.Orders
                            select new
                            {
                                Date = order.OrderDate,
                                OrdersAmount = customer.Orders.Count()
                            };

            var result = from order in orders
                         group order by order.Date.Month into monthGroup
                         select new
                         {
                             Month = monthGroup.Key,
                             Total = monthGroup.Sum(order => order.OrdersAmount)
                         };

	        foreach (var x in result)
	        {
	            Console.WriteLine(x);
	        }
        }

        [Category("Grouping Operators")]
        [Title("GroupBy - Task 10(year) - .Net Notation")]
	    [Description("Make the average annual activity statistics of clients by year")]
	    public void Linq14()
	    {
	        var result = dataSource.Customers
                .SelectMany(order => order.Orders, (customer, order) => new
	            {
	                    Date = order.OrderDate,
	                    OrdersAmount = customer.Orders.Count()
                })
                .GroupBy(order => order.Date.Year, (year, orders) => new
	            {
	                Year = year,
                    Total = orders.Sum(order => order.OrdersAmount)
	            });

	        foreach (var x in result)
	        {
	            Console.WriteLine(x);
	        }
	    }

	    [Category("Grouping Operators")]
	    [Title("GroupBy - Task 10(year) - Query Notation")]
	    [Description("Make the average annual activity statistics of clients by year")]
	    public void Linq141()
	    {
	        var orders = from customer in dataSource.Customers
	            from order in customer.Orders
	            select new
	            {
	                Date = order.OrderDate,
	                OrdersAmount = customer.Orders.Count()
	            };

            var result = from order in orders
	            group order by order.Date.Year into yearGroup
	            select new
	            {
	                Year = yearGroup.Key,
	                Total = yearGroup.Sum(order => order.OrdersAmount)
	            };

	        foreach (var x in result)
	        {
	            Console.WriteLine(x);
	        }
        }

        [Category("Grouping Operators")]
	    [Title("GroupBy - Task 10(month + year) - .Net Notation")]
	    [Description(
	        "Make the average annual activity statistics of clients by year and by month (when one month in different years has its own value)")]
	    public void Linq15()
	    {
	        var result = dataSource.Customers
                .SelectMany(order => order.Orders, (customer, order) => new
	            {
                    Date = order.OrderDate,
	                OrdersAmount = customer.Orders.Count()
                })
                .GroupBy(order => order.Date.Year, (year, orders) => new
	                {
	                    Year = year,
                        Month = orders.GroupBy(item => item.Date.Month, (month, items) => new
                        {
                            Month = month,
                            Total = items.Sum(item => item.OrdersAmount)
                        })
	                });	            

	        foreach (var x in result)
	        {
                Console.Write(x.Year);
	            foreach (var y in x.Month)
	            {
	                Console.WriteLine(y);
                }	            
	        }
	    }

	    [Category("Grouping Operators")]
	    [Title("GroupBy - Task 10(month + year) - Query Notation")]
	    [Description(
	        "Make the average annual activity statistics of clients by year and by month (when one month in different years has its own value)")]
	    public void Linq151()
	    {
	        var orders = from customer in dataSource.Customers
	            from order in customer.Orders
	            select new
	            {
	                Date = order.OrderDate,
	                OrdersAmount = customer.Orders.Count()
	            };

            var result = from order in orders
                         group order by order.Date.Year into yearGroup
                         select new
                         {
                             Year = yearGroup.Key,
                             Month = from item in yearGroup
                                     group item by item.Date.Month into monthGroup
                                     select new
                                     {
                                         Month = monthGroup.Key,
                                         Total = monthGroup.Sum(order => order.OrdersAmount)
                                     }
                         };

	        foreach (var x in result)
	        {
	            Console.Write(x.Year);
	            foreach (var y in x.Month)
	            {
	                Console.WriteLine(y);
	            }
	        }
        }
    }
}
