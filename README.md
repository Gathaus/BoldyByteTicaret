# E-Commerce App

An e-commerce web application built with ASP.NET Core MVC, following clean architecture principles.

## Features

- **Products Management**: CRUD operations for products and categories
- **User Authentication**: Registration, login, and JWT-based authentication
- **User Management**: Profile management and role-based authorization
- **Shopping Cart**: Add, update, remove items from cart
- **Order Processing**: Create and manage orders
- **Documentation**: Swagger API documentation

## Architecture

The project follows a clean/onion architecture approach with the following layers:

- **Domain Layer**: Core business entities, interfaces, and business rules
- **Application Layer**: Application services implementing business logic
- **Infrastructure Layer**: External concerns (database, messaging, etc.)
- **API Layer**: HTTP controllers, DTOs, and routing

## Technology Stack

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Authentication**: JWT (JSON Web Tokens)
- **API Documentation**: Swagger

## Project Structure

```
├── ECommerceApp.Domain/             # Domain layer (entities, interfaces)
├── ECommerceApp.Application/        # Application layer (services)
├── ECommerceApp.Infrastructure/     # Infrastructure layer (data access)
└── ECommerceApp.Web/                # Web layer (controllers, views)
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server

### Installation

1. Clone the repository:

```bash
git clone https://github.com/yourusername/ECommerceApp.git
cd ECommerceApp
```

2. Update the connection string in `ECommerceApp.Web/appsettings.json` to match your SQL Server instance.

3. Run database migrations:

```bash
cd ECommerceApp.Web
dotnet ef database update
```

4. Run the application:

```bash
dotnet run
```

The API will be available at https://localhost:7001 and the Swagger UI at https://localhost:7001/swagger.

## API Endpoints

### Authentication

- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login and receive JWT token
- `GET /api/auth/profile` - Get current user profile
- `PUT /api/auth/profile` - Update user profile
- `POST /api/auth/change-password` - Change user password

### Products

- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `GET /api/products/category/{categoryId}` - Get products by category
- `GET /api/products/search?term={searchTerm}` - Search products
- `GET /api/products/featured?count={count}` - Get featured products
- `POST /api/products` - Create a new product (Admin)
- `PUT /api/products/{id}` - Update a product (Admin)
- `DELETE /api/products/{id}` - Delete a product (Admin)

### Cart

- `GET /api/cart` - Get current user's cart
- `POST /api/cart` - Add item to cart
- `PUT /api/cart/items/{id}` - Update cart item
- `DELETE /api/cart/items/{id}` - Remove cart item
- `DELETE /api/cart` - Clear cart

### Orders

- `GET /api/orders` - Get user's orders
- `GET /api/orders/{id}` - Get order details
- `POST /api/orders` - Create a new order
- `PUT /api/orders/{id}/cancel` - Cancel an order
- `PUT /api/orders/{id}/status` - Update order status (Admin)

## Default Users

Upon first run, the system will create:

- Admin: admin@example.com / Admin@123

## License

This project is licensed under the MIT License - see the LICENSE file for details. 