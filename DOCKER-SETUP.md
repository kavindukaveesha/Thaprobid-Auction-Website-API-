# Thaprobid Auction API - Docker Setup

This guide explains how to run the Thaprobid Auction API with MySQL using Docker.

## Prerequisites

- Docker Desktop installed and running
- .NET 8.0 SDK (for development)

## Quick Start

### Option 1: Docker Compose (Recommended)

1. **Start Docker Desktop** on your machine

2. **Run the startup script:**
   ```bash
   ./start-project.sh
   ```

   This script will:
   - Start MySQL container with sample data
   - Wait for MySQL to be ready
   - Create database migrations
   - Build and start the API container

3. **Access the application:**
   - API: http://localhost:5031
   - Swagger Documentation: http://localhost:5031/swagger
   - MySQL: localhost:3306

### Option 2: Local MySQL

If you prefer to use a local MySQL installation:

```bash
./run-local-mysql.sh
```

## Manual Setup

### 1. Start MySQL Only

```bash
docker-compose up mysql -d
```

### 2. Create Migrations

```bash
cd api
dotnet ef migrations add InitialMySQLMigration
dotnet ef database update
```

### 3. Start API in Development

```bash
cd api
dotnet run
```

### 4. Full Docker Setup

```bash
docker-compose up --build
```

## Configuration

### Database Connection

The application uses different connection strings based on environment:

- **Local Development**: `DefaultConnection` (localhost:3306)
- **Docker Environment**: `DockerConnection` (mysql:3306)

### Environment Variables

- `DOCKER_ENV=true`: Uses Docker connection string
- `ASPNETCORE_ENVIRONMENT=Development`: Enables development features

## Sample Data

The MySQL container automatically loads sample data including:

- **Users**: Admin, Clients, and Sellers with profiles
- **Categories**: Art & Collectibles, Antiques, Jewelry, Books, Coins
- **Auctions**: 3 sample auctions with various statuses
- **Items**: 8 auction items with different categories
- **Bids**: Sample bidding history

### Sample Login Credentials

| Role | Email | Password |
|------|--------|----------|
| Admin | admin@thaprobid.com | Password123! |
| Client | john.doe@email.com | Password123! |
| Client | jane.smith@email.com | Password123! |
| Seller | seller@antiques.com | Password123! |
| Seller | seller@artgallery.com | Password123! |

## API Endpoints

### Authentication
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/forgot-password` - Password reset

### Auctions
- `GET /api/auction/all` - Get all auctions
- `GET /api/auction/live` - Get live auctions
- `GET /api/auction/{id}` - Get auction details

### Items & Bidding
- `GET /api/auction/{auctionId}/items` - Get auction items
- `POST /api/auction/{auctionId}/items/{itemId}/bids` - Place bid

### Admin
- `GET /api/admin/manage-fields` - Manage categories
- `GET /api/admin/usemannagemet/users` - User management

## Development

### Project Structure

```
api/
├── Controllers/        # API controllers
├── Models/            # Entity models  
├── Data/              # Database context
├── Dto/               # Data transfer objects
├── Services/          # Business logic
├── Repositories/      # Data access layer
└── Interfaces/        # Service contracts

docker/
└── mysql/
    └── init/          # Database initialization scripts
```

### Adding New Features

1. Update entity models in `api/Models/`
2. Create/update DTOs in `api/Dto/`
3. Add repository interfaces and implementations
4. Create service layer business logic
5. Add controller endpoints
6. Create new migration: `dotnet ef migrations add <MigrationName>`
7. Update database: `dotnet ef database update`

## Troubleshooting

### Docker Issues

1. **Docker not running:**
   ```
   Cannot connect to the Docker daemon
   ```
   **Solution**: Start Docker Desktop

2. **Port conflicts:**
   ```
   Port 3306 is already in use
   ```
   **Solution**: Stop existing MySQL services or change ports in docker-compose.yml

3. **MySQL connection failed:**
   ```
   Access denied for user
   ```
   **Solution**: Wait for MySQL to fully initialize (30-60 seconds)

### Migration Issues

1. **Migration already exists:**
   ```bash
   dotnet ef migrations remove
   dotnet ef migrations add InitialMySQLMigration
   ```

2. **Database connection timeout:**
   - Ensure MySQL container is healthy
   - Check connection string in appsettings.json

### API Issues

1. **Build errors after MySQL changes:**
   ```bash
   dotnet clean
   dotnet restore
   dotnet build
   ```

2. **Swagger not loading:**
   - Check if API is running on correct port
   - Verify ASPNETCORE_URLS configuration

## Stopping Services

```bash
# Stop all services
docker-compose down

# Stop and remove volumes (clears database)
docker-compose down -v

# Stop specific service
docker-compose stop mysql
```

## Database Management

### Connect to MySQL Container

```bash
docker-compose exec mysql mysql -u thaprobid_user -pThaprobidUser123! thaprobidauction
```

### Backup Database

```bash
docker-compose exec mysql mysqldump -u thaprobid_user -pThaprobidUser123! thaprobidauction > backup.sql
```

### Restore Database

```bash
docker-compose exec -T mysql mysql -u thaprobid_user -pThaprobidUser123! thaprobidauction < backup.sql
```

## Production Deployment

For production deployment:

1. Update connection strings with production database
2. Set `ASPNETCORE_ENVIRONMENT=Production`
3. Configure proper SSL certificates
4. Use secrets management for sensitive data
5. Set up proper logging and monitoring

## Support

For issues or questions:
1. Check the troubleshooting section above
2. Review Docker and MySQL logs: `docker-compose logs`
3. Ensure all prerequisites are installed correctly