package service

import (
	"context"

	"github.com/mert-yagci/ecommerce-api/pkg/domain/entity"
)

// UserService defines the interface for user business logic
type UserService interface {
	GetUsers(ctx context.Context, limit, offset int) ([]entity.User, int64, error)
	GetUserByID(ctx context.Context, id uint) (*entity.User, error)
	GetUserByEmail(ctx context.Context, email string) (*entity.User, error)
	CreateUser(ctx context.Context, user *entity.User) error
	UpdateUser(ctx context.Context, user *entity.User) error
	DeleteUser(ctx context.Context, id uint) error
	ChangePassword(ctx context.Context, id uint, oldPassword, newPassword string) error
}

// AuthService defines the interface for authentication business logic
type AuthService interface {
	Register(ctx context.Context, user *entity.User) error
	Login(ctx context.Context, email, password string) (string, error)
	VerifyToken(ctx context.Context, token string) (*entity.User, error)
}

// AddressService defines the interface for address business logic
type AddressService interface {
	GetAddressesByUserID(ctx context.Context, userID uint) ([]entity.Address, error)
	GetAddressByID(ctx context.Context, id uint) (*entity.Address, error)
	CreateAddress(ctx context.Context, address *entity.Address) error
	UpdateAddress(ctx context.Context, address *entity.Address) error
	DeleteAddress(ctx context.Context, id uint) error
	SetDefaultAddress(ctx context.Context, userID, addressID uint) error
}
