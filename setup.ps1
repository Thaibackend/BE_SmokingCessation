# Install Entity Framework CLI tools (if not already installed)
dotnet tool install --global dotnet-ef

# Restore packages
dotnet restore

# Create migration
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update

# Build project
dotnet build

# Run project
dotnet run 