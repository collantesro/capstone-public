using CCSInventory.Models;
using CCSInventory.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Pages.Categories
{
    [Authorize("ReadonlyUser")]
    public class IndexModel : PageModel
    {
        private readonly CCSInventory.Models.CCSDbContext _context;

        public IndexModel(CCSInventory.Models.CCSDbContext context)
        {
            _context = context;
        }

        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public IList<Subcategory> Subcategory { get; set; }
        public IList<Category> Category { get; set; }
        public PaginatedList<Subcategory> SubcategoriesList { get; set; }      

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;

            Subcategory = await _context.Subcategories
                        .Include(s => s.Category)
                        .ToListAsync();

            this.SubcategoriesList = new PaginatedList<Subcategory>(Subcategory.ToList(), Subcategory.Count, 1, 10);

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Subcategory> subcat = from c in _context.Subcategories select c;
            //IList<Subcategory> subcat = null;

            if (!String.IsNullOrEmpty(searchString))
            { 
                //Ability to search through all 3 columns of data with one search bar. 
                subcat = subcat.Where(c =>
                    c.SubcategoryName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                || c.Category.CategoryName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                || c.SubcategoryNote != null && c.SubcategoryNote.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
            }

            if (subcat != null)
            {
                int pageSize = 10;
                SubcategoriesList = await PaginatedList<Subcategory>.CreateAsync(subcat, pageIndex ?? 1, pageSize);
            }
        }
    }
}
