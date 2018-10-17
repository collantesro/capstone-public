# Capstone Project for CS4790

## Group Members

Amy Lea  
Rolando Collantes  
Kion Shamsa  
Spencer Harston  
Mark Haslam

---

## Restore Instructions

If you're using the `dotnet` command from the terminal, restore the client-side libraries with LibMan:  
When using LibMan for the first time, ensure it is installed:  
`dotnet tool install -g Microsoft.Web.LibraryManager.Cli`

Then restore the client-side libraries as defined in `libman.json`:  
`libman restore`

Visual Studio should handle this for you, if you're using an IDE.

The .gitignore file is set to ignore `*/wwwroot/lib` files, as we do not want to track third-party code.
