using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

using CCSInventory.Models;
using CCSInventory.Models.ViewModels;

namespace CCSInventory.Pages {
    public class LoginModel : PageModel {
        [BindProperty]
        public UserLogin Login { get; set; }

        // For redirects back to the page they were viewing
        // [BindProperty]
        // public string ReturnUri { get; set; }

        private CCSDbContext _context;

        public LoginModel(CCSDbContext context){
            _context = context;
        }

        public IActionResult OnPost(){
            if(!ModelState.IsValid){
                return Page();
            } else {
                // Pull user from DB
                User user = _context.Users.SingleOrDefault(u => u.UserName.ToLower() == Login.UserName.ToLower());
                if(user == null || !user.MatchesPassword(Login.Password)){
                    ModelState.AddModelError("", "Login Failed: Invalid username or password");
                    return Page();
                }
                
                // Login is successful here.  Add Authentication Cookie now.
                //TODO: Finish login authentication
                return RedirectToPage("Index");
            }
        }
    }
}
