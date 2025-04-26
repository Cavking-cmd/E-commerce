using System;
using E_commerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace E_commerce.DataContext
{
    public class E_commerceDbContext :DbContext
    {
        public E_commerceDbContext(DbContextOptions<E_commerceDbContext> options) : base(options)
        {}

        public DbSet<Cart>Carts { get; set; }
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
        public DbSet<Wishlist>Wishlists { get; set; }


         
    }
}

