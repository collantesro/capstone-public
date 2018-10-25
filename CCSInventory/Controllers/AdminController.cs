using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using CCSInventory.Models;
using CCSInventory.Models.ViewModels;

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
        [Route("AllUsers")]
        /// <summary>
        /// This action displays a view of all users in the database.
        /// </summary>
        /// <returns></returns>
        public async Task<ViewResult> AllUsers()
        {
            return View(await _context.Users.AsNoTracking().ToListAsync());
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
            User toEdit = await _context.Users.Where(u => u.ID == userId).SingleOrDefaultAsync();
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
                User dbUser = await _context.Users.Where(u => u.ID == userId).SingleOrDefaultAsync();
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
                dbUser.Note = toEdit.Note;
                dbUser.Role = toEdit.Role;
                await _context.SaveChangesAsync(User.Identity.Name);
                _log.LogInformation($"Admin user {User.Identity.Name} changed user {dbUser.UserName}.");
                return RedirectToAction("AllUsers");
            }
            else
            {
                return await EditUser(userId);
            }
        }
    }
}
