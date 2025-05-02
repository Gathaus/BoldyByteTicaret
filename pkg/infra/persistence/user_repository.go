package persistence

import (
	"context"

	"github.com/mert-yagci/ecommerce-api/pkg/domain/entity"
	"github.com/mert-yagci/ecommerce-api/pkg/domain/repository"
	"gorm.io/gorm"
)

// UserRepositoryImpl implements the UserRepository interface
type UserRepositoryImpl struct {
	db *gorm.DB
}

// NewUserRepository creates a new user repository implementation
func NewUserRepository(db *gorm.DB) repository.UserRepository {
	return &UserRepositoryImpl{
		db: db,
	}
}

// mapUserModelToDomain maps a user model to a domain entity
func mapUserModelToDomain(model *User) *entity.User {
	if model == nil {
		return nil
	}
	return &entity.User{
		ID:        model.ID,
		Email:     model.Email,
		Password:  model.Password,
		FirstName: model.FirstName,
		LastName:  model.LastName,
		Role:      model.Role,
		CreatedAt: model.CreatedAt,
		UpdatedAt: model.UpdatedAt,
	}
}

// mapUserDomainToModel maps a domain entity to a user model
func mapUserDomainToModel(domain *entity.User) *User {
	if domain == nil {
		return nil
	}
	return &User{
		Model: gorm.Model{
			ID:        domain.ID,
			CreatedAt: domain.CreatedAt,
			UpdatedAt: domain.UpdatedAt,
		},
		Email:     domain.Email,
		Password:  domain.Password,
		FirstName: domain.FirstName,
		LastName:  domain.LastName,
		Role:      domain.Role,
	}
}

// FindAll retrieves a paginated list of users
func (r *UserRepositoryImpl) FindAll(ctx context.Context, limit, offset int) ([]entity.User, int64, error) {
	var users []User
	var count int64

	// Count total users
	if err := r.db.Model(&User{}).Count(&count).Error; err != nil {
		return nil, 0, err
	}

	// Retrieve paginated users
	if err := r.db.Limit(limit).Offset(offset).Find(&users).Error; err != nil {
		return nil, 0, err
	}

	// Map to domain entities
	result := make([]entity.User, len(users))
	for i, user := range users {
		domainUser := mapUserModelToDomain(&user)
		result[i] = *domainUser
	}

	return result, count, nil
}

// FindByID retrieves a user by ID
func (r *UserRepositoryImpl) FindByID(ctx context.Context, id uint) (*entity.User, error) {
	var user User
	if err := r.db.First(&user, id).Error; err != nil {
		return nil, err
	}
	return mapUserModelToDomain(&user), nil
}

// FindByEmail retrieves a user by email
func (r *UserRepositoryImpl) FindByEmail(ctx context.Context, email string) (*entity.User, error) {
	var user User
	if err := r.db.Where("email = ?", email).First(&user).Error; err != nil {
		return nil, err
	}
	return mapUserModelToDomain(&user), nil
}

// Create creates a new user
func (r *UserRepositoryImpl) Create(ctx context.Context, user *entity.User) error {
	model := mapUserDomainToModel(user)
	if err := r.db.Create(model).Error; err != nil {
		return err
	}
	// Update the domain entity with generated ID and timestamps
	user.ID = model.ID
	user.CreatedAt = model.CreatedAt
	user.UpdatedAt = model.UpdatedAt
	return nil
}

// Update updates an existing user
func (r *UserRepositoryImpl) Update(ctx context.Context, user *entity.User) error {
	model := mapUserDomainToModel(user)
	result := r.db.Model(&User{}).Where("id = ?", user.ID).Updates(map[string]interface{}{
		"email":      model.Email,
		"first_name": model.FirstName,
		"last_name":  model.LastName,
		"role":       model.Role,
	})
	if result.Error != nil {
		return result.Error
	}
	if result.RowsAffected == 0 {
		return gorm.ErrRecordNotFound
	}
	return nil
}

// Delete deletes a user by ID
func (r *UserRepositoryImpl) Delete(ctx context.Context, id uint) error {
	result := r.db.Delete(&User{}, id)
	if result.Error != nil {
		return result.Error
	}
	if result.RowsAffected == 0 {
		return gorm.ErrRecordNotFound
	}
	return nil
}

// AddressRepositoryImpl implements the AddressRepository interface
type AddressRepositoryImpl struct {
	db *gorm.DB
}

// NewAddressRepository creates a new address repository implementation
func NewAddressRepository(db *gorm.DB) repository.AddressRepository {
	return &AddressRepositoryImpl{
		db: db,
	}
}

// mapAddressModelToDomain maps an address model to a domain entity
func mapAddressModelToDomain(model *Address) *entity.Address {
	if model == nil {
		return nil
	}
	return &entity.Address{
		ID:        model.ID,
		UserID:    model.UserID,
		Street:    model.Street,
		City:      model.City,
		State:     model.State,
		ZipCode:   model.ZipCode,
		Country:   model.Country,
		IsDefault: model.IsDefault,
		CreatedAt: model.CreatedAt,
		UpdatedAt: model.UpdatedAt,
	}
}

// mapAddressDomainToModel maps a domain entity to an address model
func mapAddressDomainToModel(domain *entity.Address) *Address {
	if domain == nil {
		return nil
	}
	return &Address{
		Model: gorm.Model{
			ID:        domain.ID,
			CreatedAt: domain.CreatedAt,
			UpdatedAt: domain.UpdatedAt,
		},
		UserID:    domain.UserID,
		Street:    domain.Street,
		City:      domain.City,
		State:     domain.State,
		ZipCode:   domain.ZipCode,
		Country:   domain.Country,
		IsDefault: domain.IsDefault,
	}
}

// FindByUserID retrieves addresses for a user
func (r *AddressRepositoryImpl) FindByUserID(ctx context.Context, userID uint) ([]entity.Address, error) {
	var addresses []Address
	if err := r.db.Where("user_id = ?", userID).Find(&addresses).Error; err != nil {
		return nil, err
	}

	// Map to domain entities
	result := make([]entity.Address, len(addresses))
	for i, address := range addresses {
		domainAddress := mapAddressModelToDomain(&address)
		result[i] = *domainAddress
	}

	return result, nil
}

// FindByID retrieves an address by ID
func (r *AddressRepositoryImpl) FindByID(ctx context.Context, id uint) (*entity.Address, error) {
	var address Address
	if err := r.db.First(&address, id).Error; err != nil {
		return nil, err
	}
	return mapAddressModelToDomain(&address), nil
}

// Create creates a new address
func (r *AddressRepositoryImpl) Create(ctx context.Context, address *entity.Address) error {
	model := mapAddressDomainToModel(address)

	// If this is the default address, clear other defaults for this user
	if model.IsDefault {
		if err := r.db.Model(&Address{}).Where("user_id = ?", model.UserID).Update("is_default", false).Error; err != nil {
			return err
		}
	}

	if err := r.db.Create(model).Error; err != nil {
		return err
	}

	// Update the domain entity with generated ID and timestamps
	address.ID = model.ID
	address.CreatedAt = model.CreatedAt
	address.UpdatedAt = model.UpdatedAt
	return nil
}

// Update updates an existing address
func (r *AddressRepositoryImpl) Update(ctx context.Context, address *entity.Address) error {
	model := mapAddressDomainToModel(address)

	// If this is being set as default, clear other defaults for this user
	if model.IsDefault {
		if err := r.db.Model(&Address{}).Where("user_id = ? AND id != ?", model.UserID, model.ID).Update("is_default", false).Error; err != nil {
			return err
		}
	}

	result := r.db.Model(&Address{}).Where("id = ?", address.ID).Updates(map[string]interface{}{
		"street":     model.Street,
		"city":       model.City,
		"state":      model.State,
		"zip_code":   model.ZipCode,
		"country":    model.Country,
		"is_default": model.IsDefault,
	})

	if result.Error != nil {
		return result.Error
	}
	if result.RowsAffected == 0 {
		return gorm.ErrRecordNotFound
	}
	return nil
}

// Delete deletes an address by ID
func (r *AddressRepositoryImpl) Delete(ctx context.Context, id uint) error {
	result := r.db.Delete(&Address{}, id)
	if result.Error != nil {
		return result.Error
	}
	if result.RowsAffected == 0 {
		return gorm.ErrRecordNotFound
	}
	return nil
}

// SetDefault sets an address as the default for a user
func (r *AddressRepositoryImpl) SetDefault(ctx context.Context, userID, addressID uint) error {
	// Begin a transaction
	tx := r.db.Begin()
	if tx.Error != nil {
		return tx.Error
	}

	// Clear any existing default addresses
	if err := tx.Model(&Address{}).Where("user_id = ?", userID).Update("is_default", false).Error; err != nil {
		tx.Rollback()
		return err
	}

	// Set the new default address
	result := tx.Model(&Address{}).Where("id = ? AND user_id = ?", addressID, userID).Update("is_default", true)
	if result.Error != nil {
		tx.Rollback()
		return result.Error
	}

	if result.RowsAffected == 0 {
		tx.Rollback()
		return gorm.ErrRecordNotFound
	}

	return tx.Commit().Error
}
