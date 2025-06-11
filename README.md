# E-commerce Application

## Overview

This is an e-commerce application managing users, customers, vendors, products, categories, orders, carts, wishlists, coupons, and reviews.

### Entity Relationships

- **User**: Has required Email and Password. Many-to-many relationship with Roles via UserRoles. One-to-one relationships with UserProfile, Customer, and Vendor.
- **Customer**: Has a UserId and navigation to User. One-to-one relationship with Wishlist. One-to-many relationships with Carts, Orders, Reviews, and ShippingAddresses.
- **Wishlist**: Has a CustomerId and navigation to Customer. One-to-many relationship with WishlistItems.
- **WishlistItem**: Has a WishlistId and navigation to Wishlist. Has a ProductId and navigation to Product.
- **Cart**: Has a Name, CustomerId, and navigation to Customer. One-to-many relationships with CartItems and Coupons.
- **CartItem**: Has CartId and navigation to Cart. Has ProductId and navigation to Product. Has Quantity, ProductName, and PricePerUnit.
- **Order**: Has Name, OrderDate, TotalPrice, Status (enum), CustomerId and navigation to Customer. One-to-many relationship with OrderItems. Many-to-many relationship with Coupons.
- **OrderItem**: Has Quantity, PriceAtPurchase, OrderId and navigation to Order, ProductId and navigation to Product.

### Services and Repositories

- The application uses a repository pattern with a generic BaseRepository<T> providing common data access methods.
- Specific repositories extend BaseRepository and implement interfaces to add entity-specific data access methods, often including eager loading of related entities using Entity Framework Core's Include.
- Services implement interfaces and use repositories and a UnitOfWork to perform business logic, validation, and data manipulation. They return standardized response DTOs.
- The pattern is consistent across entities: repositories handle data access, services handle business logic and validation, and controllers handle HTTP requests.

### Application Purpose

- Users can have roles and profiles, and can be customers or vendors.
- Customers have carts and wishlists, place orders, and can write reviews.
- Carts and wishlists contain items related to products.
- Orders contain order items and can have coupons applied.
- The application uses Entity Framework Core for data access with a layered architecture separating concerns into entities, repositories, services, and controllers.

## Upcoming Features

- Implementation of Wishlist and WishlistItem services and repositories with proper one-to-many relationships and eager loading.
- Full CRUD operations with validation and error handling.
- Comprehensive testing covering critical paths and edge cases.
