using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CCSInventory.Models;
using Microsoft.AspNetCore.Authorization;

namespace CCSInventory.Pages.ChangeLog
{
    [Authorize("AdminUser")]
    public class DetailsModel : PageModel
    {
        private readonly CCSInventory.Models.CCSDbContext _context;

        public DetailsModel(CCSInventory.Models.CCSDbContext context)
        {
            _context = context;
        }

        public Log Log { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Log = await _context.Log.FirstOrDefaultAsync(m => m.LogID == id);

            if (Log == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
