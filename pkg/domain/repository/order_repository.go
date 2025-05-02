package repository

import (
	"context"

	"github.com/mert-yagci/ecommerce-api/pkg/domain/entity"
)

// OrderRepository defines the interface for order data access
type OrderRepository interface {
	FindAll(ctx context.Context, limit, offset int) ([]entity.Order, int64, error)
	FindByID(ctx context.Context, id uint) (*entity.Order, error)
	FindByUserID(ctx context.Context, userID uint, limit, offset int) ([]entity.Order, int64, error)
	Create(ctx context.Context, order *entity.Order) error
	Update(ctx context.Context, order *entity.Order) error
	Delete(ctx context.Context, id uint) error
	UpdateStatus(ctx context.Context, id uint, status string) error
	UpdatePaymentStatus(ctx context.Context, id uint, paymentStatus string) error
}

// OrderItemRepository defines the interface for order item data access
type OrderItemRepository interface {
	FindByOrderID(ctx context.Context, orderID uint) ([]entity.OrderItem, error)
	Create(ctx context.Context, orderItem *entity.OrderItem) error
	BulkCreate(ctx context.Context, orderItems []entity.OrderItem) error
	Update(ctx context.Context, orderItem *entity.OrderItem) error
	Delete(ctx context.Context, id uint) error
}
