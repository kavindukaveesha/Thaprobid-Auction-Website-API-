-- Sample Data for Thaprobid Auction Website
-- This script will be automatically executed when MySQL container starts

USE thaprobidauction;

-- Insert sample AspNetRoles
INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) VALUES
('1', 'Admin', 'ADMIN', UUID()),
('2', 'Client', 'CLIENT', UUID()),
('3', 'Seller', 'SELLER', UUID());

-- Insert sample AspNetUsers
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, IsEmailConfirmed, IsMobileConfirmed) VALUES
('admin-001', 'admin@thaprobid.com', 'ADMIN@THAPROBID.COM', 'admin@thaprobid.com', 'ADMIN@THAPROBID.COM', 1, 'AQAAAAIAAYagAAAAELKV0GC7NzJQgOqZ4QZGzxsF/RkKgJYbJ+8NrD4GcKJvN6YoU9k5C8UxJ7Ws7jZzuA==', UUID(), UUID(), '+94771234567', 1, 0, 1, 0, 1, 1),
('client-001', 'john.doe@email.com', 'JOHN.DOE@EMAIL.COM', 'john.doe@email.com', 'JOHN.DOE@EMAIL.COM', 1, 'AQAAAAIAAYagAAAAELKV0GC7NzJQgOqZ4QZGzxsF/RkKgJYbJ+8NrD4GcKJvN6YoU9k5C8UxJ7Ws7jZzuA==', UUID(), UUID(), '+94771234568', 1, 0, 1, 0, 1, 1),
('client-002', 'jane.smith@email.com', 'JANE.SMITH@EMAIL.COM', 'jane.smith@email.com', 'JANE.SMITH@EMAIL.COM', 1, 'AQAAAAIAAYagAAAAELKV0GC7NzJQgOqZ4QZGzxsF/RkKgJYbJ+8NrD4GcKJvN6YoU9k5C8UxJ7Ws7jZzuA==', UUID(), UUID(), '+94771234569', 1, 0, 1, 0, 1, 1),
('seller-001', 'seller@antiques.com', 'SELLER@ANTIQUES.COM', 'seller@antiques.com', 'SELLER@ANTIQUES.COM', 1, 'AQAAAAIAAYagAAAAELKV0GC7NzJQgOqZ4QZGzxsF/RkKgJYbJ+8NrD4GcKJvN6YoU9k5C8UxJ7Ws7jZzuA==', UUID(), UUID(), '+94771234570', 1, 0, 1, 0, 1, 1),
('seller-002', 'seller@artgallery.com', 'SELLER@ARTGALLERY.COM', 'seller@artgallery.com', 'SELLER@ARTGALLERY.COM', 1, 'AQAAAAIAAYagAAAAELKV0GC7NzJQgOqZ4QZGzxsF/RkKgJYbJ+8NrD4GcKJvN6YoU9k5C8UxJ7Ws7jZzuA==', UUID(), UUID(), '+94771234571', 1, 0, 1, 0, 1, 1);

-- Insert user roles
INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES
('admin-001', '1'),
('client-001', '2'),
('client-002', '2'),
('seller-001', '3'),
('seller-002', '3');

-- Insert sample ClientProfiles
INSERT INTO ClientProfiles (Id, AppUserId, FirstName, LastName, ProfilePicture, ClientAddress, IsClientBidder, IsClientSeller, IsProfileCompleted) VALUES
(1, 'admin-001', 'Admin', 'User', 'https://via.placeholder.com/150x150/007bff/ffffff?text=Admin', '123 Admin Street, Colombo 01', 1, 1, 1),
(2, 'client-001', 'John', 'Doe', 'https://via.placeholder.com/150x150/28a745/ffffff?text=JD', '456 Client Avenue, Kandy', 1, 0, 1),
(3, 'client-002', 'Jane', 'Smith', 'https://via.placeholder.com/150x150/dc3545/ffffff?text=JS', '789 Smith Road, Galle', 1, 0, 1),
(4, 'seller-001', 'Michael', 'Johnson', 'https://via.placeholder.com/150x150/ffc107/ffffff?text=MJ', '321 Seller Street, Colombo 03', 1, 1, 1),
(5, 'seller-002', 'Sarah', 'Williams', 'https://via.placeholder.com/150x150/17a2b8/ffffff?text=SW', '654 Art Lane, Negombo', 1, 1, 1);

-- Insert sample Sellers
INSERT INTO Sellers (Id, AppUserId, CompanyName, CompanyImgUrl, CompanyEmail, CompanyMobile, CompanyAddress, CompanyDescription, IsVerified, IsActive) VALUES
(1, 'seller-001', 'Heritage Antiques Ltd', 'https://via.placeholder.com/200x200/6f42c1/ffffff?text=Heritage', 'info@heritageantiques.lk', '+94112345678', '123 Antique Street, Colombo 07', 'Specialized in authentic Sri Lankan antiques and heritage items with over 25 years of experience.', 1, 1),
(2, 'seller-002', 'Ceylon Art Gallery', 'https://via.placeholder.com/200x200/e83e8c/ffffff?text=Ceylon+Art', 'gallery@ceylonart.lk', '+94112345679', '456 Art Boulevard, Colombo 03', 'Premier art gallery featuring contemporary and traditional Sri Lankan artwork.', 1, 1);

-- Insert sample Fields (Top-level categories)
INSERT INTO Fields (Id, FieldName, FieldImgUrl, FieldDescription, IsActive) VALUES
(1, 'Art & Collectibles', 'https://via.placeholder.com/300x200/007bff/ffffff?text=Art', 'Fine art, sculptures, paintings, and collectible items', 1),
(2, 'Antiques & Vintage', 'https://via.placeholder.com/300x200/28a745/ffffff?text=Antiques', 'Historical artifacts, vintage furniture, and antique collections', 1),
(3, 'Jewelry & Watches', 'https://via.placeholder.com/300x200/dc3545/ffffff?text=Jewelry', 'Fine jewelry, precious stones, luxury watches, and accessories', 1),
(4, 'Books & Manuscripts', 'https://via.placeholder.com/300x200/ffc107/ffffff?text=Books', 'Rare books, historical manuscripts, and literary collections', 1),
(5, 'Coins & Currency', 'https://via.placeholder.com/300x200/17a2b8/ffffff?text=Coins', 'Rare coins, historical currency, and numismatic collections', 1);

-- Insert sample Categories
INSERT INTO Categories (Id, CategoryName, CategoryImgUrl, CategoryDescription, FieldId, IsActive) VALUES
-- Art & Collectibles Categories
(1, 'Paintings', 'https://via.placeholder.com/250x150/007bff/ffffff?text=Paintings', 'Oil paintings, watercolors, and contemporary art', 1, 1),
(2, 'Sculptures', 'https://via.placeholder.com/250x150/007bff/ffffff?text=Sculptures', 'Bronze, marble, and modern sculptures', 1, 1),
(3, 'Ceramics & Pottery', 'https://via.placeholder.com/250x150/007bff/ffffff?text=Ceramics', 'Traditional and contemporary ceramic works', 1, 1),

-- Antiques & Vintage Categories
(4, 'Furniture', 'https://via.placeholder.com/250x150/28a745/ffffff?text=Furniture', 'Antique chairs, tables, cabinets, and vintage furniture', 2, 1),
(5, 'Decorative Items', 'https://via.placeholder.com/250x150/28a745/ffffff?text=Decorative', 'Vases, ornaments, and decorative artifacts', 2, 1),
(6, 'Historical Artifacts', 'https://via.placeholder.com/250x150/28a745/ffffff?text=Historical', 'Archaeological finds and historical objects', 2, 1),

-- Jewelry & Watches Categories
(7, 'Necklaces', 'https://via.placeholder.com/250x150/dc3545/ffffff?text=Necklaces', 'Gold, silver, and gemstone necklaces', 3, 1),
(8, 'Rings', 'https://via.placeholder.com/250x150/dc3545/ffffff?text=Rings', 'Wedding rings, engagement rings, and fashion rings', 3, 1),
(9, 'Watches', 'https://via.placeholder.com/250x150/dc3545/ffffff?text=Watches', 'Luxury watches, vintage timepieces', 3, 1),

-- Books & Manuscripts Categories
(10, 'Historical Books', 'https://via.placeholder.com/250x150/ffc107/ffffff?text=Historical', 'Rare historical texts and documents', 4, 1),
(11, 'Literature', 'https://via.placeholder.com/250x150/ffc107/ffffff?text=Literature', 'First editions and literary classics', 4, 1),

-- Coins & Currency Categories
(12, 'Ancient Coins', 'https://via.placeholder.com/250x150/17a2b8/ffffff?text=Ancient', 'Roman, Greek, and other ancient coins', 5, 1),
(13, 'Modern Coins', 'https://via.placeholder.com/250x150/17a2b8/ffffff?text=Modern', 'Commemorative and collectible modern coins', 5, 1);

-- Insert sample SubCategories
INSERT INTO SubCategories (Id, SubCategoryName, SubCategoryImgUrl, SubCategoryDescription, CategoryId, IsActive) VALUES
-- Paintings SubCategories
(1, 'Oil Paintings', 'https://via.placeholder.com/200x120/007bff/ffffff?text=Oil', 'Traditional oil paintings on canvas', 1, 1),
(2, 'Watercolors', 'https://via.placeholder.com/200x120/007bff/ffffff?text=Watercolor', 'Watercolor paintings and sketches', 1, 1),
(3, 'Contemporary Art', 'https://via.placeholder.com/200x120/007bff/ffffff?text=Contemporary', 'Modern and contemporary artworks', 1, 1),

-- Furniture SubCategories
(4, 'Chairs', 'https://via.placeholder.com/200x120/28a745/ffffff?text=Chairs', 'Antique and vintage chairs', 4, 1),
(5, 'Tables', 'https://via.placeholder.com/200x120/28a745/ffffff?text=Tables', 'Dining tables, coffee tables, and desks', 4, 1),
(6, 'Cabinets', 'https://via.placeholder.com/200x120/28a745/ffffff?text=Cabinets', 'Display cabinets and storage furniture', 4, 1),

-- Jewelry SubCategories
(7, 'Gold Jewelry', 'https://via.placeholder.com/200x120/dc3545/ffffff?text=Gold', 'Pure gold and gold-plated jewelry', 7, 1),
(8, 'Gemstone Jewelry', 'https://via.placeholder.com/200x120/dc3545/ffffff?text=Gemstone', 'Precious and semi-precious gemstone jewelry', 7, 1),

-- Books SubCategories
(9, 'Manuscripts', 'https://via.placeholder.com/200x120/ffc107/ffffff?text=Manuscripts', 'Historical manuscripts and documents', 10, 1),
(10, 'First Editions', 'https://via.placeholder.com/200x120/ffc107/ffffff?text=First+Ed', 'First edition books and rare publications', 11, 1);

-- Insert sample Auctions
INSERT INTO Auctions (Id, SellerId, AuctionName, AuctionTitle, AuctionDescription, AuctionCoverImageUrl, AuctionStartDate, AuctionEndDate, IsActive, IsClosed, IsVerified) VALUES
(1, 1, 'Heritage Collection Spring Auction', 'Exquisite Sri Lankan Heritage Items', 'A curated collection of authentic Sri Lankan antiques and heritage pieces, featuring rare ceramics, traditional furniture, and historical artifacts from the colonial period.', 'https://via.placeholder.com/600x400/6f42c1/ffffff?text=Heritage+Spring', '2024-08-15 10:00:00', '2024-08-20 18:00:00', 1, 0, 1),
(2, 2, 'Contemporary Art Showcase', 'Modern Sri Lankan Art Collection', 'Featuring works by renowned contemporary Sri Lankan artists, including paintings, sculptures, and mixed media artworks that represent the evolution of Sri Lankan art.', 'https://via.placeholder.com/600x400/e83e8c/ffffff?text=Art+Showcase', '2024-08-10 14:00:00', '2024-08-12 20:00:00', 1, 1, 1),
(3, 1, 'Vintage Jewelry & Timepieces', 'Luxury Vintage Collection', 'An exclusive auction featuring vintage jewelry, pocket watches, and luxury timepieces from the early 20th century, including pieces from renowned international brands.', 'https://via.placeholder.com/600x400/6f42c1/ffffff?text=Vintage+Luxury', '2024-08-25 11:00:00', '2024-08-28 17:00:00', 1, 0, 1);

-- Insert sample AuctionLotItems
INSERT INTO AuctionLotItems (Id, AuctionId, LotItemName, LotItemDescription, LotCondition, StartingPrice, EstimatedPriceMin, EstimatedPriceMax, BidInterval, LotItemImageUrl, FieldId, CategoryId, SubCategoryId, IsBiddingActive, WinningBidderId, FinalPrice, ShippingFee, AdditionalFees) VALUES
-- Heritage Collection Items
(1, 1, 'Colonial Era Kandyan Chest', 'Authentic Kandyan wooden chest from the late 19th century, featuring traditional Sri Lankan craftsmanship with intricate carvings and brass fittings.', 'Excellent', 25000.00, 20000.00, 35000.00, 1000.00, 'https://via.placeholder.com/400x300/8B4513/ffffff?text=Kandyan+Chest', 2, 4, 6, 1, NULL, NULL, 2500.00, 500.00),
(2, 1, 'Traditional Ceramic Water Pot', 'Handcrafted ceramic water pot (kalaya) from Kelaniya pottery tradition, dating back to early 1900s with original glazing intact.', 'Good', 8000.00, 6000.00, 12000.00, 500.00, 'https://via.placeholder.com/400x300/D2691E/ffffff?text=Water+Pot', 1, 3, NULL, 1, NULL, NULL, 1000.00, 200.00),
(3, 1, 'Dutch Colonial Map of Ceylon', 'Original hand-drawn map of Ceylon from 1750, showing Dutch territories and trading posts. Professionally preserved and framed.', 'Very Good', 45000.00, 40000.00, 60000.00, 2000.00, 'https://via.placeholder.com/400x300/4169E1/ffffff?text=Dutch+Map', 2, 6, NULL, 1, NULL, NULL, 3000.00, 1000.00),

-- Contemporary Art Items
(4, 2, 'Sunset Over Galle Fort', 'Oil painting by renowned artist Senaka Senanayake depicting the iconic Galle Fort during sunset, painted in 2018.', 'Mint', 150000.00, 120000.00, 200000.00, 5000.00, 'https://via.placeholder.com/400x300/FF6347/ffffff?text=Galle+Sunset', 1, 1, 1, 0, 2, 175000.00, 5000.00, 2000.00),
(5, 2, 'Traditional Dancer Bronze Sculpture', 'Life-sized bronze sculpture of a traditional Kandyan dancer, created by sculptor Tissa Ranasinghe in 2019.', 'Excellent', 85000.00, 70000.00, 110000.00, 3000.00, 'https://via.placeholder.com/400x300/CD853F/ffffff?text=Dancer+Bronze', 1, 2, NULL, 0, 3, 98000.00, 8000.00, 1500.00),

-- Vintage Jewelry Items
(6, 3, 'Art Deco Diamond Necklace', 'Stunning Art Deco diamond necklace from the 1920s, featuring geometric patterns with 2.5 carats of diamonds set in white gold.', 'Excellent', 350000.00, 300000.00, 450000.00, 10000.00, 'https://via.placeholder.com/400x300/FFD700/ffffff?text=Diamond+Necklace', 3, 7, 8, 1, NULL, NULL, 5000.00, 10000.00),
(7, 3, 'Victorian Pocket Watch', 'Swiss-made Victorian pocket watch from 1890 with gold case and original chain. Fully functional with recent servicing certificate.', 'Very Good', 125000.00, 100000.00, 180000.00, 5000.00, 'https://via.placeholder.com/400x300/DAA520/ffffff?text=Pocket+Watch', 3, 9, NULL, 1, NULL, NULL, 2000.00, 3000.00),
(8, 3, 'Ceylon Sapphire Ring', 'Exquisite 5-carat Ceylon blue sapphire ring set in 18k white gold with diamond accents. Comes with gemological certificate.', 'Mint', 280000.00, 250000.00, 350000.00, 8000.00, 'https://via.placeholder.com/400x300/4169E1/ffffff?text=Sapphire+Ring', 3, 8, NULL, 1, NULL, NULL, 3000.00, 5000.00);

-- Insert sample ItemBidded records
INSERT INTO ItemBidded (Id, AuctionLotItemId, BidderId, BidAmount, BidDateTime) VALUES
-- Bids for Colonial Era Kandyan Chest (Item 1)
(1, 1, 2, 25000.00, '2024-08-15 10:30:00'),
(2, 1, 3, 26000.00, '2024-08-15 11:15:00'),
(3, 1, 2, 28000.00, '2024-08-15 14:20:00'),
(4, 1, 3, 30000.00, '2024-08-15 16:45:00'),

-- Bids for Traditional Ceramic Water Pot (Item 2)
(5, 2, 2, 8000.00, '2024-08-15 10:45:00'),
(6, 2, 3, 9000.00, '2024-08-15 12:30:00'),
(7, 2, 2, 10500.00, '2024-08-15 15:10:00'),

-- Bids for Dutch Colonial Map (Item 3)
(8, 3, 3, 45000.00, '2024-08-15 11:00:00'),
(9, 3, 2, 48000.00, '2024-08-15 13:25:00'),
(10, 3, 3, 52000.00, '2024-08-15 17:30:00'),

-- Historical bids for closed auction items
(11, 4, 2, 150000.00, '2024-08-10 14:30:00'),
(12, 4, 3, 160000.00, '2024-08-10 16:45:00'),
(13, 4, 2, 175000.00, '2024-08-11 10:15:00'),

(14, 5, 3, 85000.00, '2024-08-10 15:00:00'),
(15, 5, 2, 92000.00, '2024-08-11 09:30:00'),
(16, 5, 3, 98000.00, '2024-08-11 18:45:00'),

-- Active bids for vintage jewelry auction
(17, 6, 2, 350000.00, '2024-08-25 11:30:00'),
(18, 6, 3, 365000.00, '2024-08-25 14:20:00'),

(19, 7, 3, 125000.00, '2024-08-25 12:00:00'),
(20, 7, 2, 135000.00, '2024-08-25 15:45:00'),

(21, 8, 2, 280000.00, '2024-08-25 11:45:00'),
(22, 8, 3, 295000.00, '2024-08-25 16:30:00');