using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace EShopping.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=PurchaseModel1")
        {
        }

        public virtual DbSet<Purchase> Purchases { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}