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

namespace CCSInventory.Pages.Transactions
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
            ViewData["AgencyID"] = new SelectList(_context.Agencies, "AgencyID", "AgencyName");
            ViewData["TransactionTypeID"] = new SelectList(_context.TransactionTypes, "TransactionTypeID", "TransactionTypeName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int choice = Transaction.AgencyID;

            if (!ModelState.IsValid)
            {
                return Page();
            }        

            Transaction = await _context.Transactions
                .Include(t => t.Agency)
                .Include(t => t.LineItems)
                .FirstOrDefaultAsync(m => m.TransactionID == this.Transaction.TransactionID);

            Transaction.AgencyID = choice;

            _context.Attach(Transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(User.Identity.Name);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(Transaction.TransactionID))
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

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionID == id);
        }

        public DateTime CbDate
        {
            get
            {
                return Transaction.CreatedDate.ToLocalTime();
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
