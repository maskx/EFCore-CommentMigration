# EFCore-CommentMigration

## About

Extract the XML comments from code, migration to database table or column description

## Usage

### Install

A compiled library is available via NuGet

To install via the nuget package console

```
Install-Package maskx.EFCore.SqlServer.CommentMigration
```

To install via the nuget user interface in Visual Studio the package to search for is "maskx.EFCore.SqlServer.CommentMigration"

### Configure

Modify your DbContext like this:

``` CSharp
public class CommentDbContext : DbContext
{
    public CommentDbContext() : base()
    {

    }
    public CommentDbContext(DbContextOptions options) : base(options)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .ReplaceService<IMigrationsSqlGenerator, SqlServerCommentMigrationsSqlGenerator>()
            .ReplaceService<IMigrationsAnnotationProvider, CommentAnnotationProvider>();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddCommentAnnotation();
        base.OnModelCreating(modelBuilder);
    }
}
```

or use CommentDbContext directly

## License

The MIT License (MIT) - See file 'LICENSE' in this project

## Reference

* https://stackoverflow.com/a/51098869
* https://github.com/aspnet/EntityFramework.Docs/issues/1210#issuecomment-450607578

