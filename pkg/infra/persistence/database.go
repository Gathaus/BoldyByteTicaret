package persistence

import (
	"fmt"

	"github.com/mert-yagci/ecommerce-api/internal/config"
	"github.com/mert-yagci/ecommerce-api/pkg/domain/entity"
	"github.com/sirupsen/logrus"
	"gorm.io/driver/postgres"
	"gorm.io/gorm"
)

// InitDB initializes the database connection
func InitDB(cfg *config.Config) (*gorm.DB, error) {
	dsn := cfg.Database.GetDSN()

	// Open database connection
	db, err := gorm.Open(postgres.Open(dsn), &gorm.Config{})
	if err != nil {
		return nil, fmt.Errorf("failed to connect to database: %w", err)
	}

	// Get underlying SQL DB
	sqlDB, err := db.DB()
	if err != nil {
		return nil, fmt.Errorf("failed to get sql.DB: %w", err)
	}

	// Set connection pool settings
	sqlDB.SetMaxIdleConns(10)
	sqlDB.SetMaxOpenConns(100)

	return db, nil
}

// MigrateDB performs auto-migration for database models
// This is only for development, use golang-migrate for production
func MigrateDB(db *gorm.DB) error {
	// Initialize logger
	logger := logrus.New()

	// Check if tables already exist
	var hasProductsTable, hasCategoriesTable, hasUsersTable bool

	// Check if Products table exists
	db.Raw("SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_schema = CURRENT_SCHEMA() AND table_name = 'products')").Scan(&hasProductsTable)

	// Check if Categories table exists
	db.Raw("SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_schema = CURRENT_SCHEMA() AND table_name = 'categories')").Scan(&hasCategoriesTable)

	// Check if Users table exists
	db.Raw("SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_schema = CURRENT_SCHEMA() AND table_name = 'users')").Scan(&hasUsersTable)

	// If tables don't exist, run migrations and seed data
	if !hasProductsTable || !hasCategoriesTable || !hasUsersTable {
		logger.Info("Running database migrations...")

		err := db.AutoMigrate(
			&entity.Product{},
			&entity.Category{},
			&entity.User{},
			&entity.Order{},
			&entity.OrderItem{},
		)
		if err != nil {
			return err
		}

		// Seed data only if tables were just created
		if err := seedDatabase(db, logger); err != nil {
			return fmt.Errorf("failed to seed database: %w", err)
		}
	} else {
		logger.Info("Database tables already exist, skipping migrations and seeding.")
	}

	return nil
}

// seedDatabase populates the database with initial seed data
func seedDatabase(db *gorm.DB, logger *logrus.Logger) error {
	// Check if categories already exist
	var categoryCount int64
	db.Model(&entity.Category{}).Count(&categoryCount)

	if categoryCount > 0 {
		logger.Info("Database already has seed data. Skipping seeding.")
		return nil
	}

	// Begin a transaction
	tx := db.Begin()
	if tx.Error != nil {
		return tx.Error
	}

	// Create categories
	categories := []entity.Category{
		{Name: "Asansör Kabinleri"},
		{Name: "Asansör Motorları"},
		{Name: "Asansör Kapıları"},
		{Name: "Kumanda Sistemleri"},
		{Name: "Güvenlik Sistemleri"},
		{Name: "Asansör Rayları"},
		{Name: "Asansör Aksesuarları"},
		{Name: "Bakım Ürünleri"},
	}

	if err := tx.Create(&categories).Error; err != nil {
		tx.Rollback()
		return err
	}

	// Create products
	products := []entity.Product{
		// Asansör Kabinleri
		{
			Name:        "Lüks Panoramik Kabin",
			Description: "360 derece panoramik görüş açısına sahip, lüks cam asansör kabini",
			Price:       125000.00,
			Stock:       5,
			CategoryID:  1,
			ImageURL:    "panoramic_cabin.jpg",
		},
		{
			Name:        "Standart Rezidans Kabini",
			Description: "Rezidans ve apartmanlar için standart paslanmaz çelik asansör kabini",
			Price:       45000.00,
			Stock:       15,
			CategoryID:  1,
			ImageURL:    "standard_cabin.jpg",
		},
		{
			Name:        "Hastane Kabini",
			Description: "Geniş, sedye taşımaya uygun, antibakteriyel hastane asansör kabini",
			Price:       68000.00,
			Stock:       8,
			CategoryID:  1,
			ImageURL:    "hospital_cabin.jpg",
		},

		// Asansör Motorları
		{
			Name:        "YTG-180 Dişlisiz Motor",
			Description: "180kg kapasiteli, enerji verimli dişlisiz asansör motoru",
			Price:       32500.00,
			Stock:       12,
			CategoryID:  2,
			ImageURL:    "ytg180_motor.jpg",
		},
		{
			Name:        "HT-500 Yüksek Kapasiteli Motor",
			Description: "500kg yük kapasiteli, endüstriyel kullanım için asansör motoru",
			Price:       78000.00,
			Stock:       7,
			CategoryID:  2,
			ImageURL:    "ht500_motor.jpg",
		},
		{
			Name:        "ECO-250 Enerji Tasarruflu Motor",
			Description: "250kg kapasiteli, %40 enerji tasarrufu sağlayan asansör motoru",
			Price:       42000.00,
			Stock:       10,
			CategoryID:  2,
			ImageURL:    "eco250_motor.jpg",
		},

		// Asansör Kapıları
		{
			Name:        "Otomatik Teleskopik Kapı",
			Description: "2 panelli otomatik teleskopik asansör kapısı, paslanmaz çelik",
			Price:       12800.00,
			Stock:       20,
			CategoryID:  3,
			ImageURL:    "telescopic_door.jpg",
		},
		{
			Name:        "Cam Otomatik Kapı Sistemi",
			Description: "Şeffaf temperli cam, otomatik asansör kapı sistemi",
			Price:       18500.00,
			Stock:       8,
			CategoryID:  3,
			ImageURL:    "glass_door.jpg",
		},
		{
			Name:        "Yangına Dayanıklı Katlanır Kapı",
			Description: "120 dakika yangına dayanıklı, katlanır acil durum asansör kapısı",
			Price:       21000.00,
			Stock:       6,
			CategoryID:  3,
			ImageURL:    "fire_door.jpg",
		},

		// Kumanda Sistemleri
		{
			Name:        "Akıllı Kontrol Panosu",
			Description: "Dokunmatik ekranlı, uzaktan izlenebilir akıllı asansör kontrol panosu",
			Price:       28500.00,
			Stock:       10,
			CategoryID:  4,
			ImageURL:    "smart_control.jpg",
		},
		{
			Name:        "Standart Kumanda Panosu",
			Description: "8 kata kadar standart asansör kumanda panosu",
			Price:       9800.00,
			Stock:       25,
			CategoryID:  4,
			ImageURL:    "standard_control.jpg",
		},
		{
			Name:        "Yüksek Bina Kontrol Sistemi",
			Description: "30+ kat için özel tasarlanmış yüksek performanslı kumanda sistemi",
			Price:       42000.00,
			Stock:       5,
			CategoryID:  4,
			ImageURL:    "highrise_control.jpg",
		},

		// Güvenlik Sistemleri
		{
			Name:        "Hız Regülatörü",
			Description: "EN 81-20 standartlarına uygun, hassas asansör hız regülatörü",
			Price:       5800.00,
			Stock:       18,
			CategoryID:  5,
			ImageURL:    "speed_regulator.jpg",
		},
		{
			Name:        "Paraşüt Fren Sistemi",
			Description: "Acil durum paraşüt fren sistemi, 1000kg kapasiteli",
			Price:       7200.00,
			Stock:       15,
			CategoryID:  5,
			ImageURL:    "parachute_brake.jpg",
		},
		{
			Name:        "Güvenlik Fotoseli",
			Description: "Kızılötesi sensörlü, 16 noktalı asansör güvenlik fotoseli",
			Price:       1200.00,
			Stock:       30,
			CategoryID:  5,
			ImageURL:    "safety_photocell.jpg",
		},

		// Asansör Rayları
		{
			Name:        "T90 Standart Ray",
			Description: "T90/B kalitede, 5 metre standart asansör rayı",
			Price:       4500.00,
			Stock:       40,
			CategoryID:  6,
			ImageURL:    "t90_rail.jpg",
		},
		{
			Name:        "T127 Ağır Hizmet Rayı",
			Description: "T127/B kalitede, yüksek yük kapasiteli 5 metre asansör rayı",
			Price:       7800.00,
			Stock:       25,
			CategoryID:  6,
			ImageURL:    "t127_rail.jpg",
		},
		{
			Name:        "T75 Hafif Ray",
			Description: "T75/B kalitede, ev asansörleri için 3 metre asansör rayı",
			Price:       2800.00,
			Stock:       35,
			CategoryID:  6,
			ImageURL:    "t75_rail.jpg",
		},

		// Asansör Aksesuarları
		{
			Name:        "LED Kabin Aydınlatma Kiti",
			Description: "Enerji tasarruflu, dokunmatik kontrollü LED kabin aydınlatma sistemi",
			Price:       1800.00,
			Stock:       22,
			CategoryID:  7,
			ImageURL:    "led_lighting.jpg",
		},
		{
			Name:        "Kabin Tutamacı",
			Description: "Paslanmaz çelik, ergonomik tasarımlı kabin tutamacı, 1 metre",
			Price:       450.00,
			Stock:       50,
			CategoryID:  7,
			ImageURL:    "handrail.jpg",
		},
		{
			Name:        "Dijital Kat Göstergesi",
			Description: "7 inç renkli LCD ekranlı, sesli uyarı sistemli dijital kat göstergesi",
			Price:       2200.00,
			Stock:       15,
			CategoryID:  7,
			ImageURL:    "floor_indicator.jpg",
		},

		// Bakım Ürünleri
		{
			Name:        "Ray Yağlama Sistemi",
			Description: "Otomatik ray yağlama sistemi, 5 litre özel ray yağı dahil",
			Price:       3800.00,
			Stock:       10,
			CategoryID:  8,
			ImageURL:    "rail_lubrication.jpg",
		},
		{
			Name:        "Asansör Bakım Kiti",
			Description: "Profesyonel teknisyenler için komple asansör bakım kiti",
			Price:       5200.00,
			Stock:       8,
			CategoryID:  8,
			ImageURL:    "maintenance_kit.jpg",
		},
		{
			Name:        "Kabin Temizlik Seti",
			Description: "Kabin iç yüzeyleri için özel kimyasallar ve temizleme malzemeleri seti",
			Price:       680.00,
			Stock:       25,
			CategoryID:  8,
			ImageURL:    "cleaning_kit.jpg",
		},
	}

	if err := tx.Create(&products).Error; err != nil {
		tx.Rollback()
		return err
	}

	// Create admin user
	adminUser := entity.User{
		Email:     "admin@example.com",
		Password:  "$2a$10$GmMLLYDkx3gNLgf1.ujkqOwuK5rdwRx7OKbL1yjp0F3hry2VfQANi", // hashed "password123"
		FirstName: "Admin",
		LastName:  "User",
		Role:      "admin",
	}

	if err := tx.Create(&adminUser).Error; err != nil {
		tx.Rollback()
		return err
	}

	// Create regular user
	regularUser := entity.User{
		Email:     "user@example.com",
		Password:  "$2a$10$GmMLLYDkx3gNLgf1.ujkqOwuK5rdwRx7OKbL1yjp0F3hry2VfQANi", // hashed "password123"
		FirstName: "Regular",
		LastName:  "User",
		Role:      "user",
	}

	if err := tx.Create(&regularUser).Error; err != nil {
		tx.Rollback()
		return err
	}

	// Commit the transaction
	return tx.Commit().Error
}
