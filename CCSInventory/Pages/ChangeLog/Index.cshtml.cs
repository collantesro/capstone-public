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

namespace CCSInventory.Pages.ChangeLog
{
    [Authorize("AdminUser")]
    public class IndexModel : PageModel
    {
        private readonly CCSInventory.Models.CCSDbContext _context;

        public IndexModel(CCSInventory.Models.CCSDbContext context)
        {
            _context = context;
        }

        public string LastNameSort { get; set; }
        public string FirstNameSort { get; set; }
        public string UserNameSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public IList<Log> Log { get; set; }
        public PaginatedList<Log> LogList { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            Log = await _context.Log
                .Include(l => l.User).ToListAsync();

            this.LogList = new PaginatedList<Log>(Log.ToList(), Log.Count, 1, 10);

            CurrentSort = sortOrder;

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IList<Log> logs = null;

            if (!String.IsNullOrEmpty(searchString))
            {
                logs = this._context.Log.Where(c => c.Description.Contains(searchString) || c.User.FirstName.Contains(searchString) || c.Date.Year.ToString().Contains(searchString) || c.Date.Month.ToString().Contains(searchString)).ToList();

            }

            if (logs != null)
            {
                int pageSize = 10;
                LogList = new PaginatedList<Log>(logs.ToList(), logs.Count, pageIndex ?? 1, pageSize);
            }
        }

    }
}
