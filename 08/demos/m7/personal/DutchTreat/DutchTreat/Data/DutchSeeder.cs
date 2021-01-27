using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _context;
        private readonly IWebHostEnvironment _hosting; //Ensures we have path to root no matter environment

        public DutchSeeder(DutchContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }

        //Populate DB with seed (sample) data if there is no existing data
        public void Seed()
        {
            //Make sure DB exists
            _context.Database.EnsureCreated();

            //If there are not any products in the DB
            if (!_context.Products.Any())
            {
                //Create sample product data
                var path = Path.Combine(_hosting.ContentRootPath,"Data/art.json");
                var json = File.ReadAllText(path);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _context.Products.AddRange(products);//shortcut to add a list of items

                //Create sample order data
                var order = _context.Orders.Where(o => o.Id == 1).FirstOrDefault();
                if (order != null)
                {
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    };
                }

                //Saves the default seeds from the model to the sql server
                _context.SaveChanges();
            }
        }
    }
}
