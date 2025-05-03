-- Delete seed data in reverse order
DELETE FROM users WHERE email IN ('admin@example.com', 'user@example.com');
DELETE FROM products;
DELETE FROM categories; 