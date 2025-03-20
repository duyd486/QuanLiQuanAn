INSERT INTO dishlist (id, name, thumbnail) VALUES
(1, 'Dessert', NULL),
(2, 'Appetizer', NULL),
(3, 'Soup', NULL),
(4, 'Noodles', NULL),
(5, 'Rice', NULL);

INSERT INTO dishes (id, dishlist_id, name, price, description) VALUES
(1, 1, 'Chocolate Cake', 50000, 'Delicious chocolate cake with rich cocoa flavor'),
(2, 1, 'Fruit Salad', 35000, 'Fresh seasonal fruits with honey dressing'),
(3, 1, 'Ice Cream', 30000, 'Creamy vanilla ice cream with chocolate syrup'),
(4, 2, 'Spring Rolls', 40000, 'Crispy spring rolls with sweet chili sauce'),
(5, 2, 'Bruschetta', 45000, 'Grilled bread with tomato, garlic, and basil'),
(6, 2, 'Garlic Bread', 30000, 'Toasted bread with garlic butter'),
(7, 3, 'Chicken Soup', 50000, 'Warm chicken soup with vegetables'),
(8, 3, 'Miso Soup', 45000, 'Japanese miso soup with tofu and seaweed'),
(9, 3, 'Tomato Soup', 40000, 'Creamy tomato soup with basil'),
(10, 4, 'Ramen', 70000, 'Japanese noodle soup with pork and egg'),
(11, 4, 'Spaghetti Carbonara', 75000, 'Classic Italian pasta with bacon and cheese'),
(12, 4, 'Pad Thai', 60000, 'Thai stir-fried noodles with shrimp'),
(13, 5, 'Fried Rice', 50000, 'Stir-fried rice with vegetables and egg'),
(14, 5, 'Sushi Rice Bowl', 80000, 'Rice bowl with fresh salmon and avocado'),
(15, 5, 'Curry Rice', 65000, 'Japanese curry with beef and steamed rice');

