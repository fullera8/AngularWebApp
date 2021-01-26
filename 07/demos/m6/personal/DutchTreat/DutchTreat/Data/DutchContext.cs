using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;

namespace DutchTreat.Data
{
    public class DutchContext : DbContext 
    {
        //Constructor is needed so the db can get/set options in the context. 
        //In this example it just passes variables as is to and from the sql server.
        public DutchContext(DbContextOptions<DutchContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        //This can be used to modify properties of the context model on creation. 
        //Does not modify sql server directly but the incoming model built within the backend.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Adds base data (limited functionality) so we are not inserting nulls for non-null fields
            //Best for lists of proper fixed objects like state name lists.
            //Not good for populating related data or variable data like products in a store.
            modelBuilder.Entity<Order>()
                .HasData(new Order() 
                { 
                    Id = 1,
                    OrderDate = DateTime.UtcNow,
                    OrderNumber = "12345"
                });
        }
    }
}
