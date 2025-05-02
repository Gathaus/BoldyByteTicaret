package logger

import (
	"os"

	"github.com/sirupsen/logrus"
)

// Setup initializes the logger
func Setup(environment string) *logrus.Logger {
	logger := logrus.New()

	// Set logger output
	logger.SetOutput(os.Stdout)

	// Set log format based on environment
	if environment == "production" {
		logger.SetFormatter(&logrus.JSONFormatter{})
	} else {
		logger.SetFormatter(&logrus.TextFormatter{
			FullTimestamp: true,
		})
	}

	// Set log level
	switch environment {
	case "development":
		logger.SetLevel(logrus.DebugLevel)
	case "test":
		logger.SetLevel(logrus.WarnLevel)
	default:
		logger.SetLevel(logrus.InfoLevel)
	}

	return logger
}
