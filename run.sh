#!/bin/sh

set -e

echo "Checking database connection info..."
echo "Host: $DATABASE_HOST"
echo "Port: $DATABASE_PORT"
echo "User: $DATABASE_USER"
echo "Database: $DATABASE_NAME"

echo "Waiting for database to be ready..."
# Basitleştirilmiş veritabanı erişilebilirlik kontrolü
for i in $(seq 1 30); do
    if pg_isready -h $DATABASE_HOST -p $DATABASE_PORT -U $DATABASE_USER; then
        echo "Database is ready!"
        break
    fi
    echo "Waiting for database... ($i/30)"
    sleep 2
done

# Son bir kontrol daha yapalım
if ! pg_isready -h $DATABASE_HOST -p $DATABASE_PORT -U $DATABASE_USER; then
    echo "Could not connect to database after 30 attempts. Exiting."
    exit 1
fi

echo "Running database migrations..."
CONNECTION_STRING="postgres://$DATABASE_USER:$DATABASE_PASSWORD@$DATABASE_HOST:$DATABASE_PORT/$DATABASE_NAME?sslmode=$DATABASE_SSLMODE"
echo "Connection string: postgres://$DATABASE_USER:***@$DATABASE_HOST:$DATABASE_PORT/$DATABASE_NAME?sslmode=$DATABASE_SSLMODE"
migrate -path ./migrations -database "$CONNECTION_STRING" up

echo "Starting application server..."
exec ./server 