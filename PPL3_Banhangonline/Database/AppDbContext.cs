using PPL3_Banhangonline.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PPL3_Banhangonline.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetails");
            modelBuilder.Entity<Payment>().ToTable("Payments");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Account>().ToTable("Account");

            // AppDbContext.cs

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Order)
                .WithMany()
                .HasForeignKey(r => r.OrderID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasIndex(r => new { r.CustomerID, r.ProductID, r.OrderID })
                .IsUnique();

            modelBuilder.Entity<RescueRegistration>()
                .HasOne(r => r.Campaign)
                .WithMany(c => c.Registrations)
                .HasForeignKey(r => r.CampaignID)
                .OnDelete(DeleteBehavior.NoAction); // Bắt buộc là NoAction

            modelBuilder.Entity<RescueRegistration>()
                .HasOne(r => r.Customer)
                .WithMany()
                .HasForeignKey(r => r.CustomerID)
                .OnDelete(DeleteBehavior.NoAction); // Bắt buộc là NoAction
            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderID, od.ProductID });

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductID);
       
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<CartItem>()
            //    .Property(c => c.Price)
            //    .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Price>()
                .Property(p => p.Value)
                .HasPrecision(18, 2);

         modelBuilder.Entity<CartItem>()
        .HasOne(ci => ci.Product)
        .WithMany(p => p.CartItems) // Đảm bảo trong class Product có: public ICollection<CartItem> CartItems { get; set; }
        .HasForeignKey(ci => ci.ProductID)
        .OnDelete(DeleteBehavior.Restrict); // Đổi từ Cascade sang Restrict để tránh vòng lặp xóa


         modelBuilder.Entity<OrderDetail>()
        .HasOne(od => od.Product)
        .WithMany(p => p.OrderDetails)
        .HasForeignKey(od => od.ProductID)
        .OnDelete(DeleteBehavior.Restrict);

            // Thêm vào trong method OnModelCreating
         modelBuilder.Entity<RescueCampaign>()
        .Property(r => r.Price)
        .HasPrecision(18, 2);
        }

        

        
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Account> Account { get; internal set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Cart> Carts { get; set; }
        
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RescueCampaign> RescueCampaigns { get; set; }
        public DbSet<RescueRegistration> RescueRegistrations { get; set; }

        public DbSet<Review> Reviews { get; set; }
        public object Accounts { get; internal set; }
    }
}
