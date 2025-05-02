# BoldByteTicaret E-Commerce API Setup Guide

This document provides step-by-step instructions to set up and run the BoldByteTicaret E-Commerce API on your local machine.

## Prerequisites

1. **Go 1.23+** - The project is built with Go 1.23
2. **PostgreSQL** - Required for the database
3. **Git** - For version control (optional but recommended)

## Installation Steps

### 1. Install PostgreSQL

#### macOS (using Homebrew)
```bash
# Install Homebrew if you don't have it
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

# Install PostgreSQL
brew install postgresql@14

# Start PostgreSQL
brew services start postgresql@14

# Create a default postgres user (if needed)
createuser -s postgres
```

#### Linux (Ubuntu/Debian)
```bash
# Install PostgreSQL
sudo apt update
sudo apt install postgresql postgresql-contrib

# Start PostgreSQL
sudo systemctl start postgresql
sudo systemctl enable postgresql

# Set password for postgres user
sudo -u postgres psql -c "ALTER USER postgres PASSWORD 'postgres';"
```

### 2. Install Go Migrate Tool

```bash
# Install the golang-migrate CLI tool
go install -tags 'postgres' github.com/golang-migrate/migrate/v4/cmd/migrate@latest

# Make sure $GOPATH/bin is in your PATH
export PATH=$PATH:$(go env GOPATH)/bin
```

### 3. Configure the Project

The configuration file is located at `config/config.yaml`. Update the database connection details if necessary:

```yaml
database:
  host: "localhost"  # Change if your PostgreSQL is on a different host
  port: 5432         # Default PostgreSQL port
  user: "postgres"   # Default PostgreSQL user
  password: "postgres" # Change to your PostgreSQL password
  dbname: "ecommerce" # Database name that will be created
  sslmode: "disable"  # For local development
```

### 4. Set Up the Database

```bash
# Make the setup script executable
chmod +x scripts/setup_db.sh

# Run the setup script
cd scripts && ./setup_db.sh
```

### 5. Download Dependencies

```bash
# Download all Go dependencies
go mod download
```

### 6. Run the Application

```bash
# Make the run script executable
chmod +x run_server.sh

# Run the application
./run_server.sh
```

The API will be available at http://localhost:8080/api/v1

## API Documentation

Once the application is running, you can access the Swagger API documentation at:

```
http://localhost:8080/api/v1/swagger/index.html
```

## Common Issues

### PostgreSQL Connection Issues

If you encounter PostgreSQL connection problems, check:
1. PostgreSQL service is running
2. The connection details in the `config/config.yaml` file are correct
3. Your PostgreSQL user has the correct permissions

### Migration Errors

If you encounter migration errors:
1. Check if the database exists
2. Ensure the PostgreSQL user has sufficient privileges
3. Check if the migrate tool is installed correctly and in your PATH

### Go Version Mismatch

This project requires Go 1.23 or higher. Check your Go version with:
```bash
go version
```

If your version is lower, update to the latest Go version. 