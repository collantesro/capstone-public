# Project Structure

## How-to Video

By Kion Shamsa: [https://www.youtube.com/watch?v=qPix-HWC-O8](https://www.youtube.com/watch?v=qPix-HWC-O8)

## Source Code Structure

This application is structured similarly to other ASP.NET Core applications.

Both MVC and Razor Pages were used in this application.  The MVC Controllers (the "C") are in the `CCSInventory/Controllers/` directory.  Razor Pages are kept in the `CCSInventory/Pages/` directory.

Controllers are decorated with `[Route()]` annotations to describe to the framework how to route the user to the proper controller and action.

The only piece of custom Middleware in the application is BarcodeMiddleware (in CCSInventory/Middleware/), which generates Code 39 barcodes for URLs containing `/barcode?code=1234`, where `1234` is the content of the barcode.

As with any ASP.NET application, Program.cs contains the `Main()` method that starts the whole thing.  This file was unmodified from the empty template that started the project.

Startup.cs contains the configuration and setup for the application.  It configures services like the database, describes the middleware pipeline, and defines the authorization policies for users.

appsettings.json\* files are used to store values, such as the database connection string.  These values can be changed without recompiling the application.  Some constants (range of Bin Numbers and recents for ViewComponents) are also defined in this file.

Validation of the user's login at each request is handled by `CCSInventory/Utilities/LoginValidator.cs`.  Modifying a user causes that user to be logged out.

Migrations are used for versioning of the database model.  They are used by Entity Framework Core in order to add seed data or alter tables to match the models.  Migrations are created by the `dotnet ef migrations` commands.

The file most important in describing the database model is `CCSInventory/Models/CCSDbContext.cs`.  It extends from Entity Framework Core's DbContext.  All the seed data is defined within it, along with all the tables (`DbSet<>` properties).  Upon changing this file, migrations and the database need to be updated.
