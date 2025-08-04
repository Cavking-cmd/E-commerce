using System;
using E_commerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace E_commerce.DataContext
{
    public class E_commerceDbContext : DbContext
    {
        public E_commerceDbContext(DbContextOptions<E_commerceDbContext> options) : base(options)
        { }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CouponOrder> CouponOrders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Admin user & role IDs
            var adminUserId = Guid.NewGuid();
            var adminProfileId = Guid.NewGuid();
            var adminRoleId = Guid.NewGuid();
            var adminUserRoleId = Guid.NewGuid();

            // Hashed password using BCrypt
            var adminPassword = BCrypt.Net.BCrypt.HashPassword("Admin@123");

            // Seed Admin Role
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = adminRoleId,
                    Name = "Admin"
                }
            );

            // Seed Admin User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = adminUserId,
                    Email = "admin@yopmail.com",
                    Password = adminPassword
                }
            );

            // Seed Admin UserProfile
            modelBuilder.Entity<UserProfile>().HasData(
                new UserProfile
                {
                    Id = adminProfileId,
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin1@yopmail.com",
                    PhoneNumber = "08000000000",
                    AddressLine = "123 Admin Street",
                    City = "Admin City",
                    State = "Admin State",
                    PostalCode = "100001",
                    Country = "Adminland",
                    UserId = adminUserId
                }
            );

            // Seed UserRole
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    Id = adminUserRoleId,
                    UserId = adminUserId,
                    RoleId = adminRoleId
                }
            );
        }

    }
}


