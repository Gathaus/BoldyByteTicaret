#!/bin/bash
set -e

MIGRATE_CMD="migrate -path ./migrations -database"

# Get database configuration from environment variables or use the provided ones
DB_HOST=${DB_HOST:-pg-126bf3ed-rizamertyagci-dd43.l.aivencloud.com}
DB_PORT=${DB_PORT:-25406}
DB_USER=${DB_USER:-avnadmin}
DB_PASSWORD=${DB_PASSWORD:-AVNS_q6IcUYO6K9TNpZkKTKX}
DB_NAME=${DB_NAME:-ecommerce}
DB_SSLMODE=${DB_SSLMODE:-require}

# Construct the database URL
DB_URL="postgres://${DB_USER}:${DB_PASSWORD}@${DB_HOST}:${DB_PORT}/${DB_NAME}?sslmode=${DB_SSLMODE}"

# Command can be "up", "down", "drop", "version", "force VERSION", etc.
COMMAND=$1

if [ -z "$COMMAND" ]; then
    echo "Usage: $0 <command> [options]"
    echo "Examples:"
    echo "  $0 up                  # Apply all migrations"
    echo "  $0 down                # Rollback last migration"
    echo "  $0 down 2              # Rollback last 2 migrations"
    echo "  $0 drop                # Drop all tables"
    echo "  $0 version             # Show current migration version"
    echo "  $0 force VERSION       # Force to specific VERSION"
    exit 1
fi

# Execute the migration command
echo "Running migration command: $COMMAND"
echo "Database URL: ${DB_URL}"
${MIGRATE_CMD} ${DB_URL} ${COMMAND} ${2:-} 