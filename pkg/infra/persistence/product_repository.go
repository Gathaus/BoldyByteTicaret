package persistence

import (
	"context"

	"github.com/mert-yagci/ecommerce-api/pkg/domain/entity"
	"github.com/mert-yagci/ecommerce-api/pkg/domain/repository"
	"gorm.io/gorm"
)

// ProductRepositoryImpl implements the ProductRepository interface
type ProductRepositoryImpl struct {
	db *gorm.DB
}

// NewProductRepository creates a new product repository implementation
func NewProductRepository(db *gorm.DB) repository.ProductRepository {
	return &ProductRepositoryImpl{
		db: db,
	}
}

// mapProductModelToDomain maps a product model to a domain entity
func mapProductModelToDomain(model *Product) *entity.Product {
	if model == nil {
		return nil
	}
	return &entity.Product{
		ID:          model.ID,
		Name:        model.Name,
		Description: model.Description,
		Price:       model.Price,
		Stock:       model.Stock,
		ImageURL:    model.ImageURL,
		CategoryID:  model.CategoryID,
		CreatedAt:   model.CreatedAt,
		UpdatedAt:   model.UpdatedAt,
	}
}

// mapProductDomainToModel maps a domain entity to a product model
func mapProductDomainToModel(domain *entity.Product) *Product {
	if domain == nil {
		return nil
	}
	return &Product{
		Model: gorm.Model{
			ID:        domain.ID,
			CreatedAt: domain.CreatedAt,
			UpdatedAt: domain.UpdatedAt,
		},
		Name:        domain.Name,
		Description: domain.Description,
		Price:       domain.Price,
		Stock:       domain.Stock,
		ImageURL:    domain.ImageURL,
		CategoryID:  domain.CategoryID,
	}
}

// FindAll retrieves a paginated list of products
func (r *ProductRepositoryImpl) FindAll(ctx context.Context, limit, offset int) ([]entity.Product, int64, error) {
	var products []Product
	var count int64

	// Count total products
	if err := r.db.Model(&Product{}).Count(&count).Error; err != nil {
		return nil, 0, err
	}

	// Retrieve paginated products
	if err := r.db.Limit(limit).Offset(offset).Find(&products).Error; err != nil {
		return nil, 0, err
	}

	// Map to domain entities
	result := make([]entity.Product, len(products))
	for i, product := range products {
		domainProduct := mapProductModelToDomain(&product)
		result[i] = *domainProduct
	}

	return result, count, nil
}

// FindByID retrieves a product by its ID
func (r *ProductRepositoryImpl) FindByID(ctx context.Context, id uint) (*entity.Product, error) {
	var product Product
	if err := r.db.First(&product, id).Error; err != nil {
		return nil, err
	}
	return mapProductModelToDomain(&product), nil
}

// FindByCategory retrieves products by category ID
func (r *ProductRepositoryImpl) FindByCategory(ctx context.Context, categoryID uint, limit, offset int) ([]entity.Product, int64, error) {
	var products []Product
	var count int64

	// Count total products in category
	if err := r.db.Model(&Product{}).Where("category_id = ?", categoryID).Count(&count).Error; err != nil {
		return nil, 0, err
	}

	// Retrieve paginated products by category
	if err := r.db.Where("category_id = ?", categoryID).Limit(limit).Offset(offset).Find(&products).Error; err != nil {
		return nil, 0, err
	}

	// Map to domain entities
	result := make([]entity.Product, len(products))
	for i, product := range products {
		domainProduct := mapProductModelToDomain(&product)
		result[i] = *domainProduct
	}

	return result, count, nil
}

// Search searches for products by name or description
func (r *ProductRepositoryImpl) Search(ctx context.Context, query string, limit, offset int) ([]entity.Product, int64, error) {
	var products []Product
	var count int64
	searchQuery := "%" + query + "%"

	// Count total matching products
	if err := r.db.Model(&Product{}).Where("name LIKE ? OR description LIKE ?", searchQuery, searchQuery).Count(&count).Error; err != nil {
		return nil, 0, err
	}

	// Retrieve paginated search results
	if err := r.db.Where("name LIKE ? OR description LIKE ?", searchQuery, searchQuery).Limit(limit).Offset(offset).Find(&products).Error; err != nil {
		return nil, 0, err
	}

	// Map to domain entities
	result := make([]entity.Product, len(products))
	for i, product := range products {
		domainProduct := mapProductModelToDomain(&product)
		result[i] = *domainProduct
	}

	return result, count, nil
}

// Create creates a new product
func (r *ProductRepositoryImpl) Create(ctx context.Context, product *entity.Product) error {
	model := mapProductDomainToModel(product)
	if err := r.db.Create(model).Error; err != nil {
		return err
	}
	// Update the domain entity with generated ID and timestamps
	product.ID = model.ID
	product.CreatedAt = model.CreatedAt
	product.UpdatedAt = model.UpdatedAt
	return nil
}

// Update updates an existing product
func (r *ProductRepositoryImpl) Update(ctx context.Context, product *entity.Product) error {
	model := mapProductDomainToModel(product)
	result := r.db.Model(&Product{}).Where("id = ?", product.ID).Updates(map[string]interface{}{
		"name":        model.Name,
		"description": model.Description,
		"price":       model.Price,
		"stock":       model.Stock,
		"image_url":   model.ImageURL,
		"category_id": model.CategoryID,
	})
	if result.Error != nil {
		return result.Error
	}
	if result.RowsAffected == 0 {
		return gorm.ErrRecordNotFound
	}
	// Update the updated_at timestamp
	product.UpdatedAt = model.UpdatedAt
	return nil
}

// Delete deletes a product by its ID
func (r *ProductRepositoryImpl) Delete(ctx context.Context, id uint) error {
	result := r.db.Delete(&Product{}, id)
	if result.Error != nil {
		return result.Error
	}
	if result.RowsAffected == 0 {
		return gorm.ErrRecordNotFound
	}
	return nil
}

// CategoryRepositoryImpl implements the CategoryRepository interface
type CategoryRepositoryImpl struct {
	db *gorm.DB
}

// NewCategoryRepository creates a new category repository implementation
func NewCategoryRepository(db *gorm.DB) repository.CategoryRepository {
	return &CategoryRepositoryImpl{
		db: db,
	}
}

// mapCategoryModelToDomain maps a category model to a domain entity
func mapCategoryModelToDomain(model *Category) *entity.Category {
	if model == nil {
		return nil
	}
	return &entity.Category{
		ID:        model.ID,
		Name:      model.Name,
		CreatedAt: model.CreatedAt,
		UpdatedAt: model.UpdatedAt,
	}
}

// mapCategoryDomainToModel maps a domain entity to a category model
func mapCategoryDomainToModel(domain *entity.Category) *Category {
	if domain == nil {
		return nil
	}
	return &Category{
		Model: gorm.Model{
			ID:        domain.ID,
			CreatedAt: domain.CreatedAt,
			UpdatedAt: domain.UpdatedAt,
		},
		Name: domain.Name,
	}
}

// FindAll retrieves all categories
func (r *CategoryRepositoryImpl) FindAll(ctx context.Context) ([]entity.Category, error) {
	var categories []Category
	if err := r.db.Find(&categories).Error; err != nil {
		return nil, err
	}

	// Map to domain entities
	result := make([]entity.Category, len(categories))
	for i, category := range categories {
		domainCategory := mapCategoryModelToDomain(&category)
		result[i] = *domainCategory
	}

	return result, nil
}

// FindByID retrieves a category by its ID
func (r *CategoryRepositoryImpl) FindByID(ctx context.Context, id uint) (*entity.Category, error) {
	var category Category
	if err := r.db.First(&category, id).Error; err != nil {
		return nil, err
	}
	return mapCategoryModelToDomain(&category), nil
}

// Create creates a new category
func (r *CategoryRepositoryImpl) Create(ctx context.Context, category *entity.Category) error {
	model := mapCategoryDomainToModel(category)
	if err := r.db.Create(model).Error; err != nil {
		return err
	}
	// Update the domain entity with generated ID and timestamps
	category.ID = model.ID
	category.CreatedAt = model.CreatedAt
	category.UpdatedAt = model.UpdatedAt
	return nil
}

// Update updates an existing category
func (r *CategoryRepositoryImpl) Update(ctx context.Context, category *entity.Category) error {
	result := r.db.Model(&Category{}).Where("id = ?", category.ID).Updates(map[string]interface{}{
		"name": category.Name,
	})
	if result.Error != nil {
		return result.Error
	}
	if result.RowsAffected == 0 {
		return gorm.ErrRecordNotFound
	}
	return nil
}

// Delete deletes a category by its ID
func (r *CategoryRepositoryImpl) Delete(ctx context.Context, id uint) error {
	result := r.db.Delete(&Category{}, id)
	if result.Error != nil {
		return result.Error
	}
	if result.RowsAffected == 0 {
		return gorm.ErrRecordNotFound
	}
	return nil
}
