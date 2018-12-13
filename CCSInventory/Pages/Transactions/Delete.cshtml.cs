using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CCSInventory.Models;
using Microsoft.AspNetCore.Authorization;

namespace CCSInventory.Pages.Transactions
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
        public Transaction Transaction { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Transaction = await _context.Transactions
                .Include(t => t.Agency)
                .Include(t => t.LineItems)
                .FirstOrDefaultAsync(m => m.TransactionID == id);

            if (Transaction == null)
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

            Transaction = await _context.Transactions.FindAsync(id);

            if (Transaction != null)
            {
                Transaction.IsArchived = true;
                await _context.SaveChangesAsync(User.Identity.Name);
            }

            return RedirectToPage("./Index");
        }

        public DateTime CbDate { get => Transaction.CreatedDate.ToLocalTime(); }
        public DateTime MbDate { get => Transaction.ModifiedDate.ToLocalTime(); }
    }
}
