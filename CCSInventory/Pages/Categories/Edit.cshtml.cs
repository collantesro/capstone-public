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

namespace CCSInventory.Pages.Categories
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
           ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");

            return Page();
        } 

        public async Task<IActionResult> OnPostAsync()
        {
            string typed = Subcategory.SubcategoryNote;
            int choice = Subcategory.CategoryID;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Subcategory = await _context.Subcategories
                .Include(s => s.Category).FirstOrDefaultAsync(m => m.SubcategoryID == this.Subcategory.SubcategoryID);
            Subcategory.SubcategoryNote = typed;
            Subcategory.CategoryID = choice;

            _context.Attach(Subcategory).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync(User.Identity.Name);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubcategoryExists(Subcategory.SubcategoryID))
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

        private bool SubcategoryExists(int id)
        {
            return _context.Subcategories.Any(e => e.SubcategoryID == id);
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
                return DateTime.UtcNow.ToLocalTime();
            }

            set
            {

            }
        }
    }
}
