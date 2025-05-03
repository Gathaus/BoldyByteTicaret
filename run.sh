#!/bin/sh

set -e

echo "Checking database connection info..."
echo "Host: ${DATABASE_HOST:-not set}"
echo "Port: ${DATABASE_PORT:-not set}"
echo "User: ${DATABASE_USER:-not set}"
echo "Database: ${DATABASE_NAME:-not set}"

# Check if required environment variables are set
if [ -z "$DATABASE_HOST" ] || [ -z "$DATABASE_PORT" ] || [ -z "$DATABASE_USER" ] || [ -z "$DATABASE_PASSWORD" ] || [ -z "$DATABASE_NAME" ]; then
    echo "ERROR: Required database environment variables are not set."
    echo "Please make sure the following variables are set:"
    echo "- DATABASE_HOST"
    echo "- DATABASE_PORT"
    echo "- DATABASE_USER"
    echo "- DATABASE_PASSWORD"
    echo "- DATABASE_NAME"
    
    # Try to connect to the Render internal PostgreSQL service directly
    echo "Attempting to use Render's internal PostgreSQL service..."
    
    # Set default values for Render PostgreSQL service
    DATABASE_HOST=${DATABASE_HOST:-localhost}
    DATABASE_PORT=${DATABASE_PORT:-5432}
    DATABASE_USER=${DATABASE_USER:-postgres}
    DATABASE_NAME=${DATABASE_NAME:-postgres}
    DATABASE_SSLMODE=${DATABASE_SSLMODE:-prefer}
    
    echo "Using fallback values:"
    echo "Host: $DATABASE_HOST"
    echo "Port: $DATABASE_PORT"
    echo "User: $DATABASE_USER"
    echo "Database: $DATABASE_NAME"
    echo "SSL Mode: $DATABASE_SSLMODE"
fi

echo "Waiting for database to be ready..."
# Simplified database connection check
for i in $(seq 1 30); do
    if pg_isready -h "${DATABASE_HOST}" -p "${DATABASE_PORT}"; then
        echo "Database is ready!"
        break
    fi
    echo "Waiting for database... ($i/30)"
    sleep 2
done

# One more check
if ! pg_isready -h "${DATABASE_HOST}" -p "${DATABASE_PORT}"; then
    echo "Could not connect to database after 30 attempts. Exiting."
    exit 1
fi

echo "Running database migrations..."
CONNECTION_STRING="postgres://${DATABASE_USER}:${DATABASE_PASSWORD}@${DATABASE_HOST}:${DATABASE_PORT}/${DATABASE_NAME}?sslmode=${DATABASE_SSLMODE:-require}"
echo "Connection string: postgres://${DATABASE_USER}:***@${DATABASE_HOST}:${DATABASE_PORT}/${DATABASE_NAME}?sslmode=${DATABASE_SSLMODE:-require}"
migrate -path ./migrations -database "$CONNECTION_STRING" up

echo "Starting application server..."
exec ./server 