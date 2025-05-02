package handlers

import (
	"net/http"
	"strconv"

	"github.com/gin-gonic/gin"
	"github.com/mert-yagci/ecommerce-api/internal/errors"
	"github.com/mert-yagci/ecommerce-api/pkg/api/dto"
	"github.com/mert-yagci/ecommerce-api/pkg/domain/entity"
	"github.com/mert-yagci/ecommerce-api/pkg/domain/service"
)

// ProductHandler handles product-related requests
type ProductHandler struct {
	productService  service.ProductService
	categoryService service.CategoryService
}

// NewProductHandler creates a new product handler
func NewProductHandler(productService service.ProductService, categoryService service.CategoryService) *ProductHandler {
	return &ProductHandler{
		productService:  productService,
		categoryService: categoryService,
	}
}

// GetProductService returns the product service
func (h *ProductHandler) GetProductService() service.ProductService {
	return h.productService
}

// GetCategoryService returns the category service
func (h *ProductHandler) GetCategoryService() service.CategoryService {
	return h.categoryService
}

// GetProducts godoc
// @Summary Get all products
// @Description Get a paginated list of all products
// @Tags products
// @Accept json
// @Produce json
// @Param page query int false "Page number"
// @Param limit query int false "Items per page"
// @Success 200 {object} dto.ProductListResponse
// @Failure 400 {object} map[string]string
// @Failure 500 {object} map[string]string
// @Router /api/v1/products [get]
func (h *ProductHandler) GetProducts(c *gin.Context) {
	// Parse pagination parameters
	page, limit := parsePaginationParams(c)
	offset := (page - 1) * limit

	// Get products from the service
	products, total, err := h.productService.GetProducts(c, limit, offset)
	if err != nil {
		errors.HandleError(c, err)
		return
	}

	// Map domain entities to response DTOs
	response := dto.ProductListResponse{
		Products: make([]dto.ProductResponse, len(products)),
		Total:    total,
		Page:     page,
		Limit:    limit,
	}

	for i, product := range products {
		response.Products[i] = mapProductToResponse(&product)
	}

	c.JSON(http.StatusOK, response)
}

// GetProductByID godoc
// @Summary Get product by ID
// @Description Get detailed information about a product by its ID
// @Tags products
// @Accept json
// @Produce json
// @Param id path int true "Product ID"
// @Success 200 {object} dto.ProductResponse
// @Failure 404 {object} map[string]string
// @Failure 500 {object} map[string]string
// @Router /api/v1/products/{id} [get]
func (h *ProductHandler) GetProductByID(c *gin.Context) {
	// Parse product ID
	id, err := strconv.ParseUint(c.Param("id"), 10, 32)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid product ID"})
		return
	}

	// Get product from the service
	product, err := h.productService.GetProductByID(c, uint(id))
	if err != nil {
		errors.HandleError(c, err)
		return
	}

	// If product not found
	if product == nil {
		c.JSON(http.StatusNotFound, gin.H{"error": "Product not found"})
		return
	}

	// Map domain entity to response DTO
	response := mapProductToResponse(product)

	// Get category name if available
	if product.CategoryID > 0 {
		category, err := h.categoryService.GetCategoryByID(c, product.CategoryID)
		if err == nil && category != nil {
			response.CategoryName = category.Name
		}
	}

	c.JSON(http.StatusOK, response)
}

// GetProductsByCategory godoc
// @Summary Get products by category
// @Description Get a paginated list of products by category ID
// @Tags products
// @Accept json
// @Produce json
// @Param categoryId path int true "Category ID"
// @Param page query int false "Page number"
// @Param limit query int false "Items per page"
// @Success 200 {object} dto.ProductListResponse
// @Failure 400 {object} map[string]string
// @Failure 500 {object} map[string]string
// @Router /api/v1/categories/{categoryId}/products [get]
func (h *ProductHandler) GetProductsByCategory(c *gin.Context) {
	// Parse category ID
	categoryID, err := strconv.ParseUint(c.Param("categoryId"), 10, 32)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid category ID"})
		return
	}

	// Parse pagination parameters
	page, limit := parsePaginationParams(c)
	offset := (page - 1) * limit

	// Get products from the service
	products, total, err := h.productService.GetProductsByCategory(c, uint(categoryID), limit, offset)
	if err != nil {
		errors.HandleError(c, err)
		return
	}

	// Map domain entities to response DTOs
	response := dto.ProductListResponse{
		Products: make([]dto.ProductResponse, len(products)),
		Total:    total,
		Page:     page,
		Limit:    limit,
	}

	// Get category name
	var categoryName string
	category, err := h.categoryService.GetCategoryByID(c, uint(categoryID))
	if err == nil && category != nil {
		categoryName = category.Name
	}

	for i, product := range products {
		productResponse := mapProductToResponse(&product)
		productResponse.CategoryName = categoryName
		response.Products[i] = productResponse
	}

	c.JSON(http.StatusOK, response)
}

// SearchProducts godoc
// @Summary Search products
// @Description Search products by name or description
// @Tags products
// @Accept json
// @Produce json
// @Param query query string true "Search query"
// @Param page query int false "Page number"
// @Param limit query int false "Items per page"
// @Success 200 {object} dto.ProductListResponse
// @Failure 400 {object} map[string]string
// @Failure 500 {object} map[string]string
// @Router /api/v1/products/search [get]
func (h *ProductHandler) SearchProducts(c *gin.Context) {
	// Get search query
	query := c.Query("query")
	if query == "" {
		c.JSON(http.StatusBadRequest, gin.H{"error": "Search query is required"})
		return
	}

	// Parse pagination parameters
	page, limit := parsePaginationParams(c)
	offset := (page - 1) * limit

	// Get products from the service
	products, total, err := h.productService.SearchProducts(c, query, limit, offset)
	if err != nil {
		errors.HandleError(c, err)
		return
	}

	// Map domain entities to response DTOs
	response := dto.ProductListResponse{
		Products: make([]dto.ProductResponse, len(products)),
		Total:    total,
		Page:     page,
		Limit:    limit,
	}

	for i, product := range products {
		response.Products[i] = mapProductToResponse(&product)
	}

	c.JSON(http.StatusOK, response)
}

// CreateProduct godoc
// @Summary Create a new product
// @Description Create a new product with the provided details
// @Tags products
// @Accept json
// @Produce json
// @Param product body dto.CreateProductRequest true "Product information"
// @Success 201 {object} dto.ProductResponse
// @Failure 400 {object} map[string]string
// @Failure 500 {object} map[string]string
// @Security ApiKeyAuth
// @Router /api/v1/products [post]
func (h *ProductHandler) CreateProduct(c *gin.Context) {
	// Parse request body
	var request dto.CreateProductRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	// Validate category exists
	_, err := h.categoryService.GetCategoryByID(c, request.CategoryID)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid category ID"})
		return
	}

	// Map DTO to domain entity
	product := entity.Product{
		Name:        request.Name,
		Description: request.Description,
		Price:       request.Price,
		Stock:       request.Stock,
		ImageURL:    request.ImageURL,
		CategoryID:  request.CategoryID,
	}

	// Create the product
	err = h.productService.CreateProduct(c, &product)
	if err != nil {
		errors.HandleError(c, err)
		return
	}

	// Map domain entity to response DTO
	response := mapProductToResponse(&product)

	c.JSON(http.StatusCreated, response)
}

// UpdateProduct godoc
// @Summary Update a product
// @Description Update a product with the provided details
// @Tags products
// @Accept json
// @Produce json
// @Param id path int true "Product ID"
// @Param product body dto.UpdateProductRequest true "Product information"
// @Success 200 {object} dto.ProductResponse
// @Failure 400 {object} map[string]string
// @Failure 404 {object} map[string]string
// @Failure 500 {object} map[string]string
// @Security ApiKeyAuth
// @Router /api/v1/products/{id} [put]
func (h *ProductHandler) UpdateProduct(c *gin.Context) {
	// Parse product ID
	id, err := strconv.ParseUint(c.Param("id"), 10, 32)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid product ID"})
		return
	}

	// Parse request body
	var request dto.UpdateProductRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	// Validate category exists
	_, err = h.categoryService.GetCategoryByID(c, request.CategoryID)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid category ID"})
		return
	}

	// Get the existing product
	product, err := h.productService.GetProductByID(c, uint(id))
	if err != nil {
		errors.HandleError(c, err)
		return
	}

	// If product not found
	if product == nil {
		c.JSON(http.StatusNotFound, gin.H{"error": "Product not found"})
		return
	}

	// Update product fields
	product.Name = request.Name
	product.Description = request.Description
	product.Price = request.Price
	product.Stock = request.Stock
	product.ImageURL = request.ImageURL
	product.CategoryID = request.CategoryID

	// Update the product
	err = h.productService.UpdateProduct(c, product)
	if err != nil {
		errors.HandleError(c, err)
		return
	}

	// Map domain entity to response DTO
	response := mapProductToResponse(product)

	c.JSON(http.StatusOK, response)
}

// DeleteProduct godoc
// @Summary Delete a product
// @Description Delete a product by its ID
// @Tags products
// @Accept json
// @Produce json
// @Param id path int true "Product ID"
// @Success 204 "No Content"
// @Failure 400 {object} map[string]string
// @Failure 404 {object} map[string]string
// @Failure 500 {object} map[string]string
// @Security ApiKeyAuth
// @Router /api/v1/products/{id} [delete]
func (h *ProductHandler) DeleteProduct(c *gin.Context) {
	// Parse product ID
	id, err := strconv.ParseUint(c.Param("id"), 10, 32)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid product ID"})
		return
	}

	// Delete the product
	err = h.productService.DeleteProduct(c, uint(id))
	if err != nil {
		errors.HandleError(c, err)
		return
	}

	c.Status(http.StatusNoContent)
}

// GetCategories godoc
// @Summary Get all categories
// @Description Get a list of all product categories
// @Tags categories
// @Accept json
// @Produce json
// @Success 200 {object} dto.CategoryListResponse
// @Failure 500 {object} map[string]string
// @Router /api/v1/categories [get]
func (h *ProductHandler) GetCategories(c *gin.Context) {
	// Get categories from the service
	categories, err := h.categoryService.GetCategories(c)
	if err != nil {
		errors.HandleError(c, err)
		return
	}

	// Map domain entities to response DTOs
	response := dto.CategoryListResponse{
		Categories: make([]dto.CategoryResponse, len(categories)),
	}

	for i, category := range categories {
		response.Categories[i] = dto.CategoryResponse{
			ID:   category.ID,
			Name: category.Name,
		}
	}

	c.JSON(http.StatusOK, response)
}

// GetCategoryByID godoc
// @Summary Get category by ID
// @Description Get a product category by its ID
// @Tags categories
// @Accept json
// @Produce json
// @Param id path int true "Category ID"
// @Success 200 {object} dto.CategoryResponse
// @Failure 404 {object} map[string]string
// @Failure 500 {object} map[string]string
// @Router /api/v1/categories/{id} [get]
func (h *ProductHandler) GetCategoryByID(c *gin.Context) {
	// Parse category ID
	id, err := strconv.ParseUint(c.Param("id"), 10, 32)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid category ID"})
		return
	}

	// Get category from the service
	category, err := h.categoryService.GetCategoryByID(c, uint(id))
	if err != nil {
		errors.HandleError(c, err)
		return
	}

	// If category not found
	if category == nil {
		c.JSON(http.StatusNotFound, gin.H{"error": "Category not found"})
		return
	}

	// Map domain entity to response DTO
	response := dto.CategoryResponse{
		ID:   category.ID,
		Name: category.Name,
	}

	c.JSON(http.StatusOK, response)
}

// CreateCategory godoc
// @Summary Create a new category
// @Description Create a new product category
// @Tags categories
// @Accept json
// @Produce json
// @Param category body dto.CreateCategoryRequest true "Category information"
// @Success 201 {object} dto.CategoryResponse
// @Failure 400 {object} map[string]string
// @Failure 500 {object} map[string]string
// @Security ApiKeyAuth
// @Router /api/v1/categories [post]
func (h *ProductHandler) CreateCategory(c *gin.Context) {
	// Parse request body
	var request dto.CreateCategoryRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	// Map DTO to domain entity
	category := entity.Category{
		Name: request.Name,
	}

	// Create the category
	err := h.categoryService.CreateCategory(c, &category)
	if err != nil {
		errors.HandleError(c, err)
		return
	}

	// Map domain entity to response DTO
	response := dto.CategoryResponse{
		ID:   category.ID,
		Name: category.Name,
	}

	c.JSON(http.StatusCreated, response)
}

// UpdateCategory godoc
// @Summary Update a category
// @Description Update a product category
// @Tags categories
// @Accept json
// @Produce json
// @Param id path int true "Category ID"
// @Param category body dto.UpdateCategoryRequest true "Category information"
// @Success 200 {object} dto.CategoryResponse
// @Failure 400 {object} map[string]string
// @Failure 404 {object} map[string]string
// @Failure 500 {object} map[string]string
// @Security ApiKeyAuth
// @Router /api/v1/categories/{id} [put]
func (h *ProductHandler) UpdateCategory(c *gin.Context) {
	// Parse category ID
	id, err := strconv.ParseUint(c.Param("id"), 10, 32)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid category ID"})
		return
	}

	// Parse request body
	var request dto.UpdateCategoryRequest
	if err := c.ShouldBindJSON(&request); err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
		return
	}

	// Get the existing category
	category, err := h.categoryService.GetCategoryByID(c, uint(id))
	if err != nil {
		errors.HandleError(c, err)
		return
	}

	// If category not found
	if category == nil {
		c.JSON(http.StatusNotFound, gin.H{"error": "Category not found"})
		return
	}

	// Update category fields
	category.Name = request.Name

	// Update the category
	err = h.categoryService.UpdateCategory(c, category)
	if err != nil {
		errors.HandleError(c, err)
		return
	}

	// Map domain entity to response DTO
	response := dto.CategoryResponse{
		ID:   category.ID,
		Name: category.Name,
	}

	c.JSON(http.StatusOK, response)
}

// DeleteCategory godoc
// @Summary Delete a category
// @Description Delete a product category by its ID
// @Tags categories
// @Accept json
// @Produce json
// @Param id path int true "Category ID"
// @Success 204 "No Content"
// @Failure 400 {object} map[string]string
// @Failure 404 {object} map[string]string
// @Failure 500 {object} map[string]string
// @Security ApiKeyAuth
// @Router /api/v1/categories/{id} [delete]
func (h *ProductHandler) DeleteCategory(c *gin.Context) {
	// Parse category ID
	id, err := strconv.ParseUint(c.Param("id"), 10, 32)
	if err != nil {
		c.JSON(http.StatusBadRequest, gin.H{"error": "Invalid category ID"})
		return
	}

	// Delete the category
	err = h.categoryService.DeleteCategory(c, uint(id))
	if err != nil {
		errors.HandleError(c, err)
		return
	}

	c.Status(http.StatusNoContent)
}

// Helper function to parse pagination parameters
func parsePaginationParams(c *gin.Context) (page, limit int) {
	pageStr := c.DefaultQuery("page", "1")
	limitStr := c.DefaultQuery("limit", "10")

	page, err := strconv.Atoi(pageStr)
	if err != nil || page < 1 {
		page = 1
	}

	limit, err = strconv.Atoi(limitStr)
	if err != nil || limit < 1 || limit > 100 {
		limit = 10
	}

	return page, limit
}

// Helper function to map a domain entity to a response DTO
func mapProductToResponse(product *entity.Product) dto.ProductResponse {
	return dto.ProductResponse{
		ID:          product.ID,
		Name:        product.Name,
		Description: product.Description,
		Price:       product.Price,
		Stock:       product.Stock,
		ImageURL:    product.ImageURL,
		CategoryID:  product.CategoryID,
	}
}
