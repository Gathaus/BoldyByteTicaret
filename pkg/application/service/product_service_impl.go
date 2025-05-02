package service

import (
	"context"

	"github.com/mert-yagci/ecommerce-api/pkg/domain/entity"
	"github.com/mert-yagci/ecommerce-api/pkg/domain/repository"
	"github.com/mert-yagci/ecommerce-api/pkg/domain/service"
)

// ProductServiceImpl implements the ProductService interface
type ProductServiceImpl struct {
	productRepo repository.ProductRepository
}

// NewProductService creates a new product service implementation
func NewProductService(productRepo repository.ProductRepository) service.ProductService {
	return &ProductServiceImpl{
		productRepo: productRepo,
	}
}

// GetProducts retrieves a paginated list of products
func (s *ProductServiceImpl) GetProducts(ctx context.Context, limit, offset int) ([]entity.Product, int64, error) {
	return s.productRepo.FindAll(ctx, limit, offset)
}

// GetProductByID retrieves a product by its ID
func (s *ProductServiceImpl) GetProductByID(ctx context.Context, id uint) (*entity.Product, error) {
	return s.productRepo.FindByID(ctx, id)
}

// GetProductsByCategory retrieves products by category ID
func (s *ProductServiceImpl) GetProductsByCategory(ctx context.Context, categoryID uint, limit, offset int) ([]entity.Product, int64, error) {
	return s.productRepo.FindByCategory(ctx, categoryID, limit, offset)
}

// SearchProducts searches for products by name or description
func (s *ProductServiceImpl) SearchProducts(ctx context.Context, query string, limit, offset int) ([]entity.Product, int64, error) {
	return s.productRepo.Search(ctx, query, limit, offset)
}

// CreateProduct creates a new product
func (s *ProductServiceImpl) CreateProduct(ctx context.Context, product *entity.Product) error {
	return s.productRepo.Create(ctx, product)
}

// UpdateProduct updates an existing product
func (s *ProductServiceImpl) UpdateProduct(ctx context.Context, product *entity.Product) error {
	return s.productRepo.Update(ctx, product)
}

// DeleteProduct deletes a product by its ID
func (s *ProductServiceImpl) DeleteProduct(ctx context.Context, id uint) error {
	return s.productRepo.Delete(ctx, id)
}

// CategoryServiceImpl implements the CategoryService interface
type CategoryServiceImpl struct {
	categoryRepo repository.CategoryRepository
}

// NewCategoryService creates a new category service implementation
func NewCategoryService(categoryRepo repository.CategoryRepository) service.CategoryService {
	return &CategoryServiceImpl{
		categoryRepo: categoryRepo,
	}
}

// GetCategories retrieves all categories
func (s *CategoryServiceImpl) GetCategories(ctx context.Context) ([]entity.Category, error) {
	return s.categoryRepo.FindAll(ctx)
}

// GetCategoryByID retrieves a category by its ID
func (s *CategoryServiceImpl) GetCategoryByID(ctx context.Context, id uint) (*entity.Category, error) {
	return s.categoryRepo.FindByID(ctx, id)
}

// CreateCategory creates a new category
func (s *CategoryServiceImpl) CreateCategory(ctx context.Context, category *entity.Category) error {
	return s.categoryRepo.Create(ctx, category)
}

// UpdateCategory updates an existing category
func (s *CategoryServiceImpl) UpdateCategory(ctx context.Context, category *entity.Category) error {
	return s.categoryRepo.Update(ctx, category)
}

// DeleteCategory deletes a category by its ID
func (s *CategoryServiceImpl) DeleteCategory(ctx context.Context, id uint) error {
	return s.categoryRepo.Delete(ctx, id)
}
