using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CCSInventory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CCSInventory.Pages.Agencies
{
    [Authorize("StandardUser")]
    public class CreateModel : PageModel
    {
        private readonly CCSDbContext _context;

        public CreateModel(CCSDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["States"] = States;
            //States = new List<SelectListItem>(_context.Addresses, "State", "State");
            return Page();
        }

        [BindProperty]
        public Agency Agency { get; set; }

        //public static SelectList States { get; set; }

        public static List<SelectListItem> States = new List<SelectListItem>()
        {
            new SelectListItem() { Text="Alabama", Value="AL"},
            new SelectListItem() { Text="Alaska", Value="AK"},
            new SelectListItem() { Text="Arizona", Value="AZ"},
            new SelectListItem() { Text="Arkansas", Value="AR"},
            new SelectListItem() { Text="California", Value="CA"},
            new SelectListItem() { Text="Colorado", Value="CO"},
            new SelectListItem() { Text="Connecticut", Value="CT"},
            new SelectListItem() { Text="District of Columbia", Value="DC"},
            new SelectListItem() { Text="Delaware", Value="DE"},
            new SelectListItem() { Text="Florida", Value="FL"},
            new SelectListItem() { Text="Georgia", Value="GA"},
            new SelectListItem() { Text="Hawaii", Value="HI"},
            new SelectListItem() { Text="Idaho", Value="ID"},
            new SelectListItem() { Text="Illinois", Value="IL"},
            new SelectListItem() { Text="Indiana", Value="IN"},
            new SelectListItem() { Text="Iowa", Value="IA"},
            new SelectListItem() { Text="Kansas", Value="KS"},
            new SelectListItem() { Text="Kentucky", Value="KY"},
            new SelectListItem() { Text="Louisiana", Value="LA"},
            new SelectListItem() { Text="Maine", Value="ME"},
            new SelectListItem() { Text="Maryland", Value="MD"},
            new SelectListItem() { Text="Massachusetts", Value="MA"},
            new SelectListItem() { Text="Michigan", Value="MI"},
            new SelectListItem() { Text="Minnesota", Value="MN"},
            new SelectListItem() { Text="Mississippi", Value="MS"},
            new SelectListItem() { Text="Missouri", Value="MO"},
            new SelectListItem() { Text="Montana", Value="MT"},
            new SelectListItem() { Text="Nebraska", Value="NE"},
            new SelectListItem() { Text="Nevada", Value="NV"},
            new SelectListItem() { Text="New Hampshire", Value="NH"},
            new SelectListItem() { Text="New Jersey", Value="NJ"},
            new SelectListItem() { Text="New Mexico", Value="NM"},
            new SelectListItem() { Text="New York", Value="NY"},
            new SelectListItem() { Text="North Carolina", Value="NC"},
            new SelectListItem() { Text="North Dakota", Value="ND"},
            new SelectListItem() { Text="Ohio", Value="OH"},
            new SelectListItem() { Text="Oklahoma", Value="OK"},
            new SelectListItem() { Text="Oregon", Value="OR"},
            new SelectListItem() { Text="Pennsylvania", Value="PA"},
            new SelectListItem() { Text="Rhode Island", Value="RI"},
            new SelectListItem() { Text="South Carolina", Value="SC"},
            new SelectListItem() { Text="South Dakota", Value="SD"},
            new SelectListItem() { Text="Tennessee", Value="TN"},
            new SelectListItem() { Text="Texas", Value="TX"},
            new SelectListItem() { Text="Utah", Value="UT"},
            new SelectListItem() { Text="Vermont", Value="VT"},
            new SelectListItem() { Text="Virginia", Value="VA"},
            new SelectListItem() { Text="Washington", Value="WA"},
            new SelectListItem() { Text="West Virginia", Value="WV"},
            new SelectListItem() { Text="Wisconsin", Value="WI"},
            new SelectListItem() { Text="Wyoming", Value="WY"}
        };


        [BindProperty]
        public bool HasAddress { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            // Manually validating the properties of the form.

            // AgencyName is an Alternate Key, so it needs to be unique, non-null, and non-empty when inserting into the database
            if (string.IsNullOrWhiteSpace(Agency.AgencyName))
            {
                ModelState.AddModelError("Agency.AgencyName", "Agency Name cannot be empty");
                return Page();
            }

            // Remove whitespace before and after these fields:
            Agency.AgencyName = Agency.AgencyName.Trim();
            Agency.PhoneNumber = Agency.PhoneNumber != null ? Agency.PhoneNumber.Trim() : "";
            Agency.EmailAddress = Agency.EmailAddress != null ? Agency.EmailAddress.Trim() : "";

            // An agency can be inserted into the database without an address.
            if (!Agency.HasAddress)
            {
                // When HasAddress is false, reset all the other address stuff to make sure Entity Framework Core doesn't
                // try to create relationships with the addresses.
                Agency.Address = null;
                Agency.AddressID = null;

                // Check that the AgencyName isn't already taken.  Again, this is an alternate key, so it should be unique or the DB is gonna throw a fit:
                var agencyName = await _context.Agencies.FirstOrDefaultAsync(a => a.AgencyName == Agency.AgencyName);
                if (agencyName != null) // Agency with that name already exists.  They must specify a different name.
                {
                    ModelState.AddModelError("Agency.AgencyName", "This exact agency name is already in use.");
                    return Page();
                }
                else
                {
                    // The AgencyName is fine.
                    _context.Agencies.Add(Agency);
                    await _context.SaveChangesAsync(User.Identity.Name);
                }
            }
            else
            {
                // Here is where the rest of the address properties are checked by hand.

                // In order to give feedback on the whole form at once (rather than submit, error, resubmit, error, ...)
                // this boolean flag is used to mark if any of the below failed.
                bool goodModel = true;
                if (string.IsNullOrWhiteSpace(Agency.Address.StreetAddress1))
                {
                    goodModel = false;
                    ModelState.AddModelError("Agency.Address.StreetAddress1", "Street Address 1 cannot be empty.");
                }
                if (string.IsNullOrWhiteSpace(Agency.Address.City))
                {
                    goodModel = false;
                    ModelState.AddModelError("Agency.Address.City", "City cannot be empty.");
                }
                if (string.IsNullOrWhiteSpace(Agency.Address.State))
                {
                    goodModel = false;
                    ModelState.AddModelError("Agency.Address.State", "State cannot be empty.");
                }
                if (string.IsNullOrWhiteSpace(Agency.Address.Zip))
                {
                    goodModel = false;
                    ModelState.AddModelError("Agency.Address.Zip", "Zip cannot be empty.");
                }

                if (!goodModel)
                {
                    return Page();
                }
                else
                { // Everything appears to check out.  Insert the agency:
                    _context.Agencies.Add(Agency);
                    _context.SaveChanges(User.Identity.Name);
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
