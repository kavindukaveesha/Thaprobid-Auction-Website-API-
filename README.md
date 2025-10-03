# Thaprobid Auction Website API 🏛️

A comprehensive RESTful API for an online auction platform built with .NET 8 and Entity Framework Core. This project provides robust auction management, user authentication, bidding system, and administrative capabilities.

## 🌟 Features

### Core Functionality
- **User Management**: Registration, authentication, and profile management
- **Auction System**: Create, manage, and participate in auctions
- **Bidding Engine**: Real-time bidding with bid history tracking
- **Category Management**: Hierarchical categorization (Fields → Categories → SubCategories)
- **Admin Panel**: Comprehensive administrative controls
- **Email Services**: Automated notifications and password reset
- **Mobile Verification**: OTP-based mobile number verification

### Technical Features
- JWT Authentication & Authorization
- Entity Framework Core with MySQL
- RESTful API design
- Swagger/OpenAPI documentation
- Docker containerization
- Clean Architecture principles
- Global exception handling
- Data validation and mapping

## 🏗️ Project Structure

```
api/
├── Controller/           # API Controllers
│   ├── Admin/           # Admin management endpoints
│   ├── Client/          # Client-facing endpoints
│   └── auth/            # Authentication endpoints
├── Data/                # Database context
├── Dto/                 # Data transfer objects
├── Handlers/            # Exception handlers
├── Helpers/             # Query objects and utilities
├── Interfaces/          # Service interfaces
├── Mappers/             # Entity-DTO mapping
├── Models/              # Domain models
├── Repository/          # Data access layer
├── Service/             # Business logic services
└── Response/            # API response models
```

## 🚀 Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker & Docker Compose](https://docs.docker.com/get-docker/)
- [MySQL 8.0](https://dev.mysql.com/downloads/) (for local development)
- [Git](https://git-scm.com/)

### 📥 Clone Repository

```bash
git clone https://github.com/your-username/Thaprobid-Auction-Website-API-.git
cd Thaprobid-Auction-Website-API-
```

## 🛠️ Development Setup

### Option 1: Docker Setup (Recommended)

1. **Start with Docker Compose**
   ```bash
   chmod +x start-project.sh
   ./start-project.sh
   ```

2. **Manual Docker Setup**
   ```bash
   # Start MySQL container
   docker-compose up mysql -d
   
   # Wait for MySQL to be ready
   docker-compose logs -f mysql
   
   # Run migrations
   cd api
   dotnet ef migrations add InitialMySQLMigration --force
   dotnet ef database update
   
   # Start API container
   cd ..
   docker-compose up api --build
   ```

### Option 2: Local Development

1. **Setup MySQL Database**
   ```bash
   # Install MySQL 8.0 and create database
   mysql -u root -p
   CREATE DATABASE thaprobidauction;
   CREATE USER 'thaprobid_user'@'localhost' IDENTIFIED BY 'ThaprobidUser123!';
   GRANT ALL PRIVILEGES ON thaprobidauction.* TO 'thaprobid_user'@'localhost';
   FLUSH PRIVILEGES;
   ```

2. **Configure Connection String**
   ```bash
   cd api
   # Update appsettings.Development.json with your MySQL connection
   ```

3. **Run Migrations**
   ```bash
   dotnet ef migrations add InitialMigration
   dotnet ef database update
   ```

4. **Start the API**
   ```bash
   dotnet run
   ```

### Option 3: Local MySQL with Docker

```bash
chmod +x run-local-mysql.sh
./run-local-mysql.sh
```

## 🌐 Access Points

| Service | URL | Description |
|---------|-----|-------------|
| API | http://localhost:5031 | Main API endpoint |
| Swagger UI | http://localhost:5031/swagger | Interactive API documentation |
| MySQL | localhost:3306 | Database server |

## 📊 Sample Data

The project includes comprehensive sample data:

### Users
- **Admin**: `admin@thaprobid.com` / `Password123!`
- **Client**: `john.doe@email.com` / `Password123!`
- **Seller**: `seller@antiques.com` / `Password123!`

### Auctions
- Heritage Collection Auction (Live)
- Contemporary Art Showcase (Closed)
- Vintage Jewelry & Timepieces (Live)

### Categories
- **Antiques & Collectibles**
  - Furniture (Chairs, Tables, Cabinets)
  - Ceramics & Pottery (Vases, Bowls, Decorative)
- **Art & Crafts**
  - Paintings (Portraits, Landscapes, Abstract)
  - Sculptures (Bronze, Stone, Wood)
- **Jewelry & Accessories**
  - Rings, Necklaces, Watches

## 🔧 Configuration

### Environment Variables

```bash
# Database
DOCKER_ENV=true/false
ASPNETCORE_ENVIRONMENT=Development/Production

# Email Settings (configure in appsettings.json)
EmailSettings__Host=smtp.gmail.com
EmailSettings__Port=587
EmailSettings__Username=your-email@gmail.com
EmailSettings__Password=your-app-password

# JWT Settings
JWT__Key=your-secret-key
JWT__Issuer=ThaprobidAuction
JWT__Audience=ThaprobidUsers
```

### Database Connection Strings

**Local Development:**
```json
"DefaultConnection": "Server=localhost;Port=3306;Database=thaprobidauction;Uid=thaprobid_user;Pwd=ThaprobidUser123!;"
```

**Docker Environment:**
```json
"DockerConnection": "Server=mysql;Port=3306;Database=thaprobidauction;Uid=thaprobid_user;Pwd=ThaprobidUser123!;"
```

## 📚 API Documentation

### Authentication Endpoints
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/forgot-password` - Password reset request
- `POST /api/auth/reset-password` - Password reset

### Auction Management
- `GET /api/auctions` - List all auctions
- `POST /api/auctions` - Create new auction
- `GET /api/auctions/{id}` - Get auction details
- `PUT /api/auctions/{id}` - Update auction
- `DELETE /api/auctions/{id}` - Delete auction

### Bidding System
- `POST /api/bids` - Place a bid
- `GET /api/bids/auction/{auctionId}` - Get auction bids
- `GET /api/bids/user/{userId}` - Get user's bids

### Category Management
- `GET /api/fields` - Get all fields
- `GET /api/categories` - Get all categories
- `GET /api/subcategories` - Get all subcategories

### Admin Features
- `GET /api/admin/users` - User management
- `POST /api/admin/auctions` - Auction management
- `GET /api/admin/categories` - Category management

## 🧪 Testing

### Manual Testing
1. Visit Swagger UI at http://localhost:5031/swagger
2. Use the interactive documentation to test endpoints
3. Authenticate using sample credentials

### API Testing Tools
```bash
# Using curl
curl -X GET "http://localhost:5031/api/auctions" \
     -H "accept: text/plain"

# Using Postman
# Import the Swagger JSON from /swagger/v1/swagger.json
```

## 🐛 Troubleshooting

### Common Issues

1. **MySQL Connection Failed**
   ```bash
   # Check MySQL is running
   docker-compose logs mysql
   
   # Verify connection string in appsettings.json
   # Ensure database exists and user has permissions
   ```

2. **Port Already in Use**
   ```bash
   # Find process using port 5031
   lsof -i :5031
   
   # Kill the process
   kill -9 <PID>
   ```

3. **Docker Issues**
   ```bash
   # Clean up containers
   docker-compose down
   docker system prune -a
   
   # Rebuild containers
   docker-compose up --build
   ```

4. **Migration Issues**
   ```bash
   # Reset database
   dotnet ef database drop
   dotnet ef migrations remove
   dotnet ef migrations add InitialMigration
   dotnet ef database update
   ```

## 🤝 Contributing

We welcome contributions from developers of all skill levels! Here's how you can help:

### Getting Started

1. **Fork the Repository**
   ```bash
   # Click "Fork" on GitHub
   git clone https://github.com/YOUR-USERNAME/Thaprobid-Auction-Website-API-.git
   cd Thaprobid-Auction-Website-API-
   ```

2. **Create a Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Set Up Development Environment**
   ```bash
   # Follow the setup instructions above
   dotnet run
   ```

### Development Guidelines

#### Code Style
- Follow C# naming conventions
- Use meaningful variable and method names
- Add XML documentation for public methods
- Keep methods small and focused
- Use dependency injection

#### Architecture Patterns
- Repository pattern for data access
- DTO pattern for API contracts
- Mapper pattern for object transformation
- Service layer for business logic

#### Database Migrations
```bash
# Create new migration
dotnet ef migrations add YourMigrationName

# Apply migrations
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

### Contribution Areas

#### 🆕 Beginner-Friendly Issues
- Add unit tests for existing services
- Improve error messages and validation
- Add API endpoint documentation
- Fix code formatting and styling
- Add configuration validation

#### 🔧 Intermediate Features
- Implement real-time bidding with SignalR
- Add file upload for auction images
- Create advanced search and filtering
- Implement caching with Redis
- Add rate limiting and throttling

#### 🚀 Advanced Features
- Implement microservices architecture
- Add payment gateway integration
- Create automated testing pipeline
- Implement event sourcing
- Add monitoring and logging

### Pull Request Process

1. **Before Creating PR**
   ```bash
   # Ensure code builds without warnings
   dotnet build
   
   # Run existing tests (when available)
   dotnet test
   
   # Check for formatting issues
   dotnet format
   ```

2. **Create Pull Request**
   - Use descriptive title and description
   - Reference related issues
   - Include screenshots for UI changes
   - Add tests for new functionality

3. **PR Review Checklist**
   - [ ] Code follows project conventions
   - [ ] No breaking changes without discussion
   - [ ] Documentation updated if needed
   - [ ] Tests added for new features
   - [ ] Performance impact considered

### Issue Reporting

When reporting bugs or requesting features:

1. **Bug Reports**
   - Include steps to reproduce
   - Provide error messages and logs
   - Specify environment details
   - Include sample code if relevant

2. **Feature Requests**
   - Describe the use case
   - Explain expected behavior
   - Consider implementation complexity
   - Discuss alternatives

## 📁 Project Resources

### Documentation Files
- `ADMIN-PANEL-DOCUMENTATION.md` - Admin features guide
- `DOCKER-SETUP.md` - Detailed Docker setup
- `/swagger` - Interactive API documentation

### Scripts
- `start-project.sh` - Full Docker setup
- `run-local-mysql.sh` - Local MySQL setup

### Sample Data
- `docker/mysql/init/01-sample-data.sql` - Database seed data

## 🛡️ Security

### Authentication & Authorization
- JWT tokens with configurable expiration
- Role-based access control (Admin, Client, Seller)
- Password hashing with salt
- Email verification for registration

### Best Practices Implemented
- Input validation and sanitization
- SQL injection prevention with EF Core
- CORS configuration
- Secure password requirements
- Rate limiting ready infrastructure

## 🔄 Future Roadmap

### Phase 1 - Core Enhancements
- [ ] Real-time bidding with SignalR
- [ ] Image upload and management
- [ ] Advanced search and filtering
- [ ] Email templates and notifications

### Phase 2 - Scalability
- [ ] Redis caching implementation
- [ ] Message queue integration
- [ ] API versioning
- [ ] Performance monitoring

### Phase 3 - Advanced Features
- [ ] Payment gateway integration
- [ ] Mobile app API support
- [ ] Analytics and reporting
- [ ] Multi-language support

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Built with .NET 8 and Entity Framework Core
- MySQL database with Pomelo provider
- JWT authentication implementation
- Docker containerization
- Swagger/OpenAPI documentation

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/your-username/Thaprobid-Auction-Website-API-/issues)
- **Discussions**: [GitHub Discussions](https://github.com/your-username/Thaprobid-Auction-Website-API-/discussions)
- **Email**: support@thaprobid.com

---

**Made with ❤️ for the developer community**

*Happy Coding! 🚀*