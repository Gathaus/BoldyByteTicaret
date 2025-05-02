package service

import (
	"context"

	"github.com/mert-yagci/ecommerce-api/pkg/domain/entity"
)

// OrderService defines the interface for order business logic
type OrderService interface {
	GetOrders(ctx context.Context, limit, offset int) ([]entity.Order, int64, error)
	GetOrderByID(ctx context.Context, id uint) (*entity.Order, error)
	GetOrdersByUserID(ctx context.Context, userID uint, limit, offset int) ([]entity.Order, int64, error)
	CreateOrder(ctx context.Context, order *entity.Order) error
	UpdateOrder(ctx context.Context, order *entity.Order) error
	DeleteOrder(ctx context.Context, id uint) error
	UpdateOrderStatus(ctx context.Context, id uint, status string) error
	UpdatePaymentStatus(ctx context.Context, id uint, paymentStatus string) error
	ProcessPayment(ctx context.Context, orderID uint, paymentMethod string, paymentDetails map[string]interface{}) error
}

// CartService defines the interface for shopping cart business logic
type CartService interface {
	GetCart(ctx context.Context, userID uint) (*entity.Order, error)
	AddToCart(ctx context.Context, userID, productID uint, quantity int) error
	UpdateCartItem(ctx context.Context, userID, productID uint, quantity int) error
	RemoveFromCart(ctx context.Context, userID, productID uint) error
	ClearCart(ctx context.Context, userID uint) error
	CheckoutCart(ctx context.Context, userID, addressID uint, paymentMethod string) (*entity.Order, error)
}
