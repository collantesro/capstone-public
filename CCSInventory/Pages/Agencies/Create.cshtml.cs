using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CCSInventory.Models;
using Microsoft.AspNetCore.Authorization;

namespace CCSInventory.Pages.Agencies
{
    [Authorize("StandardUser")]
    public class CreateModel : PageModel
    {
        private readonly CCSInventory.Models.CCSDbContext _context;

        public CreateModel(CCSInventory.Models.CCSDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["AddressID"] = new SelectList(_context.Addresses, "AddressID", "City");
        ViewData["MailingAddressID"] = new SelectList(_context.Addresses, "AddressID", "City");
            return Page();
        }

        [BindProperty]
        public Agency Agency { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emptyAgency = new Agency();

            if (await TryUpdateModelAsync<Agency>(
                emptyAgency,
                "agency",//prefix in form fields)
                s => s.AgencyID, s => s.Address, s => s.AgencyName, s => s.AddressID))


                _context.Agencies.Add(Agency);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
