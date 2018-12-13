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
    [Authorize("ReadonlyUser")]
    public class DetailsModel : PageModel
    {
        private readonly CCSInventory.Models.CCSDbContext _context;

        public DetailsModel(CCSInventory.Models.CCSDbContext context)
        {
            _context = context;
        }

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

            //Subcategory.CreatedDate = Subcategory.CreatedDate.ToLocalTime();
            //Subcategory.ModifiedDate = Subcategory.ModifiedDate.ToLocalTime(); 
            return Page();
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
