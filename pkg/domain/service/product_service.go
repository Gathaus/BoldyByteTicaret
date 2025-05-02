package service

import (
	"context"

	"github.com/mert-yagci/ecommerce-api/pkg/domain/entity"
)

// ProductService defines the interface for product business logic
type ProductService interface {
	GetProducts(ctx context.Context, limit, offset int) ([]entity.Product, int64, error)
	GetProductByID(ctx context.Context, id uint) (*entity.Product, error)
	GetProductsByCategory(ctx context.Context, categoryID uint, limit, offset int) ([]entity.Product, int64, error)
	SearchProducts(ctx context.Context, query string, limit, offset int) ([]entity.Product, int64, error)
	CreateProduct(ctx context.Context, product *entity.Product) error
	UpdateProduct(ctx context.Context, product *entity.Product) error
	DeleteProduct(ctx context.Context, id uint) error
}

// CategoryService defines the interface for category business logic
type CategoryService interface {
	GetCategories(ctx context.Context) ([]entity.Category, error)
	GetCategoryByID(ctx context.Context, id uint) (*entity.Category, error)
	CreateCategory(ctx context.Context, category *entity.Category) error
	UpdateCategory(ctx context.Context, category *entity.Category) error
	DeleteCategory(ctx context.Context, id uint) error
}
