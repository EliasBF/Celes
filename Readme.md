# Celes

Wrapper for Dapper connection to SqlServer, MySQL & PostgreSQL

## Example Usage

### Import package

```csharp
using Celes;
```

### Create database connection

```csharp
DatabaseProvider provider = new DatabaseProvider(new ConnectionParameters
{
  Server = "",
  Database = "",
  User = "",
  Password = "",
  Dialect = DatabaseDialect.SqlServer // SqlServer | Mysql | Postgres
});
```

### Execute commands

```csharp
provider.ExecCommand("DELETE FROM Test");

provider.ExecCommands("DELETE FROM Test WHERE Id = @Id", new List<dynamic>
{
  new { Id = 1 },
  new { Id = 2 }
});
```

### Execute queries

```csharp
MyClass myObject = provider.QueryOne<MyClass>("SELECT Id, Name FROM Test WHERE Id = @Id", new
{
  Id = 1
});

List<MyClass> myObjects = provider.QueryAll<MyClass>("SELECT Id, Name FROM Test");
```
