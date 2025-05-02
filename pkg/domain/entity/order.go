package entity

import (
	"time"
)

// Order represents an order entity in the domain
type Order struct {
	ID            uint        `json:"id"`
	UserID        uint        `json:"userId"`
	AddressID     uint        `json:"addressId"`
	TotalAmount   float64     `json:"totalAmount"`
	Status        string      `json:"status"`        // e.g., "pending", "processing", "shipped", "delivered", "cancelled"
	PaymentStatus string      `json:"paymentStatus"` // e.g., "pending", "paid", "failed"
	PaymentMethod string      `json:"paymentMethod"` // e.g., "credit_card", "paypal"
	OrderItems    []OrderItem `json:"orderItems,omitempty"`
	CreatedAt     time.Time   `json:"createdAt"`
	UpdatedAt     time.Time   `json:"updatedAt"`
}

// OrderItem represents an item in an order
type OrderItem struct {
	ID        uint      `json:"id"`
	OrderID   uint      `json:"orderId"`
	ProductID uint      `json:"productId"`
	Quantity  int       `json:"quantity"`
	Price     float64   `json:"price"` // Price at the time of order
	CreatedAt time.Time `json:"createdAt"`
	UpdatedAt time.Time `json:"updatedAt"`
}
