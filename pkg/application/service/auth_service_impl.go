package service

import (
	"context"
	"fmt"
	"time"

	"github.com/golang-jwt/jwt/v5"
	"github.com/mert-yagc
	"github.com/mert-yagci/ecommerce-api/internal/config"
	"github.com/mert-yagci/ecommerce-api/pkg/domain/entity"
	"github.com/mert-yagci/ecommerce-api/pkg/domain/repository"
	"github.com/golang-jwt/jwt/v5"
	"golang.org/x/crypto/bcrypt"
)

// AuthServiceImpl implements the AuthService interface
type AuthServiceImpl struct {
	userRepo repository.UserRepository
	config   *config.Config
}

// CustomClaims represents JWT claims with user ID
type CustomClaims struct {
	UserID uint `json:"userId"`
	jwt.RegisteredClaims
}

// NewAuthService creates a new authentication service implementation
func NewAuthService(userRepo repository.UserRepository, config *config.Config) service.AuthService {
	return &AuthServiceImpl{
		userRepo: userRepo,
		config:   config,
	}
}

// Register registers a new user
func (s *AuthServiceImpl) Register(ctx context.Context, user *entity.User) error {
	// Check if user with this email already exists
	existingUser, err := s.userRepo.FindByEmail(ctx, user.Email)
	if err == nil && existingUser != nil {
		return fmt.Errorf("email already registered")
	}

	// Hash the password
	hashedPassword, err := bcrypt.GenerateFromPassword([]byte(user.Password), bcrypt.DefaultCost)
	if err != nil {
		return fmt.Errorf("error hashing password: %w", err)
	}
	user.Password = string(hashedPassword)

	// Set created/updated timestamps
	now := time.Now()
	user.CreatedAt = now
	user.UpdatedAt = now

	// Set default role if not set
	if user.Role == "" {
		user.Role = "customer"
	}

	// Create the user
	return s.userRepo.Create(ctx, user)
}

// Login authenticates a user and returns a JWT token
func (s *AuthServiceImpl) Login(ctx context.Context, email, password string) (string, error) {
	// Find the user by email
	user, err := s.userRepo.FindByEmail(ctx, email)
	if err != nil {
		return "", fmt.Errorf("invalid email or password")
	}

	// Compare passwords
	err = bcrypt.CompareHashAndPassword([]byte(user.Password), []byte(password))
	if err != nil {
		return "", fmt.Errorf("invalid email or password")
	}

	// Generate the JWT token
	token, err := s.generateJWT(user)
	if err != nil {
		return "", fmt.Errorf("error generating token: %w", err)
	}

	return token, nil
}

// VerifyToken verifies a JWT token and returns the user
func (s *AuthServiceImpl) VerifyToken(ctx context.Context, tokenString string) (*entity.User, error) {
	// Parse and validate the token
	token, err := jwt.ParseWithClaims(tokenString, &CustomClaims{}, func(token *jwt.Token) (interface{}, error) {
		// Validate the signing method
		if _, ok := token.Method.(*jwt.SigningMethodHMAC); !ok {
			return nil, fmt.Errorf("unexpected signing method: %v", token.Header["alg"])
		}
		return []byte(s.config.JWT.Secret), nil
	})

	if err != nil {
		return nil, fmt.Errorf("invalid token: %w", err)
	}

	// Extract the claims
	claims, ok := token.Claims.(*CustomClaims)
	if !ok || !token.Valid {
		return nil, fmt.Errorf("invalid token claims")
	}

	// Find the user by ID
	user, err := s.userRepo.FindByID(ctx, claims.UserID)
	if err != nil {
		return nil, fmt.Errorf("user not found: %w", err)
	}

	return user, nil
}

// generateJWT generates a JWT token for a user
func (s *AuthServiceImpl) generateJWT(user *entity.User) (string, error) {
	// Set expiration time
	expiresAt := time.Now().Add(time.Hour * time.Duration(s.config.JWT.ExpiryHours))

	// Create the claims
	claims := &CustomClaims{
		UserID: user.ID,
		RegisteredClaims: jwt.RegisteredClaims{
			Subject:   user.Email,
			ExpiresAt: jwt.NewNumericDate(expiresAt),
			IssuedAt:  jwt.NewNumericDate(time.Now()),
			Issuer:    "ecommerce-api",
		},
	}

	// Create the token
	token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)

	// Sign the token
	tokenString, err := token.SignedString([]byte(s.config.JWT.Secret))
	if err != nil {
		return "", err
	}

	return tokenString, nil
}
