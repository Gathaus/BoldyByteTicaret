package main

import (
	"context"
	"fmt"
	"log"
	"net/http"
	"os"
	"os/signal"
	"syscall"
	"time"

	"github.com/gin-gonic/gin"

	_ "github.com/mert-yagci/ecommerce-api/docs" // Import swagger docs
	"github.com/mert-yagci/ecommerce-api/internal/config"
	"github.com/mert-yagci/ecommerce-api/internal/logger"
	"github.com/mert-yagci/ecommerce-api/pkg/api/handlers"
	"github.com/mert-yagci/ecommerce-api/pkg/api/routes"
	"github.com/mert-yagci/ecommerce-api/pkg/application/service"
	"github.com/mert-yagci/ecommerce-api/pkg/infra/persistence"
)

// @title E-Commerce API
// @version 1.0
// @description A RESTful API for an E-Commerce application
// @termsOfService http://swagger.io/terms/

// @contact.name API Support
// @contact.email your-email@example.com

// @license.name MIT
// @license.url https://opensource.org/licenses/MIT

// @host localhost:8081
// @BasePath /
// @schemes http https

// @securityDefinitions.apikey BearerAuth
// @in header
// @name Authorization
// @description Type "Bearer" followed by a space and the JWT token.

func main() {
	// Load configuration
	cfg, err := config.NewConfig()
	if err != nil {
		log.Fatalf("Failed to load configuration: %v", err)
	}

	// Initialize logger
	loggerInstance := logger.Setup(cfg.Server.Env)

	// Initialize database
	db, err := persistence.InitDB(cfg)
	if err != nil {
		loggerInstance.WithError(err).Fatal("Failed to connect to database")
	}

	// Run database migrations and seed data
	if err := persistence.MigrateDB(db); err != nil {
		loggerInstance.WithError(err).Fatal("Failed to run database migrations")
	}

	// Initialize repositories
	productRepo := persistence.NewProductRepository(db)
	categoryRepo := persistence.NewCategoryRepository(db)
	userRepo := persistence.NewUserRepository(db)

	// Initialize services
	productService := service.NewProductService(productRepo)
	categoryService := service.NewCategoryService(categoryRepo)
	userService := service.NewUserService(userRepo)
	authService := service.NewAuthService(userRepo, cfg)

	// Initialize API handlers
	productHandler := handlers.NewProductHandler(productService, categoryService)
	authHandler := handlers.NewAuthHandler(authService, userService)

	// Set Gin mode based on environment
	if cfg.Server.Env == "production" {
		gin.SetMode(gin.ReleaseMode)
	}

	// Initialize Gin router
	router := gin.New()

	// Add middleware
	router.Use(gin.Recovery())

	// Configure API routes
	routes.SetupRouter(router, productHandler, authHandler, authService)

	// Note: Static routes and NoRoute handler are now configured in routes.SetupRouter

	// Create HTTP server
	srv := &http.Server{
		Addr:    fmt.Sprintf(":%d", cfg.Server.Port),
		Handler: router,
	}

	// Start server in a goroutine
	go func() {
		loggerInstance.WithField("port", cfg.Server.Port).Info("Starting server")
		if err := srv.ListenAndServe(); err != nil && err != http.ErrServerClosed {
			loggerInstance.WithError(err).Fatal("Failed to start server")
		}
	}()

	// Wait for interrupt signal to gracefully shutdown the server
	quit := make(chan os.Signal, 1)
	signal.Notify(quit, syscall.SIGINT, syscall.SIGTERM)
	<-quit

	loggerInstance.Info("Shutting down server...")

	// Create a deadline for server shutdown
	ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
	defer cancel()

	// Attempt graceful shutdown
	if err := srv.Shutdown(ctx); err != nil {
		loggerInstance.WithError(err).Fatal("Server forced to shutdown")
	}

	loggerInstance.Info("Server exited gracefully")
}
