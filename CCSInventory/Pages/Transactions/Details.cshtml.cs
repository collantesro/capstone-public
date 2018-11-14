using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CCSInventory.Models;

namespace CCSInventory.Pages.Transactions
{
    public class DetailsModel : PageModel
    {
        private readonly CCSInventory.Models.CCSDbContext _context;

        public DetailsModel(CCSInventory.Models.CCSDbContext context)
        {
            _context = context;
        }

        public Transaction Transaction { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Transaction = await _context.Transactions
                .Include(t => t.Agency)
                .Include(t => t.TransactionType).FirstOrDefaultAsync(m => m.TransactionID == id);

            if (Transaction == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
