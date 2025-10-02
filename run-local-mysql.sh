#!/bin/bash

echo "🚀 Starting Thaprobid Auction API with Local MySQL"
echo "=================================================="

# Check if MySQL is installed locally
if ! command -v mysql &> /dev/null; then
    echo "❌ MySQL is not installed locally."
    echo "   Please install MySQL or use Docker by running: ./start-project.sh"
    exit 1
fi

# Check if MySQL service is running
if ! mysqladmin ping -h localhost --silent; then
    echo "❌ MySQL service is not running."
    echo "   Please start MySQL service and try again."
    echo "   macOS: brew services start mysql"
    echo "   Linux: sudo systemctl start mysql"
    exit 1
fi

echo "✅ MySQL is running locally!"

# Create database if it doesn't exist
echo "🗄️  Setting up database..."
mysql -u root -p -e "CREATE DATABASE IF NOT EXISTS thaprobidauction;"
mysql -u root -p -e "CREATE USER IF NOT EXISTS 'thaprobid_user'@'localhost' IDENTIFIED BY 'ThaprobidUser123!';"
mysql -u root -p -e "GRANT ALL PRIVILEGES ON thaprobidauction.* TO 'thaprobid_user'@'localhost';"
mysql -u root -p -e "FLUSH PRIVILEGES;"

# Create and run migrations
echo "🔄 Creating database migrations..."
cd api
dotnet ef migrations add InitialMySQLMigration --force 2>/dev/null || echo "Migration already exists"
dotnet ef database update

echo "📊 Database setup complete!"

# Load sample data
echo "📊 Loading sample data..."
mysql -u thaprobid_user -pThaprobidUser123! thaprobidauction < ../docker/mysql/init/01-sample-data.sql

# Start the API
echo "🏗️  Starting the API..."
dotnet run

echo "🌟 Application is running!"
echo "   API: http://localhost:5031"
echo "   Swagger: http://localhost:5031/swagger"