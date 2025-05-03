#!/bin/sh

set -e

echo "Setting up hardcoded database configuration..."

# Hardcoded Aiven PostgreSQL credentials
export DATABASE_HOST="pg-126bf3ed-rizamertyagci-dd43.l.aivencloud.com"
export DATABASE_PORT="25406"
export DATABASE_USER="avnadmin"
export DATABASE_PASSWORD="AVNS_q6IcUYO6K9TNpZkKTKX"
export DATABASE_NAME="ecommerce"
export DATABASE_SSLMODE="require"

echo "Using database configuration:"
echo "Host: $DATABASE_HOST"
echo "Port: $DATABASE_PORT"
echo "User: $DATABASE_USER"
echo "Database: $DATABASE_NAME"
echo "SSL Mode: $DATABASE_SSLMODE"

echo "Waiting for database to be ready..."
# Simplified database connection check
for i in $(seq 1 10); do
    if pg_isready -h "${DATABASE_HOST}" -p "${DATABASE_PORT}"; then
        echo "Database is ready!"
        break
    fi
    echo "Waiting for database... ($i/10)"
    sleep 3
done

# One more check
if ! pg_isready -h "${DATABASE_HOST}" -p "${DATABASE_PORT}"; then
    echo "Could not connect to database after 10 attempts. Exiting."
    exit 1
fi

echo "Running database migrations..."
CONNECTION_STRING="postgres://${DATABASE_USER}:${DATABASE_PASSWORD}@${DATABASE_HOST}:${DATABASE_PORT}/${DATABASE_NAME}?sslmode=${DATABASE_SSLMODE}"
echo "Connection string: postgres://${DATABASE_USER}:***@${DATABASE_HOST}:${DATABASE_PORT}/${DATABASE_NAME}?sslmode=${DATABASE_SSLMODE}"
migrate -path ./migrations -database "$CONNECTION_STRING" up

echo "Starting application server..."
exec ./server 