using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _dutchContext;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext dutchContect, ILogger<DutchRepository> logger)
        {
            _dutchContext = dutchContect;
            _logger = logger;
        }

        //Get all orders
        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
            {
                return _dutchContext.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .ToList(); //Basic order list for testing
            }
            else
            {
                return _dutchContext.Orders
                    .ToList(); //Basic order list for testing
            }

            
        }

        //Get order by id
        public Order GetOrderById(int id)
        {
            return _dutchContext.Orders
                .Where(o => o.Id == id)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(); //Basic order list for testing
        }

        //Get all products
        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("GetAllProducts called");
                return _dutchContext.Products
                    .OrderBy(p => p.Title)
                    .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error in GetAllProducts: {e}");
                return null;
            }
            
        }
        

        //Get all products by category
        public IEnumerable<Product> GetAllProductsByCategory(string category)
        {
            return _dutchContext.Products
                .Where(p => p.Category == category)
                .OrderBy(p => p.Title)
                .ToList();
        }

        public bool SaveAll()
        {
            return _dutchContext.SaveChanges() > 0;
        }

        public void AddEntity(object model)
        {
            _dutchContext.Add(model);
        }
    }
}
