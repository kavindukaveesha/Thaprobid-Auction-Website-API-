# Thaprobid Auction Website(API)
 
1. Docker Configuration
    - docker-compose.yml with MySQL 8.0 and API services
    - Dockerfile for containerizing the .NET API
    - Health checks and proper service dependencies
  2. MySQL Integration
    - Added Pomelo.EntityFrameworkCore.MySql package
    - Updated connection strings for local and Docker environments
    - Modified Program.cs to use MySQL with environment-based configuration
  3. Sample Data Scripts
    - Comprehensive sample data including:
        - 5 users (admin, clients, sellers) with profiles
      - Hierarchical categories (Fields → Categories → SubCategories)
      - 3 sample auctions with different statuses
      - 8 auction items across various categories
      - 22 sample bids showing bidding history
  4. Setup Scripts
    - start-project.sh - Full Docker setup with MySQL
    - run-local-mysql.sh - Local MySQL development option
    - Both scripts handle migrations and sample data loading
  5. Documentation
    - DOCKER-SETUP.md - Comprehensive setup and troubleshooting guide

  🚀 To Run the Project:

  Option 1: Docker (Recommended)
  ./start-project.sh

  Option 2: Local MySQL
  ./run-local-mysql.sh

  📊 Sample Data Includes:

  - Heritage Collection Auction (live)
  - Contemporary Art Showcase (closed)
  - Vintage Jewelry & Timepieces (live)
  - Items: Kandyan chest, ceramic pots, paintings, sculptures, jewelry
  - Active bidding on live auctions

  🌐 Access Points:

  - API: http://localhost:5031
  - Swagger: http://localhost:5031/swagger
  - MySQL: localhost:3306

  🔐 Sample Login:

  - Admin: mailto:admin@thaprobid.com / Password123!
  - Client: mailto:john.doe@email.com / Password123!
  - Seller: mailto:seller@antiques.com / Password123!


  file explain all pages,colors,i use react+tailwindcss dakrmode lightmode