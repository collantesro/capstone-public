# Application Dependencies

This application was written against .NET Core 2.1.  At the end of the semester, .NET Core 2.2 was released, but much too late to use it properly.  We attempted to update the project to .NET Core 2.2.  Everything appeared to work without a hitch, but Osiris not having been updated was an issue, and so we reverted to Core 2.1.

## Server-side Dependencies

The following dependencies are used by the C# code.  They are defined in CCSInventory/CCSInventory.csproj:

* BCrypt.Net-Next.StrongName: This package is used to implement the password hashing.  It uses the BCrypt algorithm.
* DocumentFormat.OpenXml: This package provides an SDK to generate Word, PowerPoint, and (most importantly) Excel support.  The Inventory Report generates Excel files.
* Microsoft.AspNetCore.App: This meta-package is what provides ASP.NET Core 2.1.
* Microsoft.AspNetCore.Razor.Design: I'm unsure why this is there.  Perhaps it was added automatically by Visual Studio.  Shouldn't it be implied by the previous package?
* Microsoft.EntityFrameworkCore.Design: This package is used to provide the `dotnet ef` command with templates, I think.  Since we rely on Entity Framework Core as the ORM for the app, this was added along with the next package.
* Microsoft.EntityFrameworkCore.Sqlite: This package was used to provide SQLite support in Entity Framework Core.  SQLite was only used for developmental purposes, and it's not intended for the final app to use SQLite.
* Microsoft.VisualStudio.Web.CodeGeneration.Design: This package was required for the scaffolding functionality for Razor Pages of Visual Studio and the `dotnet aspnet-codegenerator` commands.  If no further scaffolding is required, this package may be removed.
* ServiceStack.Text.Core: This package is used for its CSV serialization.
* SixLabors.ImageSharp: This package is used to convert a bitmap in memory into a PNG image in the BarcodeMiddleware.
* ZXing.Net: This package is used to generate a Code 39 barcode of the container's bin number in BarcodeMiddleware.

## Client-side Dependencies

The following dependencies are used by the front-end code.  These JavaScript dependencies are defined in CCSInventory/libman.json:

* twitter-bootstrap: Bootstrap provides the styling framework (.css) used throughout the application.
* jquery: twitter-bootstrap above requires jQuery for some of its functionality.  Since it's included, some of the hand-written scripts make use of jQuery.
* font-awesome: This library provides icons used in buttons or links throughout the application.  Its HTML/CSS classes start with `fa`.
* lodash.js: This library provided some convenience methods and utilities to hand-written JavaScript.
* jquery-validate & jquery-validation-unobtrusive: These libraries are used for client-side validation of forms.  ASP.NET Core makes heavy use of HTML attributes that are read automatically by these libraries.
