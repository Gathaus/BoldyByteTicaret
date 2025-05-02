package handlers

import (
	"fmt"
	"net/http"
	"os"
	"path/filepath"
	"regexp"
	"strings"

	"github.com/gin-gonic/gin"
	"github.com/mert-yagci/ecommerce-api/pkg/domain/service"
)

// StaticHandler handles requests for static HTML pages
type StaticHandler struct {
	productService  service.ProductService
	categoryService service.CategoryService
}

// NewStaticHandler creates a new static page handler
func NewStaticHandler(productService service.ProductService, categoryService service.CategoryService) *StaticHandler {
	return &StaticHandler{
		productService:  productService,
		categoryService: categoryService,
	}
}

// VirtualAssetMiddleware creates a middleware to serve virtual assets when they don't exist
func VirtualAssetMiddleware() gin.HandlerFunc {
	// Patterns for assets we want to handle virtually if they don't exist
	missingAssetPatterns := []string{
		"/common/assets/js/lib/swiper8.min.js",
		"/inner_pages/assets/js/main.js",
		"/common/assets/js/lib/lity.min.js",
		"/common/assets/js/common_scripts.js",
		"/common/assets/img/fav2.png",
	}

	// Create a map for faster lookup
	assetMap := make(map[string]bool)
	for _, path := range missingAssetPatterns {
		assetMap[path] = true
	}

	return func(c *gin.Context) {
		path := c.Request.URL.Path

		// If this is a path we want to handle as a virtual asset
		if assetMap[path] {
			// Extract file extension to set correct content type
			ext := filepath.Ext(path)
			var contentType string

			switch ext {
			case ".js":
				contentType = "application/javascript"
			case ".css":
				contentType = "text/css"
			case ".png", ".jpg", ".jpeg", ".gif":
				contentType = "image/" + ext[1:]
			default:
				contentType = "text/plain"
			}

			fmt.Printf("Serving virtual asset: %s\n", path)
			c.Header("Content-Type", contentType)

			if strings.HasSuffix(path, ".js") {
				var jsContent string

				if strings.Contains(path, "main.js") {
					// Special handling for main.js which is likely managing loaders
					jsContent = `
					console.log('Virtual main.js loaded');
					
					// This script is crucial for page initialization
					document.addEventListener('DOMContentLoaded', function() {
						console.log('DOMContentLoaded - main.js handler');
						
						// Remove loader
						var loaderWrap = document.querySelector('.loader-wrap');
						if (loaderWrap) {
							loaderWrap.style.display = 'none';
							console.log('Loader hidden by virtual main.js');
						}
						
						// Initialize UI components that might be expected
						try {
							// Setup any required swiper sliders
							if (window.Swiper) {
								var swipers = document.querySelectorAll('.swiper-container, .swiper');
								swipers.forEach(function(element) {
									new Swiper(element, {
										speed: 1000,
										spaceBetween: 30,
										loop: true,
										autoplay: {
											delay: 5000
										}
									});
								});
							}
						} catch(e) {
							console.error('Error initializing components:', e);
						}
					});
					
					// Hide loader immediately too
					var loaderWrap = document.querySelector('.loader-wrap');
					if (loaderWrap) {
						loaderWrap.style.display = 'none';
						console.log('Loader immediately hidden by virtual main.js');
					}
					`
				} else if strings.Contains(path, "swiper") {
					// Virtual Swiper implementation
					jsContent = `
					console.log('Virtual Swiper loaded');
					window.Swiper = function(selector, options) {
						console.log('Swiper initialized with', selector, options);
						
						// Return basic mock implementation
						return {
							destroy: function() {},
							update: function() {},
							on: function(event, callback) {
								if (event === 'init') {
									setTimeout(callback, 100);
								}
							},
							autoplay: {
								start: function() {},
								stop: function() {}
							},
							params: options || {}
						};
					};
					`
				} else if strings.Contains(path, "lity") {
					// Virtual lity implementation
					jsContent = `
					console.log('Virtual lity loaded');
					window.lity = function(target, options) {
						console.log('lity called with', target, options);
						return function() {};
					};
					window.lity.options = {};
					`
				} else if strings.Contains(path, "common_scripts") {
					// Virtual common scripts implementation
					jsContent = `
					console.log('Virtual common_scripts.js loaded');
					
					// This runs immediately to ensure the page loads
					document.addEventListener('DOMContentLoaded', function() {
						console.log('DOMContentLoaded - common_scripts.js');
						
						// Remove loader
						var loaderWrap = document.querySelector('.loader-wrap');
						if (loaderWrap) {
							loaderWrap.style.display = 'none';
							console.log('Loader hidden by common_scripts.js');
						}
					});
					
					// Mobile navigation
					if (typeof jQuery !== 'undefined') {
						jQuery(document).ready(function($) {
							$('.navbar-toggler').on('click', function() {
								$(this).toggleClass('active');
							});
						});
					}
					`
				} else {
					// Generic JS asset
					jsContent = `console.log('Virtual asset loaded: ${path}');`
				}

				c.String(http.StatusOK, jsContent)
				c.Abort() // Stop further handlers
				return
			} else if strings.HasSuffix(path, ".png") ||
				strings.HasSuffix(path, ".jpg") ||
				strings.HasSuffix(path, ".jpeg") ||
				strings.HasSuffix(path, ".gif") {
				// For images, return a transparent 1x1 pixel GIF
				c.Data(http.StatusOK, contentType, []byte("GIF89a\x01\x00\x01\x00\x80\x00\x00\xff\xff\xff\x00\x00\x00!\xf9\x04\x01\x00\x00\x00\x00,\x00\x00\x00\x00\x01\x00\x01\x00\x00\x02\x02D\x01\x00;"))
				c.Abort() // Stop further handlers
				return
			} else {
				// Return empty content for other types
				c.String(http.StatusOK, "/* Virtual asset: "+path+" */")
				c.Abort() // Stop further handlers
				return
			}
		}

		// Continue to next handler if not a virtual asset
		c.Next()
	}
}

// fixHTMLAssetPaths modifies HTML content to use absolute paths for assets
func fixHTMLAssetPaths(htmlContent string) string {
	// Replace relative paths with absolute paths
	replacements := map[string]string{
		"../common/assets": "/common/assets",
		"\"./assets/":      "\"/inner_pages/assets/",
		"\"assets/":        "\"/inner_pages/assets/",
	}

	fixed := htmlContent
	for from, to := range replacements {
		fixed = strings.ReplaceAll(fixed, from, to)
	}

	// Fix HTML links using a simpler, Go-compatible regex approach
	// First, compile the regular expression for finding relative HTML/PHP links
	reHtml := regexp.MustCompile(`(href|src)=["']([^/"':]+\.(?:html|php))["']`)

	// Replace relative HTML/PHP links with absolute paths to inner_pages
	fixed = reHtml.ReplaceAllString(fixed, `$1="/inner_pages/$2"`)

	return fixed
}

// serveModifiedHTML reads a file, fixes asset paths, and serves the content
func (h *StaticHandler) serveModifiedHTML(c *gin.Context, filePath string) {
	// Read the HTML file
	htmlBytes, err := os.ReadFile(filePath)
	if err != nil {
		fmt.Printf("Error reading file %s: %v\n", filePath, err)
		c.String(http.StatusInternalServerError, "Error reading file")
		return
	}

	// Fix asset paths in the HTML content
	htmlContent := string(htmlBytes)
	fixedHTML := fixHTMLAssetPaths(htmlContent)

	// Add debugging script to help identify issues and fix the loader problem
	debugScript := `
	<script>
	console.log('Page loaded with modified paths');
	
	// Forcibly remove loader after DOM is loaded
	document.addEventListener('DOMContentLoaded', function() {
		console.log('DOM Content Loaded - auto-hiding loaders');
		
		// Function to hide loader with retry
		function hideLoader() {
			// Try to find and hide loader
			var loaderWrap = document.querySelector('.loader-wrap');
			if (loaderWrap) {
				loaderWrap.style.display = 'none';
				console.log('Loader hidden by injected script');
				return true;
			}
			return false;
		}
		
		// Try immediately
		if (!hideLoader()) {
			// Retry a few times if not found
			for (var i = 0; i < 5; i++) {
				setTimeout(hideLoader, i * 500);
			}
		}
		
		// Force document body to have loaded class
		document.body.classList.add('loaded');
		document.body.classList.remove('loading');
	});
	
	// Try to hide loader immediately too
	window.addEventListener('load', function() {
		var loaderWrap = document.querySelector('.loader-wrap');
		if (loaderWrap) {
			loaderWrap.style.display = 'none';
			console.log('Loader hidden by load event');
		}
	});
	
	// Hide loader immediately
	(function() {
		var loaderWrap = document.querySelector('.loader-wrap');
		if (loaderWrap) {
			loaderWrap.style.display = 'none';
			console.log('Loader immediately hidden');
		}
	})();
	</script>
	`

	// Insert debug script before </head>
	fixedHTML = strings.Replace(fixedHTML, "</head>", debugScript+"</head>", 1)

	// Also add an inline style to hide the loader
	inlineStyle := `<style>.loader-wrap {display: none !important;}</style>`
	fixedHTML = strings.Replace(fixedHTML, "</head>", inlineStyle+"</head>", 1)

	// Log the page being served and its path
	fmt.Printf("Serving modified HTML file: %s\n", filePath)

	// Set content type and serve the modified HTML
	c.Header("Content-Type", "text/html; charset=utf-8")
	c.String(http.StatusOK, fixedHTML)
}

// Home handles the home page request
func (h *StaticHandler) Home(c *gin.Context) {
	// Use web/static directory instead of frontend/swoo_html
	c.File("/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/home_electronic/index.html")
}

// Products handles the products page request
func (h *StaticHandler) Products(c *gin.Context) {
	fmt.Println("Serving products page")
	h.serveModifiedHTML(c, "/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/inner_pages/products.html")
}

// ProductsLayout2 handles the alternative products layout page request
func (h *StaticHandler) ProductsLayout2(c *gin.Context) {
	fmt.Println("Serving products layout 2 page")
	h.serveModifiedHTML(c, "/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/inner_pages/products_layout_2.html")
}

// SingleProduct handles the single product page request
func (h *StaticHandler) SingleProduct(c *gin.Context) {
	fmt.Println("Serving single product page")
	h.serveModifiedHTML(c, "/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/inner_pages/single_product.html")
}

// SingleProductPay handles the single product payment page request
func (h *StaticHandler) SingleProductPay(c *gin.Context) {
	fmt.Println("Serving single product pay page")
	h.serveModifiedHTML(c, "/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/inner_pages/single_product_pay.html")
}

// Cart handles the cart page request
func (h *StaticHandler) Cart(c *gin.Context) {
	fmt.Println("Serving cart page")
	h.serveModifiedHTML(c, "/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/inner_pages/cart.html")
}

// Checkout handles the checkout page request
func (h *StaticHandler) Checkout(c *gin.Context) {
	fmt.Println("Serving checkout page")
	h.serveModifiedHTML(c, "/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/inner_pages/checkout.html")
}

// Profile handles the profile page request
func (h *StaticHandler) Profile(c *gin.Context) {
	fmt.Println("Serving profile page")
	h.serveModifiedHTML(c, "/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/inner_pages/profile.html")
}

// Login handles the login page request
func (h *StaticHandler) Login(c *gin.Context) {
	fmt.Println("Serving login page")
	h.serveModifiedHTML(c, "/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/inner_pages/login.html")
}

// Register handles the register page request
func (h *StaticHandler) Register(c *gin.Context) {
	fmt.Println("Serving register page")
	h.serveModifiedHTML(c, "/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/inner_pages/register.html")
}

// About handles the about page request
func (h *StaticHandler) About(c *gin.Context) {
	fmt.Println("Serving about page")
	h.serveModifiedHTML(c, "/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/inner_pages/about.html")
}

// Contact handles the contact page request
func (h *StaticHandler) Contact(c *gin.Context) {
	fmt.Println("Serving contact page")
	h.serveModifiedHTML(c, "/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/inner_pages/contact.html")
}

// Error404 handles 404 errors
func (h *StaticHandler) Error404(c *gin.Context) {
	// Check if the request is for the API
	if len(c.Request.URL.Path) >= 4 && c.Request.URL.Path[:4] == "/api" {
		c.JSON(http.StatusNotFound, gin.H{"error": "API endpoint not found"})
		return
	}

	// Log detailed information about the request
	fmt.Println("------------------------------------")
	fmt.Println("404 Error for path:", c.Request.URL.Path)
	fmt.Println("Method:", c.Request.Method)
	fmt.Println("Headers:", c.Request.Header)
	fmt.Println("Query params:", c.Request.URL.Query())
	fmt.Println("------------------------------------")

	// For non-API paths, serve the home page from the new location
	c.File("/Users/mert.yagci/myDevLogs/ETİCARET/backend/ecommerce-api/web/static/home_electronic/index.html")
}
