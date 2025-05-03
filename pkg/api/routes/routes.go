package routes

import (
	"net/http"

	"github.com/gin-gonic/gin"
	"github.com/mert-yagci/ecommerce-api/pkg/api/handlers"
	"github.com/mert-yagci/ecommerce-api/pkg/api/middleware"
	"github.com/mert-yagci/ecommerce-api/pkg/domain/service"
	swaggerFiles "github.com/swaggo/files"
	ginSwagger "github.com/swaggo/gin-swagger"
)

// SetupRouter configures the API routes
func SetupRouter(
	router *gin.Engine,
	productHandler *handlers.ProductHandler,
	authHandler *handlers.AuthHandler,
	authService service.AuthService,
) {
	// Set up debug mode to see more details
	gin.SetMode(gin.DebugMode)

	// Initialize static handler
	staticHandler := handlers.NewStaticHandler()

	// Static file serving
	setupStaticRoutes(router, staticHandler)

	// API v1 group - all API routes are prefixed with /api/v1
	v1 := router.Group("/api/v1")

	// Swagger documentation
	router.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerFiles.Handler))
	v1.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerFiles.Handler))

	// Public routes
	setupPublicRoutes(v1, productHandler, authHandler)

	// Protected routes (require authentication)
	authMiddleware := middleware.AuthMiddleware(authService)
	setupProtectedRoutes(v1, productHandler, authHandler, authMiddleware)

	// No route handler
	router.NoRoute(func(c *gin.Context) {
		c.JSON(http.StatusNotFound, gin.H{"message": "Page not found"})
	})
}

// setupStaticRoutes configures routes for static files
func setupStaticRoutes(router *gin.Engine, staticHandler *handlers.StaticHandler) {
	// Serve home page at root URL
	router.GET("/", staticHandler.Home)

	// Serve inner pages
	router.GET("/about", staticHandler.About)
	router.GET("/contact", staticHandler.Contact)
	router.GET("/login", staticHandler.Login)
	router.GET("/register", staticHandler.Register)
	router.GET("/profile", staticHandler.Profile)
	router.GET("/products", staticHandler.Products)
	router.GET("/products-layout-2", staticHandler.ProductsLayout2)
	router.GET("/cart", staticHandler.Cart)
	router.GET("/checkout", staticHandler.Checkout)
	router.GET("/single-product", staticHandler.SingleProduct)
	router.GET("/single-product-pay", staticHandler.SingleProductPay)

	// Serve static assets
	router.Static("/assets", "./web/static/pages/assets")
	router.Static("/inner_pages/assets", "./web/static/pages/assets")
	router.Static("/common", "./web/static/common")
}

// setupPublicRoutes sets up the non-authenticated routes
func setupPublicRoutes(v1 *gin.RouterGroup, productHandler *handlers.ProductHandler, authHandler *handlers.AuthHandler) {
	// Auth routes
	v1.POST("/auth/register", authHandler.Register)
	v1.POST("/auth/login", authHandler.Login)

	// Product routes
	v1.GET("/products", productHandler.GetProducts)
	v1.GET("/products/:id", productHandler.GetProductByID)
	v1.GET("/categories", productHandler.GetCategories)
	v1.GET("/categories/:id/products", productHandler.GetProductsByCategory)
}

// setupProtectedRoutes sets up the authenticated routes
func setupProtectedRoutes(v1 *gin.RouterGroup, productHandler *handlers.ProductHandler, authHandler *handlers.AuthHandler, authMiddleware gin.HandlerFunc) {
	// User routes (require authentication)
	userGroup := v1.Group("/users")
	userGroup.Use(authMiddleware)
	{
		userGroup.GET("/me", authHandler.GetProfile)
	}

	// Product management routes (require authentication)
	productGroup := v1.Group("/products")
	productGroup.Use(authMiddleware)
	{
		productGroup.POST("/", productHandler.CreateProduct)
		productGroup.PUT("/:id", productHandler.UpdateProduct)
		productGroup.DELETE("/:id", productHandler.DeleteProduct)
	}
}
