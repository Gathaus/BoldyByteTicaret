package repository

import (
	"context"

	"github.com/mert-yagci/ecommerce-api/pkg/domain/entity"
)

// ProductRepository defines the interface for product data access
type ProductRepository interface {
	FindAll(ctx context.Context, limit, offset int) ([]entity.Product, int64, error)
	FindByID(ctx context.Context, id uint) (*entity.Product, error)
	FindByCategory(ctx context.Context, categoryID uint, limit, offset int) ([]entity.Product, int64, error)
	Search(ctx context.Context, query string, limit, offset int) ([]entity.Product, int64, error)
	Create(ctx context.Context, product *entity.Product) error
	Update(ctx context.Context, product *entity.Product) error
	Delete(ctx context.Context, id uint) error
}

// CategoryRepository defines the interface for category data access
type CategoryRepository interface {
	FindAll(ctx context.Context) ([]entity.Category, error)
	FindByID(ctx context.Context, id uint) (*entity.Category, error)
	Create(ctx context.Context, category *entity.Category) error
	Update(ctx context.Context, category *entity.Category) error
	Delete(ctx context.Context, id uint) error
}
