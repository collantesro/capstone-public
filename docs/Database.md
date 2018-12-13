# Database Details

## ERD Video

By Amy Lea: [https://www.youtube.com/watch?v=Q6IB3YgWnpQ](https://www.youtube.com/watch?v=Q6IB3YgWnpQ)

## Development

Our application uses Entity Framework Core to abstract away the SQL database.  We use the version of Entity Framework Core included with .NET Core 2.1.

For ease of development, the database we used was SQLite.  The reasons are as follows:

* SQLite doesn't require installation of any additional software.
* SQLite is portable across all major operating systems.
* The files produced by SQLite can be tracked by git, which allows collaborators to share their database.
* The project can be executed immediately after the repository has been cloned and restored, with no need to first run `dotnet ef` commands.

The major drawback of using SQLite is that Visual Studio's SQL Server Object Explorer cannot inspect SQLite files.  An external tool (for example, the `sqlite3` command-line tool) must be used instead.

Refer to [README.md](../README.md) for instructions about updating the SQLite database.

SQLite is not intended to be the final database software.  The production database software is supposed to be Microsoft SQL Server.

## Production

As the application approaches completion, we are starting to host the application on Microsoft SQL Server.  Thanks to Entity Framework Core, switching is simply a matter of changing the connection string and the extension method (`UseSqlServer(...)`) in `Startup.ConfigureServices()` in Startup.cs.  The hosted app on Osiris makes use of Titan (SQL Server) for its database, instead of SQLite.  The git branch `sqlserver` has the migrations and configuration to use SQL Server.

Refer to the file `appsettings.json` for connection strings.
