using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CCSInventory.Models;
using Microsoft.AspNetCore.Authorization;

namespace CCSInventory.Pages.Categories
{
    [Authorize("StandardUser")]
    public class CreateModel : PageModel
    {
        private readonly CCSInventory.Models.CCSDbContext _context;
        private DateTime dt = DateTime.Now.Date.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);

        public CreateModel(CCSInventory.Models.CCSDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            return Page();
        }

        [BindProperty]
        public Subcategory Subcategory { get; set; }

        [BindProperty]
        public DateTime Date
        {
            get
            {
                return dt;
            }

            set
            {
                if(this.dt != value)
                {
                    dt = value;
                }
            }
        } 

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Subcategory.ModifiedDate = dt;
            //Subcategory.CreatedDate = dt;

            _context.Subcategories.Add(Subcategory);
            await _context.SaveChangesAsync(User.Identity.Name);

            return RedirectToPage("./Index");
        }
    }
}
