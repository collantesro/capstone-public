# Login

The login logic for the application is primary located in `CCSInventory/Pages/Account/Login.cshtml.cs`.  Password hashing is done by BCrypt (see [Dependencies.md](./Dependencies.md)).  Proper authorization is enforced by the framework and the `[Authorize]` annotations at each Controller, Controller Action, or Razor Page PageModel.

For development purposes, there is a user with UserRole of ADMIN named `skram`.  The password to this account is `M8/iq+W1` (it was randomly generated, there is no significance to its meaning).

Login attempts are artificially delayed to 500 milliseconds to prevent instant feedback when a username doesn't exist.  The idea is to attempt to hide whether the username or the password is incorrect.  A valid login has no delay.

Logins are, by default, only for the current session.  This means that the login is cleared when the user restarts their browser.  A Remember Me checkbox is provided for a more persistent login.

Each request made to an authorized page is validated against the user's ModifiedDate value in the database.  If a user's ModifiedDate is changed in the database, their logins are invalidated and they must log in again.
