# Capstone Project for CS4790

## Group Members

Amy Lea  
Rolando Collantes  
Kion Shamsa  
Spencer Harston  
Mark Haslam

---

## Restore Instructions

These instructions are run in the working directory of CCSInventory/

### Using LibMan for client-side libraries

If you're using the `dotnet` command from the terminal, restore the client-side libraries:

When using LibMan for the first time, ensure it is installed:  
`dotnet tool install -g Microsoft.Web.LibraryManager.Cli`

Then restore the client-side libraries as defined in `libman.json`:  
`libman restore`

Visual Studio should handle this for you, if you're using an IDE.

The .gitignore file is set to ignore `*/wwwroot/lib` files, since we don't want to track third-party code.

### Applying Database Migrations

The .sqlite files should check in and out with the rest of the project. It shouldn't be necessary to "update" or create the database.  But just in case, you can do so using the `dotnet ef` command:  
`dotnet ef database update --context CCSDbContext`

If you want to start over with the database, drop it first.  Either delete the .sqlite file from the filesystem manually, or run this command:  
`dotnet ef database drop --context CCSDbContext`  
Then run the above database update command.
