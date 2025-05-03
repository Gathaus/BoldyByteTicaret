package handlers

import (
	"github.com/gin-gonic/gin"
)

// StaticHandler handles static file requests
type StaticHandler struct{}

// NewStaticHandler creates a new StaticHandler
func NewStaticHandler() *StaticHandler {
	return &StaticHandler{}
}

// Home serves the home page
func (h *StaticHandler) Home(c *gin.Context) {
	c.File("./web/templates/pages/index.html")
}

// About serves the about page
func (h *StaticHandler) About(c *gin.Context) {
	c.File("./web/templates/pages/about.html")
}

// Contact serves the contact page
func (h *StaticHandler) Contact(c *gin.Context) {
	c.File("./web/templates/pages/contact.html")
}

// Login serves the login page
func (h *StaticHandler) Login(c *gin.Context) {
	c.File("./web/templates/pages/login.html")
}

// Register serves the register page
func (h *StaticHandler) Register(c *gin.Context) {
	c.File("./web/templates/pages/register.html")
}

// Profile serves the profile page
func (h *StaticHandler) Profile(c *gin.Context) {
	c.File("./web/templates/pages/profile.html")
}

// Products serves the products page
func (h *StaticHandler) Products(c *gin.Context) {
	c.File("./web/templates/pages/products.html")
}

// ProductsLayout2 serves the products layout 2 page
func (h *StaticHandler) ProductsLayout2(c *gin.Context) {
	c.File("./web/templates/pages/products_layout_2.html")
}

// Cart serves the cart page
func (h *StaticHandler) Cart(c *gin.Context) {
	c.File("./web/templates/pages/cart.html")
}

// Checkout serves the checkout page
func (h *StaticHandler) Checkout(c *gin.Context) {
	c.File("./web/templates/pages/checkout.html")
}

// SingleProduct serves the single product page
func (h *StaticHandler) SingleProduct(c *gin.Context) {
	c.File("./web/templates/pages/single_product.html")
}

// SingleProductPay serves the single product pay page
func (h *StaticHandler) SingleProductPay(c *gin.Context) {
	c.File("./web/templates/pages/single_product_pay.html")
}
