# Capstone Project for Weber State University CS 4790 Fall 2018

## Group Members

Rolando Collantes  
Spencer Harston  
Mark Haslam  
Amy Lea  
Josh Redford  
Kion Shamsa  
Carson Smith  
Bryan Sutton  
Hannah VanderHoeven  
Maxwell Wright

---

## Start Here

The project makes use of LibMan for third-party client-side dependencies.  These are: Bootstrap, jQuery, Font Awesome, Lodash.

Read the Restore Instructions below to download these client-side dependencies.

### ERD Video

By Amy Lea: [https://www.youtube.com/watch?v=Q6IB3YgWnpQ](https://www.youtube.com/watch?v=Q6IB3YgWnpQ)

### How-to Video

By Kion Shamsa: [https://www.youtube.com/watch?v=qPix-HWC-O8](https://www.youtube.com/watch?v=qPix-HWC-O8)

## Project Description

This ASP.NET Core application is an inventory manager for Catholic Community Services of Utah.  Its purpose is to manage food donations.  Bins can be created to organize the warehouse.

## Restore Instructions

These instructions are executed in the working directory of CCSInventory/ (LibMan & Migrations)

### Using LibMan for client-side libraries

If you're using the `dotnet` command from the terminal, the following instructions to restore the client-side libraries apply:

When using LibMan for the first time, ensure it is installed:  
`dotnet tool install -g Microsoft.Web.LibraryManager.Cli`

Then restore the client-side libraries as defined in `libman.json`.  Ensure your working directory is `CCSInventory/`:  
`libman restore`

If you're using Visual Studio, the option to "Restore Client-Side Libraries" is available in the context menu when right-clicking the `libman.json` file in the Solution Explorer window.

The .gitignore file is set to ignore `*/wwwroot/lib` files, since we don't want to track third-party code.  If your layout looks ugly, you haven't restored these dependencies.

### Database

The application uses SQLite as the development database.  It's portable, requires no installation, and everyone has the same view of the database.  There is a separate branch `sqlserver` with migrations and configuration for Titan, the SQL Server server for the Computer Science department.

#### SQLite Migrations

The .sqlite files should check in and out with the rest of the project. It shouldn't be necessary for every individual to "update" or create the database.  One person can handle the migrations for everyone (usually Rolando).  But just in case, you can do so using the `dotnet ef` commands:  

1. Update the models in the [Models/](CCSInventory/Models/) folder, seed data in [CCSDbContext.OnModelCreating()](CCSInventory/Models/CCSDbContext.cs)
2. Drop the previous database.  Either delete CCSProduction.sqlite from your filesystem manually, or run this command: `dotnet ef database drop` and answer yes.
3. SQLite has limited support for the ALTER TABLE commands, so migrations don't always work properly. Since there's no need to keep old data while we're still in development, I drop the old migrations with this command: `dotnet ef migrations remove`.  Alternatively, you can manually delete the Migrations/ folder.
4. Generate new migrations: `dotnet ef migrations add DevV12` where "DevV12" is any name.  I've just incremented it by 1 each time big changes have been made, just for reference.
5. Create the CCSProduction.sqlite database file with the migrations applied: `dotnet ef database update`
