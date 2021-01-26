using DutchTreat.Data.Entities;
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

        public DutchRepository(DutchContext dutchContect)
        {
            _dutchContext = dutchContect;
        }

        //Get all products
        public IEnumerable<Product> GetAllProducts()
        {
            return _dutchContext.Products
                .OrderBy(p => p.Title)
                .ToList();
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
    }
}
