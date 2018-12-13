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

namespace CCSInventory.Pages.Agencies
{
    [Authorize("ReadonlyUser")]
    public class ArchivedAgencyModel : PageModel
    {
        private readonly CCSInventory.Models.CCSDbContext _context;

        public ArchivedAgencyModel(CCSInventory.Models.CCSDbContext context)
        {
            _context = context;
        }

        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public IList<Agency> Agency { get; set; }
        public PaginatedList<Agency> AgencyList { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {

            CurrentSort = sortOrder;

            Agency = await _context.Agencies.Where(a => a.IsArchived)
                .Include(a => a.Address)
                .ToListAsync();

            this.AgencyList = new PaginatedList<Agency>(Agency.ToList(), Agency.Count, 1, 10);

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<Agency> agency = from a in _context.Agencies select a;
            //IList<Subcategory> subcat = null;

            if (!String.IsNullOrEmpty(searchString))
            {
                agency = agency.Where(a =>
                    a.AgencyName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                || a.AgencyNote != null && a.AgencyNote.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));
                //|| c.SubcategoryNote.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                // SubcategoryNote doesn't work because some of the elements are null and throw a null exception pointer. 
            }

            if (agency != null)
            {
                int pageSize = 10;
                AgencyList = await PaginatedList<Agency>.CreateAsync(agency, pageIndex ?? 1, pageSize);
            }
        }
    }
}
