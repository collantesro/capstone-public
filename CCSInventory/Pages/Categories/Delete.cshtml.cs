using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CCSInventory.Models;
using Microsoft.AspNetCore.Authorization;

namespace CCSInventory.Pages.Categories
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
        public Subcategory Subcategory { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subcategory = await _context.Subcategories
                .Include(s => s.Category).FirstOrDefaultAsync(m => m.SubcategoryID == id);

            if (Subcategory == null)
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

            Subcategory = await _context.Subcategories.FindAsync(id);

            if (Subcategory != null)
            {
                Subcategory.IsArchived = true;
                await _context.SaveChangesAsync(User.Identity.Name);
            }

            return RedirectToPage("./Index");
        }

        public DateTime CbDate
        {
            get
            {
                return Subcategory.CreatedDate.ToLocalTime();
            }

            set
            {

            }
        }

        public DateTime MbDate
        {
            get
            {
                return Subcategory.ModifiedDate.ToLocalTime();
            }

            set
            {

            }
        }
    }
}
