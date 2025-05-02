package service

import (
	"context"
	"fmt"

	"github.com/mert-yagci/ecommerce-api/pkg/domain/entity"
	"github.com/mert-yagci/ecommerce-api/pkg/domain/repository"
	"github.com/mert-yagci/ecommerce-api/pkg/domain/service"
	"golang.org/x/crypto/bcrypt"
)

// UserServiceImpl implements the UserService interface
type UserServiceImpl struct {
	userRepo repository.UserRepository
}

// NewUserService creates a new user service implementation
func NewUserService(userRepo repository.UserRepository) service.UserService {
	return &UserServiceImpl{
		userRepo: userRepo,
	}
}

// GetUsers retrieves a paginated list of users
func (s *UserServiceImpl) GetUsers(ctx context.Context, limit, offset int) ([]entity.User, int64, error) {
	return s.userRepo.FindAll(ctx, limit, offset)
}

// GetUserByID retrieves a user by ID
func (s *UserServiceImpl) GetUserByID(ctx context.Context, id uint) (*entity.User, error) {
	return s.userRepo.FindByID(ctx, id)
}

// GetUserByEmail retrieves a user by email
func (s *UserServiceImpl) GetUserByEmail(ctx context.Context, email string) (*entity.User, error) {
	return s.userRepo.FindByEmail(ctx, email)
}

// CreateUser creates a new user
func (s *UserServiceImpl) CreateUser(ctx context.Context, user *entity.User) error {
	return s.userRepo.Create(ctx, user)
}

// UpdateUser updates an existing user
func (s *UserServiceImpl) UpdateUser(ctx context.Context, user *entity.User) error {
	return s.userRepo.Update(ctx, user)
}

// DeleteUser deletes a user by ID
func (s *UserServiceImpl) DeleteUser(ctx context.Context, id uint) error {
	return s.userRepo.Delete(ctx, id)
}

// ChangePassword changes a user's password
func (s *UserServiceImpl) ChangePassword(ctx context.Context, id uint, oldPassword, newPassword string) error {
	// Get the user
	user, err := s.userRepo.FindByID(ctx, id)
	if err != nil {
		return err
	}

	// Verify old password
	err = bcrypt.CompareHashAndPassword([]byte(user.Password), []byte(oldPassword))
	if err != nil {
		return fmt.Errorf("incorrect old password")
	}

	// Hash new password
	hashedPassword, err := bcrypt.GenerateFromPassword([]byte(newPassword), bcrypt.DefaultCost)
	if err != nil {
		return fmt.Errorf("error hashing password: %w", err)
	}

	// Update user with new password
	user.Password = string(hashedPassword)
	return s.userRepo.Update(ctx, user)
}

// AddressServiceImpl implements the AddressService interface
type AddressServiceImpl struct {
	addressRepo repository.AddressRepository
}

// NewAddressService creates a new address service implementation
func NewAddressService(addressRepo repository.AddressRepository) service.AddressService {
	return &AddressServiceImpl{
		addressRepo: addressRepo,
	}
}

// GetAddressesByUserID retrieves addresses for a user
func (s *AddressServiceImpl) GetAddressesByUserID(ctx context.Context, userID uint) ([]entity.Address, error) {
	return s.addressRepo.FindByUserID(ctx, userID)
}

// GetAddressByID retrieves an address by ID
func (s *AddressServiceImpl) GetAddressByID(ctx context.Context, id uint) (*entity.Address, error) {
	return s.addressRepo.FindByID(ctx, id)
}

// CreateAddress creates a new address
func (s *AddressServiceImpl) CreateAddress(ctx context.Context, address *entity.Address) error {
	return s.addressRepo.Create(ctx, address)
}

// UpdateAddress updates an existing address
func (s *AddressServiceImpl) UpdateAddress(ctx context.Context, address *entity.Address) error {
	return s.addressRepo.Update(ctx, address)
}

// DeleteAddress deletes an address by ID
func (s *AddressServiceImpl) DeleteAddress(ctx context.Context, id uint) error {
	return s.addressRepo.Delete(ctx, id)
}

// SetDefaultAddress sets an address as the default for a user
func (s *AddressServiceImpl) SetDefaultAddress(ctx context.Context, userID, addressID uint) error {
	return s.addressRepo.SetDefault(ctx, userID, addressID)
}
