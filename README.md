# Net Movie Management System

This is a film management system application. This project provides an API to manage films, actors, directors, and
awards. Additionally, the project includes tests written using XUnit and Moq.

## Installing

1. Clone the repository

```
git clone https://github.com/BerkayMehmetSert/net.MovieManagementSystem.git
```

2. Install dependencies

```
dotnet restore
```

3. Create database

```
dotnet ef database update
```

4. Run the project

```
dotnet run
```

5. Run the tests

```
dotnet test
```

## Prerequisites ⚙️

* .NET 6.0
* Entity Framework Core 6.0
* Microsoft SQL Server
* Swagger
* AutoMapper
* InMemory Cache

**Admin Credentials🔒**

```text
Email: admin@system.com
Password: 1234
```

## Built With 🛠️

* [Rider](https://www.jetbrains.com/rider/) - IDE
* [ASP.NET Core 6.0](https://docs.microsoft.com/tr-tr/aspnet/core/?view=aspnetcore-6.0) - Web Framework
* [Entity Framework Core 6.0](https://docs.microsoft.com/tr-tr/ef/core/) - ORM
* [Microsoft SQL Server](https://www.microsoft.com/tr-tr/sql-server/sql-server-downloads) - Database
* [Swagger](https://swagger.io/) - API Documentation
* [AutoMapper](https://automapper.org/) - Object-Object Mapper
* [InMemory Cache](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-6.0) -
  Caching
* [XUnit](https://xunit.net/) - Unit Testing