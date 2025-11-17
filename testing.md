# CREATE TESTING PROJECT

## Create testing project
```bash
dotnet new xunit -o admin-portal.Tests
```

## Add testing project to solution
```bash
dotnet sln add admin-portal.Tests/admin-portal.Tests.csproj
```

## Add reference to admin-portal project
```bash
dotnet add admin-portal.Tests/admin-portal.Tests.csproj reference admin-portal/admin-portal.csproj
```

## Run tests
```bash
dotnet test
```

## Install testing dependencies
```bash
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 8.0.0

# restore (re-install) dependencies
dotnet restore
```
