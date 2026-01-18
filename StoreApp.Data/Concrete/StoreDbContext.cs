using System.Collections.Specialized;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace StoreApp.Data.Concrete;

    public class StoreDbContext:IdentityDbContext<AppUser,AppRole,string>
    {
        public StoreDbContext(DbContextOptions<StoreDbContext>options):base(options)
        {
        
        }
         public DbSet<Product> Products => Set<Product>();
          public DbSet<Order> Orders => Set<Order>();
         public DbSet<Category> Categories=>Set<Category>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                .HasMany(e => e.Categories)
                .WithMany(e => e.Products)
                .UsingEntity<ProductCategory>();


            modelBuilder
                .Entity<Category>()
                .HasIndex(u => u.Url)
                    .IsUnique();

            modelBuilder.Entity<Product>().HasData(
                new List<Product>()
                {
                    new(){Id=1,Name="Samsungs24", Price=50000,Description="Akılı telefon"},
                    new(){Id=2,Name="Samsungs25", Price=60000,Description="Akılı telefon"},
                    new(){Id=3,Name="Samsungs26", Price=70000,Description="Akılı telefon"},
                    new(){Id=4,Name="Samsungs27", Price=80000,Description="Akılı telefon"},
                    new(){Id=5,Name="Samsungs28", Price=90000,Description="Akılı telefon"},
                    new(){Id=6,Name="Samsungs29", Price=100000,Description="Akılı telefon"},
                    
                }
            );
            modelBuilder.Entity<Category>().HasData(
                new List<Category>()
                {
                    new(){Id=1,Name="Telefon",Url="telefon"},
                    new(){Id=2,Name="Elektironik",Url="elektironik"},
                    new(){Id=3,Name="Beyaz Eşya",Url="beyaz-esya"} //extension methot,slug dotnet
                }
            );
            modelBuilder.Entity<ProductCategory>().HasData(
                new List<ProductCategory>()
                {
                    new(){ProductId=1,CategoryId=1},
                    new(){ProductId=1,CategoryId=2},
                    new(){ProductId=2,CategoryId=1},
                    new(){ProductId=3,CategoryId=1},
                    new(){ProductId=4,CategoryId=1},
                    new(){ProductId=5,CategoryId=2},
                    new(){ProductId=6,CategoryId=2},
                    

                }
            );
        }
    }