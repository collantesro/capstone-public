using System.Linq;
using System.Threading.Tasks;
using CCSInventory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CCSInventory.Pages
{
    [Authorize("ReadonlyUser")]
    public class SelfModel : PageModel
    {
        public User U { get; set; }

        private readonly CCSDbContext _context;
        private readonly ILogger<SelfModel> _log;

        public SelfModel(CCSDbContext context, ILogger<SelfModel> log)
        {
            _context = context;
            _log = log;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var dbUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(c => c.Username == User.Identity.Name);
                if (dbUser == null)
                {
                    return NotFound();
                }

                U = dbUser;
                return Page();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
