package config

import (
	"fmt"
	"os"
	"strconv"
	"strings"

	"github.com/pkg/errors"
	"github.com/spf13/viper"
)

type Config struct {
	Server    ServerConfig    `mapstructure:"server"`
	Database  DatabaseConfig  `mapstructure:"database"`
	JWT       JWTConfig       `mapstructure:"jwt"`
	Kafka     KafkaConfig     `mapstructure:"kafka"`
	Telemetry TelemetryConfig `mapstructure:"telemetry"`
}

type ServerConfig struct {
	Port int    `mapstructure:"port"`
	Env  string `mapstructure:"env"`
}

type DatabaseConfig struct {
	Host     string `mapstructure:"host"`
	Port     int    `mapstructure:"port"`
	User     string `mapstructure:"user"`
	Password string `mapstructure:"password"`
	DBName   string `mapstructure:"dbname"`
	SSLMode  string `mapstructure:"sslmode"`
}

type JWTConfig struct {
	Secret      string `mapstructure:"secret"`
	ExpiryHours int    `mapstructure:"expiryHours"`
}

type KafkaConfig struct {
	Brokers       []string `mapstructure:"brokers"`
	ConsumerGroup string   `mapstructure:"consumerGroup"`
}

type TelemetryConfig struct {
	ServiceName    string `mapstructure:"serviceName"`
	TracingEnabled bool   `mapstructure:"tracingEnabled"`
}

// GetDSN returns the PostgreSQL connection string
func (d *DatabaseConfig) GetDSN() string {
	return fmt.Sprintf("host=%s port=%d user=%s password=%s dbname=%s sslmode=%s",
		d.Host, d.Port, d.User, d.Password, d.DBName, d.SSLMode)
}

// NewConfig loads the configuration from file and environment variables
func NewConfig() (*Config, error) {
	v := viper.New()

	// Set default values
	v.SetDefault("server.port", 8080)
	v.SetDefault("server.env", "development")
	v.SetDefault("database.host", "localhost")
	v.SetDefault("database.port", 5432)
	v.SetDefault("database.sslmode", "disable")
	v.SetDefault("jwt.expiryHours", 24)

	// Try to read config from file first
	v.SetConfigName("config")
	v.SetConfigType("yaml")
	v.AddConfigPath("./config/")
	if err := v.ReadInConfig(); err != nil {
		// It's okay if config file doesn't exist
		if _, ok := err.(viper.ConfigFileNotFoundError); !ok {
			return nil, errors.Wrap(err, "error reading config file")
		}
	}

	// Override with environment variables with higher priority
	v.SetEnvKeyReplacer(strings.NewReplacer(".", "_"))
	v.AutomaticEnv()

	// Check for PORT environment variable (used by Render)
	if port := os.Getenv("PORT"); port != "" {
		portValue, err := strconv.Atoi(port)
		if err == nil {
			v.Set("server.port", portValue)
		}
	}

	// Check for database environment variables from Render
	if dbHost := os.Getenv("DATABASE_HOST"); dbHost != "" {
		v.Set("database.host", dbHost)
	}

	if dbPort := os.Getenv("DATABASE_PORT"); dbPort != "" {
		portValue, err := strconv.Atoi(dbPort)
		if err == nil {
			v.Set("database.port", portValue)
		}
	}

	if dbUser := os.Getenv("DATABASE_USER"); dbUser != "" {
		v.Set("database.user", dbUser)
	}

	if dbPassword := os.Getenv("DATABASE_PASSWORD"); dbPassword != "" {
		v.Set("database.password", dbPassword)
	}

	if dbName := os.Getenv("DATABASE_NAME"); dbName != "" {
		v.Set("database.dbname", dbName)
	}

	if dbSSLMode := os.Getenv("DATABASE_SSLMODE"); dbSSLMode != "" {
		v.Set("database.sslmode", dbSSLMode)
	}

	// Check for JWT environment variables
	if jwtSecret := os.Getenv("JWT_SECRET"); jwtSecret != "" {
		v.Set("jwt.secret", jwtSecret)
	}

	if jwtExpiryHours := os.Getenv("JWT_EXPIRYHOURS"); jwtExpiryHours != "" {
		expiryValue, err := strconv.Atoi(jwtExpiryHours)
		if err == nil {
			v.Set("jwt.expiryHours", expiryValue)
		}
	}

	// Server environment
	if serverEnv := os.Getenv("SERVER_ENV"); serverEnv != "" {
		v.Set("server.env", serverEnv)
	}

	var cfg Config
	if err := v.Unmarshal(&cfg); err != nil {
		return nil, errors.Wrap(err, "error unmarshaling config")
	}

	return &cfg, nil
}
