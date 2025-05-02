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

## Deployment on Render.com

This project is configured for easy deployment on [Render.com](https://render.com/). The `render.yaml` file in the root directory provides the infrastructure as code needed to deploy all services.

### Features of the Render Deployment

- **Web Service**: The main API application
- **PostgreSQL Database**: For storing all application data
- **Redis**: For caching and session management
- **Automatic Migrations**: Database migrations run automatically on deployment

### Deployment Steps

1. Sign up for a [Render account](https://dashboard.render.com/register) if you don't have one.

2. Fork or clone this repository to your GitHub account.

3. In the Render dashboard, click "New" and select "Blueprint" from the dropdown.

4. Connect your GitHub account and select this repository.

5. Render will detect the `render.yaml` file and show you the resources that will be created.

6. Click "Apply" to start the deployment.

7. Render will create all the necessary services and provide you with URLs to access them.

### Environment Variables

The application is configured to use environment variables for configuration in production. Render automatically sets these up based on the `render.yaml` file.

### Manual Deployment (Alternative Method)

If you prefer to set up services manually:

1. In the Render dashboard, create a new PostgreSQL database.

2. Create a new Web Service, selecting the "Docker" environment.

3. Connect your GitHub repository and set the following environment variables:
   - `DATABASE_HOST`: Your Render PostgreSQL hostname
   - `DATABASE_PORT`: Usually 5432
   - `DATABASE_USER`: From your Render PostgreSQL credentials
   - `DATABASE_PASSWORD`: From your Render PostgreSQL credentials
   - `DATABASE_NAME`: Your database name
   - `DATABASE_SSLMODE`: Set to "require" for Render
   - `JWT_SECRET`: A secure random string
   - `SERVER_ENV`: Set to "production"

4. Deploy the service.

### Cost Considerations

The deployment is configured to use Render's free tier, which is suitable for testing but has limitations:
- Free PostgreSQL databases have a 90-day lifespan
- Free services spin down after periods of inactivity

For production use, consider upgrading to paid plans. 