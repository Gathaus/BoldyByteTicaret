FROM golang:1.19-alpine AS builder

# Set working directory
WORKDIR /app

# Copy the source code
COPY . .

# Download Go modules
RUN go mod download

# Build the application
RUN go build -o server ./cmd/app/main.go

# Create a minimal production image
FROM alpine:latest

# Set working directory
WORKDIR /app

# Copy the binary from the builder stage
COPY --from=builder /app/server .
COPY --from=builder /app/web/static ./web/static

# Expose port
EXPOSE 8080

# Run the application
CMD ["./server"] 