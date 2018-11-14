using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CCSInventory.Models;
using Microsoft.AspNetCore.Authorization;

namespace CCSInventory.Pages.Agencies
{
    [Authorize("StandardUser")]
    public class DeleteModel : PageModel
    {
        private readonly CCSInventory.Models.CCSDbContext _context;

        public DeleteModel(CCSInventory.Models.CCSDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Agency Agency{ get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Agency = await _context.Agencies
                .Include(c => c.Address).FirstOrDefaultAsync(m => m.AgencyID == id);

            if (Agency == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Agency = await _context.Agencies.FindAsync(id);

            if (Agency != null)
            {
                Agency.IsArchived = true;
                await _context.SaveChangesAsync(User.Identity.Name);
            }

            return RedirectToPage("./Index");
        }
    }
}
