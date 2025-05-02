package dto

// ProductResponse represents a product for API responses
type ProductResponse struct {
	ID           uint    `json:"id"`
	Name         string  `json:"name"`
	Description  string  `json:"description"`
	Price        float64 `json:"price"`
	Stock        int     `json:"stock"`
	ImageURL     string  `json:"imageUrl"`
	CategoryID   uint    `json:"categoryId"`
	CategoryName string  `json:"categoryName,omitempty"`
}

// ProductListResponse represents a paginated list of products
type ProductListResponse struct {
	Products []ProductResponse `json:"products"`
	Total    int64             `json:"total"`
	Page     int               `json:"page"`
	Limit    int               `json:"limit"`
}

// CreateProductRequest represents a request to create a product
type CreateProductRequest struct {
	Name        string  `json:"name" binding:"required"`
	Description string  `json:"description"`
	Price       float64 `json:"price" binding:"required,gt=0"`
	Stock       int     `json:"stock" binding:"required,gte=0"`
	ImageURL    string  `json:"imageUrl"`
	CategoryID  uint    `json:"categoryId" binding:"required"`
}

// UpdateProductRequest represents a request to update a product
type UpdateProductRequest struct {
	Name        string  `json:"name" binding:"required"`
	Description string  `json:"description"`
	Price       float64 `json:"price" binding:"required,gt=0"`
	Stock       int     `json:"stock" binding:"required,gte=0"`
	ImageURL    string  `json:"imageUrl"`
	CategoryID  uint    `json:"categoryId" binding:"required"`
}

// CategoryResponse represents a category for API responses
type CategoryResponse struct {
	ID   uint   `json:"id"`
	Name string `json:"name"`
}

// CategoryListResponse represents a list of categories
type CategoryListResponse struct {
	Categories []CategoryResponse `json:"categories"`
}

// CreateCategoryRequest represents a request to create a category
type CreateCategoryRequest struct {
	Name string `json:"name" binding:"required"`
}

// UpdateCategoryRequest represents a request to update a category
type UpdateCategoryRequest struct {
	Name string `json:"name" binding:"required"`
}
