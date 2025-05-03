FROM golang:1.23-alpine AS builder

# Set working directory
WORKDIR /app

# Copy go mod and sum files
COPY go.mod go.sum ./

# Download Go modules
RUN go mod download

# Install golang-migrate during build
RUN go install -tags 'postgres' github.com/golang-migrate/migrate/v4/cmd/migrate@latest

# Copy the source code
COPY . .

# Build the application
RUN CGO_ENABLED=0 GOOS=linux go build -a -installsuffix cgo -o server ./cmd/app/main.go

# Create a minimal production image
FROM alpine:latest

# Install ca-certificates, tzdata, and netcat for health checks
RUN apk --no-cache add ca-certificates tzdata netcat-openbsd

# Set working directory
WORKDIR /app

# Copy the binary from the builder stage
COPY --from=builder /app/server .
COPY --from=builder /app/migrations ./migrations
COPY --from=builder /app/config ./config
COPY --from=builder /app/web ./web
COPY --from=builder /app/run.sh .
COPY --from=builder /go/bin/migrate /usr/local/bin/migrate

# Make sure run.sh is executable
RUN chmod +x run.sh

# Create a non-root user and set permissions
RUN adduser -D appuser && \
    chown -R appuser:appuser /app
USER appuser

# Set environment variables
ENV TZ=UTC
ENV PATH="/app:/usr/local/bin:${PATH}"

# Expose port
EXPOSE 8080

# Run the application using the run script
CMD ["./run.sh"] 