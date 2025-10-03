#!/bin/bash

echo "🚀 Starting Thaprobid Auction API with MySQL"
echo "============================================"

# Check if Docker is running
if ! docker info &> /dev/null; then
    echo "❌ Docker is not running. Please start Docker Desktop and try again."
    exit 1
fi

# Start MySQL container
echo "📦 Starting MySQL container..."
docker-compose up mysql -d

# Wait for MySQL to be ready
echo "⏳ Waiting for MySQL to be ready..."
until docker-compose exec mysql mysqladmin ping -h localhost --silent; do
    echo "   MySQL is starting up..."
    sleep 3
done

echo "✅ MySQL is ready!"

# Create and run migrations
echo "🔄 Creating database migrations..."
cd api
dotnet ef migrations add InitialMySQLMigration
dotnet ef database update

echo "📊 Database setup complete!"

# Go back to root directory
cd ..

# Build and start the API
echo "🏗️  Building and starting the API..."
docker-compose up api --build

echo "🌟 Application is running!"
echo "   API: http://localhost:5031"
echo "   Swagger: http://localhost:5031/swagger"
echo "   MySQL: localhost:3306"
echo ""
echo "Press Ctrl+C to stop all services"