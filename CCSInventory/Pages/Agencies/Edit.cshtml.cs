using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CCSInventory.Models;
using Microsoft.AspNetCore.Authorization;

namespace CCSInventory.Pages.Agencies
{
    [Authorize("StandardUser")]
    public class EditModel : PageModel
    {
        private readonly CCSInventory.Models.CCSDbContext _context;

        public EditModel(CCSInventory.Models.CCSDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Agency Agency { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Agency = await _context.Agencies
                .Include(a => a.Address)
                .Include(a => a.MailingAddress).FirstOrDefaultAsync(m => m.AgencyID == id);

            if (Agency == null)
            {
                return NotFound();
            }
           ViewData["AddressID"] = new SelectList(_context.Addresses, "AddressID", "City");
           ViewData["MailingAddressID"] = new SelectList(_context.Addresses, "AddressID", "City");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Agency).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgencyExists(Agency.AgencyID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AgencyExists(int id)
        {
            return _context.Agencies.Any(e => e.AgencyID == id);
        }
    }
}
