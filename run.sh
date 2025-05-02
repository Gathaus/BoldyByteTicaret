#!/bin/sh

set -e

echo "Installing golang-migrate for database migrations..."
go install -tags 'postgres' github.com/golang-migrate/migrate/v4/cmd/migrate@latest

echo "Waiting for database to be ready..."
# Simple check to ensure database is ready before running migrations
for i in $(seq 1 30); do
    if nc -z $DATABASE_HOST $DATABASE_PORT; then
        echo "Database is ready!"
        break
    fi
    echo "Waiting for database... ($i/30)"
    sleep 1
done

# Check if the connection was successful
if ! nc -z $DATABASE_HOST $DATABASE_PORT; then
    echo "Could not connect to database after 30 attempts. Exiting."
    exit 1
fi

echo "Running database migrations..."
CONNECTION_STRING="postgres://$DATABASE_USER:$DATABASE_PASSWORD@$DATABASE_HOST:$DATABASE_PORT/$DATABASE_NAME?sslmode=$DATABASE_SSLMODE"
$GOPATH/bin/migrate -path ./migrations -database "$CONNECTION_STRING" up

echo "Starting application server..."
exec ./server 