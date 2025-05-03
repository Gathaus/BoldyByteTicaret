package handlers

import (
	"github.com/gin-gonic/gin"
)

// StaticHandler handles static file requests
type StaticHandler struct{}

// NewStaticHandler creates a new StaticHandler
func NewStaticHandler() *StaticHandler {
	return &StaticHandler{}
}

// Home serves the home page
func (h *StaticHandler) Home(c *gin.Context) {
	c.File("./web/templates/home_electronic/index.html")
}
