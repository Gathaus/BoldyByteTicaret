#!/bin/bash

# Exit on error
set -e

echo "Setting up PostgreSQL database for e-commerce API..."

# Check if PostgreSQL is running
if ! pg_isready > /dev/null 2>&1; then
  echo "Error: PostgreSQL is not running. Please start PostgreSQL and try again."
  exit 1
fi

# Create database if it doesn't exist
if ! psql -lqt | cut -d \| -f 1 | grep -qw ecommerce; then
  echo "Creating database 'ecommerce'..."
  createdb ecommerce
else
  echo "Database 'ecommerce' already exists."
fi

# Install migrate tool if not installed
if ! command -v migrate &> /dev/null; then
  echo "Installing golang-migrate..."
  go install -tags 'postgres' github.com/golang-migrate/migrate/v4/cmd/migrate@latest
fi

# Run migrations
echo "Running database migrations..."
migrate -path ../migrations -database "postgres://postgres:postgres@localhost:5432/ecommerce?sslmode=disable" up

echo "Database setup completed successfully!" 