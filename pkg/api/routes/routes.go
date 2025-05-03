package routes

import (
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
	// Initialize the static handler with needed services
	staticHandler := handlers.NewStaticHandler(
		productHandler.GetProductService(),
		productHandler.GetCategoryService(),
	)

	// Set up debug mode to see more details
	gin.SetMode(gin.DebugMode)

	// Add the virtual asset middleware early in the chain
	// This will handle specific asset paths that might be missing
	router.Use(handlers.VirtualAssetMiddleware())

	// Define base paths for frontend assets
	swooHtmlBase := "/Users/mert.yagci/myDevLogs/ETİCARET/frontend/swoo_html"
	swooHtml2Base := "/Users/mert.yagci/myDevLogs/ETİCARET/frontend/swoo_html2"

	// Create a map to register file directories for easy reference
	assetPaths := map[string]string{
		// Main asset paths
		"/common/assets":          swooHtmlBase + "/common/assets",
		"/home_electronic/assets": swooHtmlBase + "/home_electronic/assets",
		"/inner_pages/assets":     swooHtml2Base + "/inner_pages/assets",

		// Additional entry points
		"/assets":                           swooHtmlBase + "/home_electronic/assets",
		"/swoo_html/common/assets":          swooHtmlBase + "/common/assets",
		"/swoo_html/home_electronic/assets": swooHtmlBase + "/home_electronic/assets",
		"/swoo_html2/common/assets":         swooHtml2Base + "/common/assets",
		"/swoo_html2/inner_pages/assets":    swooHtml2Base + "/inner_pages/assets",

		// Items directory paths
		"/items/swoo_html/common/assets":          swooHtml2Base + "/common/assets",
		"/items/swoo_html/inner_pages/assets":     swooHtml2Base + "/inner_pages/assets",
		"/items/swoo_html/home_electronic/assets": swooHtmlBase + "/home_electronic/assets",

		// Fallback backend static files
		"/static/home_electronic/assets": "/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/home_electronic/assets",
		"/static/common/assets":          "/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/common/assets",
		"/static/inner_pages/assets":     "/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/inner_pages/assets",
	}

	// Register all asset paths
	for urlPath, filePath := range assetPaths {
		router.Static(urlPath, filePath)
	}

	// Add a special handler for relative path that can't be directly mapped
	router.StaticFS("/inner_pages_common_assets", gin.Dir(swooHtmlBase+"/common/assets", false))

	// Add Swagger documentation at root level
	router.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerFiles.Handler))

	// API v1 group
	v1 := router.Group("/api/v1")

	// Swagger documentation in the API group as well (optional)
	v1.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerFiles.Handler))

	// Public routes
	setupPublicRoutes(v1, productHandler, authHandler)

	// Protected routes (require authentication)
	authMiddleware := middleware.AuthMiddleware(authService)
	setupProtectedRoutes(v1, productHandler, authHandler, authMiddleware)

	// Static page routes
	setupStaticRoutes(router, staticHandler)
}

// setupStaticRoutes sets up all the static page routes
func setupStaticRoutes(router *gin.Engine, staticHandler *handlers.StaticHandler) {
	// Homepage route
	router.GET("/", staticHandler.Home)
	router.GET("/index", staticHandler.Home)
	router.GET("/index.html", staticHandler.Home)

	// Inner pages routes with explicit trailing slashes to avoid redirect issues
	// Standard path format
	router.GET("/products", staticHandler.Products)
	router.GET("/products/", staticHandler.Products)
	router.GET("/products.html", staticHandler.Products)

	router.GET("/products-layout-2", staticHandler.ProductsLayout2)
	router.GET("/products-layout-2/", staticHandler.ProductsLayout2)
	router.GET("/products_layout_2.html", staticHandler.ProductsLayout2)

	router.GET("/single-product", staticHandler.SingleProduct)
	router.GET("/single-product/", staticHandler.SingleProduct)
	router.GET("/single_product.html", staticHandler.SingleProduct)

	router.GET("/single-product-pay", staticHandler.SingleProductPay)
	router.GET("/single-product-pay/", staticHandler.SingleProductPay)
	router.GET("/single_product_pay.html", staticHandler.SingleProductPay)

	router.GET("/cart", staticHandler.Cart)
	router.GET("/cart/", staticHandler.Cart)
	router.GET("/cart.html", staticHandler.Cart)

	router.GET("/checkout", staticHandler.Checkout)
	router.GET("/checkout/", staticHandler.Checkout)
	router.GET("/checkout.html", staticHandler.Checkout)

	router.GET("/profile", staticHandler.Profile)
	router.GET("/profile/", staticHandler.Profile)
	router.GET("/profile.html", staticHandler.Profile)

	router.GET("/login", staticHandler.Login)
	router.GET("/login/", staticHandler.Login)
	router.GET("/login.html", staticHandler.Login)

	router.GET("/register", staticHandler.Register)
	router.GET("/register/", staticHandler.Register)
	router.GET("/register.html", staticHandler.Register)

	router.GET("/about", staticHandler.About)
	router.GET("/about/", staticHandler.About)
	router.GET("/about.html", staticHandler.About)

	router.GET("/contact", staticHandler.Contact)
	router.GET("/contact/", staticHandler.Contact)
	router.GET("/contact.html", staticHandler.Contact)

	// Also add inner_pages prefix routes
	router.GET("/inner_pages/products", staticHandler.Products)
	router.GET("/inner_pages/products_layout_2", staticHandler.ProductsLayout2)
	router.GET("/inner_pages/single_product", staticHandler.SingleProduct)
	router.GET("/inner_pages/single_product_pay", staticHandler.SingleProductPay)
	router.GET("/inner_pages/cart", staticHandler.Cart)
	router.GET("/inner_pages/checkout", staticHandler.Checkout)
	router.GET("/inner_pages/profile", staticHandler.Profile)
	router.GET("/inner_pages/login", staticHandler.Login)
	router.GET("/inner_pages/register", staticHandler.Register)
	router.GET("/inner_pages/about", staticHandler.About)
	router.GET("/inner_pages/contact", staticHandler.Contact)

	// Add inner_pages/file.html format routes
	router.GET("/inner_pages/products.html", staticHandler.Products)
	router.GET("/inner_pages/products_layout_2.html", staticHandler.ProductsLayout2)
	router.GET("/inner_pages/single_product.html", staticHandler.SingleProduct)
	router.GET("/inner_pages/single_product_pay.html", staticHandler.SingleProductPay)
	router.GET("/inner_pages/cart.html", staticHandler.Cart)
	router.GET("/inner_pages/checkout.html", staticHandler.Checkout)
	router.GET("/inner_pages/profile.html", staticHandler.Profile)
	router.GET("/inner_pages/login.html", staticHandler.Login)
	router.GET("/inner_pages/register.html", staticHandler.Register)
	router.GET("/inner_pages/about.html", staticHandler.About)
	router.GET("/inner_pages/contact.html", staticHandler.Contact)

	// 404 handler for non-API routes
	router.NoRoute(staticHandler.Error404)
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
