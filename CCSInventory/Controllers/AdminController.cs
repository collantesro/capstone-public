using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using CCSInventory.Models;
using CCSInventory.Models.ViewModels.Admin;
using System;

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

        public AdminController(CCSDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _log = logger;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        /// <summary>
        /// This default action loads up the main view for /admin/
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("NewUser")]
        /// <summary>
        /// This action displays the form to create a new user.
        /// </summary>
        /// <returns></returns>
        public ViewResult NewUser()
        {
            return View();
        }

        [HttpPost]
        [Route("NewUser")]
        /// <summary>
        /// This is the action backing the form to verify and create the new user on POST.
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
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

        [HttpGet]
        [Route("AllUsers")]
        /// <summary>
        /// This action displays a view of all users in the database.
        /// </summary>
        /// <returns></returns>
        public async Task<ViewResult> AllUsers()
        {
            // The front-end displays the users using JavaScript.  This is passed as a
            // string of JSON to the ViewData, so the view can insert it in the
            // <script> tag in the head.
            ViewData["Users"] = JsonConvert.SerializeObject(
                await _context.Users.AsNoTracking().Select(u => new
                {
                    UserID = u.UserID,
                    Username = u.Username,
                    FullNameLastFirst = u.FullNameLastFirst,
                    Email = u.Email,
                    UserRole = u.UserRole.ToString(),
                    UserNote = u.UserNote,
                    ModifiedDateVisible = u.ModifiedDate.ToLocalTime().ToString(),
                    ModifiedDateInt = u.ModifiedDate.ToFileTime(),
                }).ToListAsync()
            );
            return View();
        }

        [HttpGet]
        [Route("EditUser/{userId}")]
        /// <summary>
        /// This action displays the form view for editing a single user.
        /// </summary>
        /// <param name="userId">User.ID of the user to edit.</param>
        /// <returns>Returns the /admin/edituser/id view.</returns>
        public async Task<IActionResult> EditUser(int userId)
        {
            User toEdit = await _context.Users.Where(u => u.UserID == userId).FirstOrDefaultAsync();
            if (toEdit == null)
            {
                return NotFound();
            }
            return View(new UserEdit(toEdit));
        }

        [HttpPost]
        [Route("EditUser/{userId}")]
        /// <summary>
        /// This action receives the form and saves the changes made to the user.  It ignores the username.
        /// </summary>
        /// <param name="userId">User.ID of the user to edit.</param>
        /// <param name="toEdit"></param>
        /// <returns></returns>
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
