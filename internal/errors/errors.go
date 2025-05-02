package errors

import (
	"net/http"

	"github.com/gin-gonic/gin"
	"github.com/pkg/errors"
)

// AppError represents an application error
type AppError struct {
	StatusCode int    `json:"-"`
	Code       string `json:"code"`
	Message    string `json:"message"`
}

// Error implements the error interface
func (e AppError) Error() string {
	return e.Message
}

// Common error types
var (
	ErrInvalidInput       = AppError{StatusCode: http.StatusBadRequest, Code: "INVALID_INPUT", Message: "Invalid input"}
	ErrUnauthorized       = AppError{StatusCode: http.StatusUnauthorized, Code: "UNAUTHORIZED", Message: "Unauthorized"}
	ErrForbidden          = AppError{StatusCode: http.StatusForbidden, Code: "FORBIDDEN", Message: "Forbidden"}
	ErrNotFound           = AppError{StatusCode: http.StatusNotFound, Code: "NOT_FOUND", Message: "Resource not found"}
	ErrInternal           = AppError{StatusCode: http.StatusInternalServerError, Code: "INTERNAL_ERROR", Message: "Internal server error"}
	ErrServiceUnavailable = AppError{StatusCode: http.StatusServiceUnavailable, Code: "SERVICE_UNAVAILABLE", Message: "Service unavailable"}
)

// WithMessage returns an error with the specified message
func WithMessage(err error, message string) error {
	return errors.WithMessage(err, message)
}

// Wrap wraps an error with a message
func Wrap(err error, message string) error {
	return errors.Wrap(err, message)
}

// IsAppError checks if an error is an AppError
func IsAppError(err error) bool {
	var appErr AppError
	return errors.As(err, &appErr)
}

// AsAppError converts an error to an AppError
func AsAppError(err error) AppError {
	var appErr AppError
	if errors.As(err, &appErr) {
		return appErr
	}
	return ErrInternal
}

// HandleError handles errors in Gin context
func HandleError(c *gin.Context, err error) {
	appErr := AsAppError(err)
	c.JSON(appErr.StatusCode, gin.H{
		"code":    appErr.Code,
		"message": appErr.Message,
	})
}
