using ClassLibrary.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importer.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=EntityDB")
        {
            //Database.SetInitializer(new ProductDatabaseInitializer());
           Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductUrl> ProductUrls { get; set; }
        public DbSet<Feature> Features { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
            .HasOptional(p => p.ProductImage)
            .WithRequired(x => x.Product);
            
            //Disable auto increment on CategoryId
            modelBuilder.Entity<Category>()
               .Property(p => p.CategoryId)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


            //Disable auto increment on FeatureId
            modelBuilder.Entity<Feature>()
               .Property(p => p.FeatureId)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
