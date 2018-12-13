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

## Unfinished Business

1. Finish the reports system.  We got incoming and inventory to a good base. The way they were written could be used as templates to write the other reports.  The Inventory report can output to an Excel file, while the incoming transaction report can output a CSV file.  One format needs to be picked, as mixing and matching isn't good.
2. The Inventory report didn't finish full functionality for filtering based on categories/subcategories/locations.  The UI needs to be implemented, and the back-end filter code should be fixed.  At the time of writing, the other filters (Upper/Lower Bounds, Start and End dates) should be working.
3. The Inventory report saves every report as a template, even when the Save Template box isn't checked.
4. (Possible bug): DateTime fields for CreatedDate and ModifiedDate are saved in UTC time.  The user is entering their date ranges in local time (in the browser frontend).  Ensure those entry dates are converted to UTC.
5. General testing and formatting. We think we got most of the bugs ironed out, but I think there should be some more in-depth testing done.
6. Providing real seed data to the database that matches the defaults that the clients expect (users, donors, etc) (seed data goes in Models/CCSDbContext.cs)
7. When clicking on a User's name in the top left, there is an option for User Settings.  At the time of writing, it merely displays their information, but there is no proper edit functionality for that info or password.  Clicking the Edit button directs the user to the admin page to edit that user, but non-administrators cannot access this page.  Any administrator can edit any user through the /admin/allusers page.  Note that LoginValidator (Utilities/LoginValidator.cs) will reset all old logins for that user if the ModifiedDate is different than their login cookie.
8. Getting the database set up on their server.  The development database was SQLite, but switching to SQL Server is simply a matter of changing the line in the ConfigureServices method of Startup.cs (one is commented out), and the migrations need to be deleted if they were made against SQLite.  See the `dotnet ef` commands below.  They apply to SQL Server as well.  The git branch `sqlserver` is the branch Osiris/Titan were set up against.
9. Some changes to the database produce a runtime log to the console.  This wasn't tied into the Log table in the DbContext.  Logging functionality for CRUD functionality needs to be finished. (This may be required for State Audits).  Models do store the username that created them, along with the username that last edited them.
10. A page for runtime exceptions needs to be made.  Currently, it merely throws an error 500 code with no details.  A page telling the user that an error has occurred (along with an entry in some log for someone to fix) should be made.
11. Consider moving Agencies to a different part of the menu.  A warehouse worker receiving donations shouldn't necessarily be an Administrator, but that section of the side-bar will not appear for them.  Requiring an Administrator to add a new donor would impede throughput.

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
