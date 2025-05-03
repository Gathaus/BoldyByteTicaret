-- Seed Categories
INSERT INTO categories (name) VALUES 
    ('Asansör Kabinleri'),
    ('Asansör Motorları'),
    ('Asansör Kapıları'),
    ('Kumanda Sistemleri'),
    ('Güvenlik Sistemleri'),
    ('Asansör Rayları'),
    ('Asansör Aksesuarları'),
    ('Bakım Ürünleri');

-- Seed Products
-- Asansör Kabinleri
INSERT INTO products (name, description, price, stock, image_url, category_id) VALUES
    ('Lüks Panoramik Kabin', '360 derece panoramik görüş açısına sahip, lüks cam asansör kabini', 125000.00, 5, 'panoramic_cabin.jpg', 1),
    ('Standart Rezidans Kabini', 'Rezidans ve apartmanlar için standart paslanmaz çelik asansör kabini', 45000.00, 15, 'standard_cabin.jpg', 1),
    ('Hastane Kabini', 'Geniş, sedye taşımaya uygun, antibakteriyel hastane asansör kabini', 68000.00, 8, 'hospital_cabin.jpg', 1);

-- Asansör Motorları
INSERT INTO products (name, description, price, stock, image_url, category_id) VALUES
    ('YTG-180 Dişlisiz Motor', '180kg kapasiteli, enerji verimli dişlisiz asansör motoru', 32500.00, 12, 'ytg180_motor.jpg', 2),
    ('HT-500 Yüksek Kapasiteli Motor', '500kg yük kapasiteli, endüstriyel kullanım için asansör motoru', 78000.00, 7, 'ht500_motor.jpg', 2),
    ('ECO-250 Enerji Tasarruflu Motor', '250kg kapasiteli, %40 enerji tasarrufu sağlayan asansör motoru', 42000.00, 10, 'eco250_motor.jpg', 2);

-- Asansör Kapıları
INSERT INTO products (name, description, price, stock, image_url, category_id) VALUES
    ('Otomatik Teleskopik Kapı', '2 panelli otomatik teleskopik asansör kapısı, paslanmaz çelik', 12800.00, 20, 'telescopic_door.jpg', 3),
    ('Cam Otomatik Kapı Sistemi', 'Şeffaf temperli cam, otomatik asansör kapı sistemi', 18500.00, 8, 'glass_door.jpg', 3),
    ('Yangına Dayanıklı Katlanır Kapı', '120 dakika yangına dayanıklı, katlanır acil durum asansör kapısı', 21000.00, 6, 'fire_door.jpg', 3);

-- Kumanda Sistemleri
INSERT INTO products (name, description, price, stock, image_url, category_id) VALUES
    ('Akıllı Kontrol Panosu', 'Dokunmatik ekranlı, uzaktan izlenebilir akıllı asansör kontrol panosu', 28500.00, 10, 'smart_control.jpg', 4),
    ('Standart Kumanda Panosu', '8 kata kadar standart asansör kumanda panosu', 9800.00, 25, 'standard_control.jpg', 4),
    ('Yüksek Bina Kontrol Sistemi', '30+ kat için özel tasarlanmış yüksek performanslı kumanda sistemi', 42000.00, 5, 'highrise_control.jpg', 4);

-- Güvenlik Sistemleri
INSERT INTO products (name, description, price, stock, image_url, category_id) VALUES
    ('Hız Regülatörü', 'EN 81-20 standartlarına uygun, hassas asansör hız regülatörü', 5800.00, 18, 'speed_regulator.jpg', 5),
    ('Paraşüt Fren Sistemi', 'Acil durum paraşüt fren sistemi, 1000kg kapasiteli', 7200.00, 15, 'parachute_brake.jpg', 5),
    ('Güvenlik Fotoseli', 'Kızılötesi sensörlü, 16 noktalı asansör güvenlik fotoseli', 1200.00, 30, 'safety_photocell.jpg', 5);

-- Asansör Rayları
INSERT INTO products (name, description, price, stock, image_url, category_id) VALUES
    ('T90 Standart Ray', 'T90/B kalitede, 5 metre standart asansör rayı', 4500.00, 40, 't90_rail.jpg', 6),
    ('T127 Ağır Hizmet Rayı', 'T127/B kalitede, yüksek yük kapasiteli 5 metre asansör rayı', 7800.00, 25, 't127_rail.jpg', 6),
    ('T75 Hafif Ray', 'T75/B kalitede, ev asansörleri için 3 metre asansör rayı', 2800.00, 35, 't75_rail.jpg', 6);

-- Asansör Aksesuarları
INSERT INTO products (name, description, price, stock, image_url, category_id) VALUES
    ('LED Kabin Aydınlatma Kiti', 'Enerji tasarruflu, dokunmatik kontrollü LED kabin aydınlatma sistemi', 1800.00, 22, 'led_lighting.jpg', 7),
    ('Kabin Tutamacı', 'Paslanmaz çelik, ergonomik tasarımlı kabin tutamacı, 1 metre', 450.00, 50, 'handrail.jpg', 7),
    ('Dijital Kat Göstergesi', '7 inç renkli LCD ekranlı, sesli uyarı sistemli dijital kat göstergesi', 2200.00, 15, 'floor_indicator.jpg', 7);

-- Bakım Ürünleri
INSERT INTO products (name, description, price, stock, image_url, category_id) VALUES
    ('Ray Yağlama Sistemi', 'Otomatik ray yağlama sistemi, 5 litre özel ray yağı dahil', 3800.00, 10, 'rail_lubrication.jpg', 8),
    ('Asansör Bakım Kiti', 'Profesyonel teknisyenler için komple asansör bakım kiti', 5200.00, 8, 'maintenance_kit.jpg', 8),
    ('Kabin Temizlik Seti', 'Kabin iç yüzeyleri için özel kimyasallar ve temizleme malzemeleri seti', 680.00, 25, 'cleaning_kit.jpg', 8);

-- Create Admin User (hashed password: password123)
INSERT INTO users (email, password, first_name, last_name, role) VALUES 
    ('admin@example.com', '$2a$10$GmMLLYDkx3gNLgf1.ujkqOwuK5rdwRx7OKbL1yjp0F3hry2VfQANi', 'Admin', 'User', 'admin');

-- Create Regular User (hashed password: password123)
INSERT INTO users (email, password, first_name, last_name, role) VALUES 
    ('user@example.com', '$2a$10$GmMLLYDkx3gNLgf1.ujkqOwuK5rdwRx7OKbL1yjp0F3hry2VfQANi', 'Regular', 'User', 'user'); 