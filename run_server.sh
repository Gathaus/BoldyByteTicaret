#!/bin/bash

# Change to ecommerce-api directory
cd "$(dirname "$0")"

# Run the server
go run cmd/app/main.go 