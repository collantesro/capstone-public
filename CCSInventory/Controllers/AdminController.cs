using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using CCSInventory.Models;
using CCSInventory.ViewModels.Admin;
using CCSInventory.Utilities;

namespace CCSInventory.Controllers
{

    [Route("Admin")]
    [Authorize("AdminUser")]
    /// <summary>
    /// AdminController manages functionality only available to UserRole of ADMIN.  This includes managing users, viewing logs, restoring deleted/archived agencies, etc.
    /// </summary>
    public class AdminController : Controller
    {
        private readonly CCSDbContext _context;
        private readonly ILogger<AdminController> _log;

        private readonly static HashSet<string> acceptedSorts = new HashSet<string>{
            "email",
            "fullnamelf",
            "username",
            "usernote",
            "userrole",
        };

        public AdminController(CCSDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _log = logger;
        }

        /// <summary>
        /// This default action loads up the main view for /admin/
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route(""), Route("Index")]
        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// This action displays the form to create a new user.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("NewUser")]
        public ViewResult NewUser()
        {
            return View();
        }

        /// <summary>
        /// This is the action backing the form to verify and create the new user on POST.
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPost, Route("NewUser")]
        public async Task<IActionResult> NewUser(UserAdd newUser)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                newUser.Username = newUser.Username.ToLower();
                User dbUser = await _context.Users.Where(u => u.Username == newUser.Username).FirstOrDefaultAsync();
                if (dbUser != null)
                {
                    ModelState.AddModelError("", "That User Name is already used");
                    return View();
                }
                else
                {
                    User toInsert = new User
                    {
                        Username = newUser.Username,
                        FirstName = newUser.FirstName,
                        LastName = newUser.LastName,
                        Email = newUser.Email,
                        UserRole = newUser.UserRole,
                        UserNote = newUser.UserNote
                    };
                    PasswordChangeResult result = toInsert.ChangePassword(newUser.Password);
                    if (result != PasswordChangeResult.PASSWORD_OK)
                    {
                        ModelState.AddModelError("", "Error setting password");
                        return View();
                    }

                    try
                    {
                        await _context.Users.AddAsync(toInsert);
                        await _context.SaveChangesAsync(User.Identity.Name);
                    }
                    catch (Exception e)
                    {
                        _log.LogError("Exception in adding user: " + e.ToString());
                        _log.LogError($"{User.Identity.Name} attempted to add a user named {toInsert.Username}");
                        ModelState.AddModelError("", "Exception thrown in adding user: " + e.ToString());
                        return View();
                    }
                    _log.LogInformation($"{User.Identity.Name} added a new user {toInsert.Username} with role {toInsert.UserRole}");
                    return RedirectToAction("AllUsers");
                }
            }

        }

        /// <summary>
        /// This action displays a view of all users in the database.
        /// </summary>
        /// <param name="p">From Route: Page Index requested. Defaults to 1.</param>
        /// <param name="searchString">From Route: String to filter presented containers</param>
        /// <param name="sortOrder">From Route: Key and order to sort by (e.g. "username_desc")</param>
        /// <returns></returns>
        [HttpGet, Route("AllUsers")]
        public async Task<IActionResult> AllUsers(int p = 1, string searchString = null, string sortOrder = null)
        {
            if (p < 1) // The PageIndex is named just "p" for shorter URLs, and "page" isn't allowed in asp-route-page, apparently
            {
                return NotFound();
            }

            var filtered = _context.Users.AsNoTracking();

            // Search box filter:
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                filtered = filtered.Where(u => u.Username.Contains(searchString, StringComparison.CurrentCultureIgnoreCase) ||
                    u.FullNameLastFirst.Contains(searchString, StringComparison.CurrentCultureIgnoreCase) ||
                    (u.Email != null && u.Email.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)) ||
                    u.UserRole.ToString().Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                );
            }

            if (string.IsNullOrWhiteSpace(sortOrder))
            {
                filtered = filtered.OrderByDescending(u => u.Username);
            }
            else
            {
                // sortOrder = "username_asc" or "email_desc", etc...
                var sort = sortOrder.Split('_', 2);
                if (sort.Length != 2) return RedirectToAction("AllUsers", new { p = 1 });
                var key = sort[0];
                var order = sort[1];
                _log.LogDebug($"sortOrder={sortOrder}, key={key}, order={order}");
                if (!acceptedSorts.Contains(key) || (order != "asc" && order != "desc"))
                {
                    return NotFound();
                }
                else
                {
                    // key is fine, order is fine:
                    bool asc = order == "asc";

                    switch (key)
                    {
                        case "email":
                            filtered = asc ? filtered.OrderBy(u => u.Email) :
                                filtered.OrderByDescending(u => u.Email);
                            break;
                        case "fullnamelf":
                            filtered = asc ? filtered.OrderBy(u => u.FullNameLastFirst) :
                                filtered.OrderByDescending(u => u.FullNameLastFirst);
                            break;
                        case "username":
                            filtered = asc ? filtered.OrderBy(u => u.Username) :
                                filtered.OrderByDescending(u => u.Username);
                            break;
                        case "usernote":
                            filtered = asc ? filtered.OrderBy(u => u.UserNote).ThenBy(u => u.Username) :
                                filtered.OrderByDescending(u => u.UserNote).ThenByDescending(u => u.Username);
                            break;
                        case "userrole":
                            filtered = asc ? filtered.OrderBy(u => u.UserRole).ThenBy(u => u.Username) :
                                filtered.OrderByDescending(u => u.UserRole).ThenByDescending(u => u.Username);
                            break;
                    }
                }
            }
            ViewData["searchString"] = searchString;
            ViewData["sortOrder"] = sortOrder;
            return View(await PaginatedList<User>.CreateAsync(filtered, p, 10));
        }

        /// <summary>
        /// This action displays the form view for editing a single user.
        /// </summary>
        /// <param name="userId">User.ID of the user to edit.</param>
        /// <returns>Returns the /admin/edituser/id view.</returns>
        [HttpGet, Route("EditUser/{userId}")]
        public async Task<IActionResult> EditUser(int userId)
        {
            User toEdit = await _context.Users.Where(u => u.UserID == userId).FirstOrDefaultAsync();
            if (toEdit == null)
            {
                return NotFound();
            }
            return View(new UserEdit(toEdit));
        }

        /// <summary>
        /// This action receives the form and saves the changes made to the user.  It ignores the username.
        /// </summary>
        /// <param name="userId">User.ID of the user to edit.</param>
        /// <param name="toEdit"></param>
        /// <returns></returns>
        [HttpPost, Route("EditUser/{userId}")]
        public async Task<IActionResult> EditUser(int userId, UserEdit toEdit)
        {
            if (ModelState.IsValid)
            {
                User dbUser = await _context.Users.Where(u => u.UserID == userId).FirstOrDefaultAsync();
                if (dbUser == null)
                {
                    return NotFound();
                }
                //TODO: length requirement on password here: 
                if (toEdit.Password != null)
                {
                    if (toEdit.Password.Length > 5)
                    {
                        var result = dbUser.ChangePassword(toEdit.Password);
                        if (result != PasswordChangeResult.PASSWORD_OK)
                        {
                            ModelState.AddModelError("", "Failure changing password.");
                            return await EditUser(userId);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Password not long enough");
                        return await EditUser(userId);
                    }
                }
                dbUser.FirstName = toEdit.FirstName;
                dbUser.LastName = toEdit.LastName;
                dbUser.Email = toEdit.Email;
                dbUser.UserNote = toEdit.UserNote;
                dbUser.UserRole = toEdit.UserRole;
                await _context.SaveChangesAsync(User.Identity.Name);
                _log.LogInformation($"Admin user {User.Identity.Name} changed user {dbUser.Username}.");
                return RedirectToAction("AllUsers");
            }
            else
            {
                return await EditUser(userId);
            }
        }
    }
}
