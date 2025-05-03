#!/bin/bash
set -e

# Directory where this script is located
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )"

# Go to project root directory
cd "${SCRIPT_DIR}/.."

echo "Installing golang-migrate if not already installed..."
if ! command -v migrate &> /dev/null; then
    go install -tags 'postgres' github.com/golang-migrate/migrate/v4/cmd/migrate@latest
fi

echo "Dropping all tables (if any)..."
"${SCRIPT_DIR}/migrate.sh" drop -f || true

echo "Running all migrations..."
"${SCRIPT_DIR}/migrate.sh" up

echo "Verifying migration version..."
"${SCRIPT_DIR}/migrate.sh" version

echo "Database has been reset and all migrations have been applied!"
echo "Seed data has been loaded in migration 000009." 