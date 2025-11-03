# CREATE A WEBAPI PROJECT

## Create webapi with controllers
```bash
dotnet new webapi --use-controllers -n MyWebApi
```

## Install EF dependencies
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
```

## Create Models for EF
  - create directory for models
  - create entities directory inside models
    - create Employee entity

## Create Database Context
  - create Data directory
    - create ApplicationDbContext.cs
      - create db context
    - add collections for entities

## Add Connection string to appsetttings.json
```yaml
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=EmployeesDB;User Id=sa;Password=ODM2MzAnSS;TrustServerCertificate=True;TrustServerCertificate=True;"
}
```

## Inject DbContext into Program.cs
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

## Create Migration
```bash
# Install EF Design Tools
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0

# Windows through Visual Studio -> Nuget Package Console
add-migration "Initial migration"

# Linux through terminal
dotnet ef migrations add InitialMigration -p admin-portal.csproj
```

## update database
```bash
# create empty database <EmployeesDB>

# Windows through Visual Studio -> Nuget Package Console
update-database

# Linux through terminal
dotnet ef database update -p admin-portal.csproj
```

## Create Controllers
- Create EmployeesController.cs

## Create Dtos for controlelrs
- Create AddEmployeeDto.cs in Models
