# E-Commerce API

This is a RESTful API backend for an e-commerce website, built using Go (Golang) with clean architecture principles.

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
- **API Layer**: HTTP handlers, DTOs, and routing

## Technology Stack

- **Language**: Go 1.23+
- **Web Framework**: Gin
- **Database**: PostgreSQL
- **ORM**: GORM
- **Database Migrations**: golang-migrate
- **Authentication**: JWT (JSON Web Tokens)
- **Configuration**: Viper
- **Logging**: Logrus
- **API Documentation**: Swagger
- **Observability**: OpenTelemetry

## Getting Started

### Prerequisites

- Go 1.23+
- PostgreSQL
- Docker (optional)

### Installation

1. Clone the repository:

```bash
git clone https://github.com/mert-yagci/ecommerce-api.git
cd ecommerce-api
```

2. Install dependencies:

```bash
go mod download
```

3. Configure the application:

Copy the example configuration and modify it as needed:

```bash
cp config/config.example.yaml config/config.yaml
```

4. Run database migrations:

```bash
# Install the migrate tool if you don't have it
go install -tags 'postgres' github.com/golang-migrate/migrate/v4/cmd/migrate@latest

# Run migrations
./scripts/migrate.sh up
```

5. Run the application:

```bash
go run ./cmd/app/main.go
```

The API will be available at http://localhost:8080.

### Docker

To run the application using Docker:

```bash
# Build the image
docker build -t ecommerce-api .

# Run the container
docker run -p 8080:8080 -e DB_HOST=your-db-host ecommerce-api
```

## API Documentation

Once the application is running, you can access the Swagger API documentation at:

```
http://localhost:8080/api/v1/swagger/index.html
```

## Project Structure

```
├── cmd/                  # Application entry points
│   └── app/              # Main application
├── pkg/                  # Public packages
│   ├── domain/           # Core domain
│   │   ├── entity/       # Domain entities
│   │   ├── repository/   # Repository interfaces
│   │   └── service/      # Service interfaces
│   ├── application/      # Application services
│   │   └── service/      # Service implementations
│   ├── infra/            # Infrastructure layer
│   │   ├── persistence/  # Database implementations
│   │   ├── messaging/    # Messaging implementations
│   │   └── auth/         # Authentication implementations
│   └── api/              # API layer
│       ├── handlers/     # HTTP handlers
│       ├── dto/          # Data Transfer Objects
│       ├── middleware/   # HTTP middleware
│       └── routes/       # Route definitions
├── internal/             # Private packages
│   ├── config/           # Configuration
│   ├── errors/           # Error handling
│   └── logger/           # Logging
├── config/               # Configuration files
├── migrations/           # Database migrations
├── docs/                 # Documentation
├── scripts/              # Helper scripts
└── web/                  # Web assets
    └── build/            # Static files for serving
```

## License

This project is licensed under the MIT License - see the LICENSE file for details. 