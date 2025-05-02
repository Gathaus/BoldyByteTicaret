package persistence

import (
	"time"

	"gorm.io/gorm"
)

// Product is the database model for products
type Product struct {
	gorm.Model
	Name        string  `gorm:"size:255;not null"`
	Description string  `gorm:"type:text"`
	Price       float64 `gorm:"not null"`
	Stock       int     `gorm:"not null"`
	ImageURL    string  `gorm:"size:255"`
	CategoryID  uint    `gorm:"index"`
	Category    Category
}

// Category is the database model for product categories
type Category struct {
	gorm.Model
	Name     string    `gorm:"size:100;not null;unique"`
	Products []Product `gorm:"foreignKey:CategoryID"`
}

// User is the database model for users
type User struct {
	gorm.Model
	Email     string    `gorm:"size:255;not null;unique;index"`
	Password  string    `gorm:"size:255;not null"`
	FirstName string    `gorm:"size:100"`
	LastName  string    `gorm:"size:100"`
	Role      string    `gorm:"size:20;default:'customer'"`
	Orders    []Order   `gorm:"foreignKey:UserID"`
	Addresses []Address `gorm:"foreignKey:UserID"`
}

// Address is the database model for user addresses
type Address struct {
	gorm.Model
	UserID    uint   `gorm:"index"`
	Street    string `gorm:"size:255;not null"`
	City      string `gorm:"size:100;not null"`
	State     string `gorm:"size:100;not null"`
	ZipCode   string `gorm:"size:20;not null"`
	Country   string `gorm:"size:100;not null"`
	IsDefault bool   `gorm:"default:false"`
}

// Order is the database model for orders
type Order struct {
	gorm.Model
	UserID        uint        `gorm:"index"`
	User          User        `gorm:"foreignKey:UserID"`
	AddressID     uint        `gorm:"index"`
	Address       Address     `gorm:"foreignKey:AddressID"`
	TotalAmount   float64     `gorm:"not null"`
	Status        string      `gorm:"size:50;default:'pending'"`
	PaymentStatus string      `gorm:"size:50;default:'pending'"`
	PaymentMethod string      `gorm:"size:50"`
	OrderItems    []OrderItem `gorm:"foreignKey:OrderID"`
}

// OrderItem is the database model for order items
type OrderItem struct {
	gorm.Model
	OrderID   uint    `gorm:"index"`
	ProductID uint    `gorm:"index"`
	Product   Product `gorm:"foreignKey:ProductID"`
	Quantity  int     `gorm:"not null"`
	Price     float64 `gorm:"not null"` // Price at the time of order
}

// Cart is the database model for a shopping cart (uses the order table with special status)
type Cart struct {
	gorm.Model
	UserID    uint       `gorm:"uniqueIndex"` // One cart per user
	User      User       `gorm:"foreignKey:UserID"`
	Items     []CartItem `gorm:"foreignKey:CartID"`
	UpdatedAt time.Time
}

// CartItem is the database model for cart items
type CartItem struct {
	gorm.Model
	CartID    uint    `gorm:"index"`
	ProductID uint    `gorm:"index"`
	Product   Product `gorm:"foreignKey:ProductID"`
	Quantity  int     `gorm:"not null"`
}
