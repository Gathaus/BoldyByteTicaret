package repository

import (
	"context"

	"github.com/mert-yagci/ecommerce-api/pkg/domain/entity"
)

// UserRepository defines the interface for user data access
type UserRepository interface {
	FindAll(ctx context.Context, limit, offset int) ([]entity.User, int64, error)
	FindByID(ctx context.Context, id uint) (*entity.User, error)
	FindByEmail(ctx context.Context, email string) (*entity.User, error)
	Create(ctx context.Context, user *entity.User) error
	Update(ctx context.Context, user *entity.User) error
	Delete(ctx context.Context, id uint) error
}

// AddressRepository defines the interface for address data access
type AddressRepository interface {
	FindByUserID(ctx context.Context, userID uint) ([]entity.Address, error)
	FindByID(ctx context.Context, id uint) (*entity.Address, error)
	Create(ctx context.Context, address *entity.Address) error
	Update(ctx context.Context, address *entity.Address) error
	Delete(ctx context.Context, id uint) error
	SetDefault(ctx context.Context, userID, addressID uint) error
}
