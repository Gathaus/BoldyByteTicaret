package dto

// OrderItemRequest represents an order item in a create order request
type OrderItemRequest struct {
	ProductID uint `json:"productId" binding:"required"`
	Quantity  int  `json:"quantity" binding:"required,gt=0"`
}

// CreateOrderRequest represents a request to create an order
type CreateOrderRequest struct {
	AddressID     uint               `json:"addressId" binding:"required"`
	PaymentMethod string             `json:"paymentMethod" binding:"required"`
	OrderItems    []OrderItemRequest `json:"orderItems" binding:"required,dive"`
}

// OrderItemResponse represents an order item in API responses
type OrderItemResponse struct {
	ID        uint    `json:"id"`
	ProductID uint    `json:"productId"`
	Name      string  `json:"name"`
	Price     float64 `json:"price"`
	Quantity  int     `json:"quantity"`
	Subtotal  float64 `json:"subtotal"`
	ImageURL  string  `json:"imageUrl,omitempty"`
}

// OrderResponse represents an order in API responses
type OrderResponse struct {
	ID            uint                `json:"id"`
	UserID        uint                `json:"userId"`
	AddressID     uint                `json:"addressId"`
	TotalAmount   float64             `json:"totalAmount"`
	Status        string              `json:"status"`
	PaymentStatus string              `json:"paymentStatus"`
	PaymentMethod string              `json:"paymentMethod"`
	OrderItems    []OrderItemResponse `json:"orderItems,omitempty"`
	CreatedAt     string              `json:"createdAt"`
	UpdatedAt     string              `json:"updatedAt"`
}

// OrderListResponse represents a paginated list of orders
type OrderListResponse struct {
	Orders []OrderResponse `json:"orders"`
	Total  int64           `json:"total"`
	Page   int             `json:"page"`
	Limit  int             `json:"limit"`
}

// UpdateOrderStatusRequest represents a request to update an order status
type UpdateOrderStatusRequest struct {
	Status string `json:"status" binding:"required"`
}

// UpdatePaymentStatusRequest represents a request to update a payment status
type UpdatePaymentStatusRequest struct {
	PaymentStatus string `json:"paymentStatus" binding:"required"`
}

// CartItemRequest represents a request to add an item to the cart
type CartItemRequest struct {
	ProductID uint `json:"productId" binding:"required"`
	Quantity  int  `json:"quantity" binding:"required,gt=0"`
}

// CartResponse represents a shopping cart in API responses
type CartResponse struct {
	ID        uint                `json:"id"`
	UserID    uint                `json:"userId"`
	Items     []OrderItemResponse `json:"items"`
	SubTotal  float64             `json:"subTotal"`
	UpdatedAt string              `json:"updatedAt"`
}

// CheckoutRequest represents a request to checkout the cart
type CheckoutRequest struct {
	AddressID     uint   `json:"addressId" binding:"required"`
	PaymentMethod string `json:"paymentMethod" binding:"required"`
}
