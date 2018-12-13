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

namespace CCSInventory.Pages.Transactions
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
        public IList<Transaction> Transaction { get; set; }
        public PaginatedList<Transaction> TransactionList { get; set; }

        public static DbSet<TransactionType> transactionTypes { get; set; }
        //public static DbSet<DonationType> DonationTypes { get; set; }

        public IList<Transaction> Transactions { get; set; }


        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            // Caution, by calling ToList(), all transactions (and their line items) are loaded into the program.
            // As the Transactions table grows, this will progressively be more and more data.
            // Consider re-writing to have most of this happen in the SQL query.
            Transaction = await _context.Transactions
                  .Include(t => t.Agency)
                  .Include(t => t.LineItems)
                  .Where(t => !t.IsArchived)
                  .ToListAsync();
            this.TransactionList = new PaginatedList<Transaction>(Transaction.ToList(), Transaction.Count, 1, 10);
            CurrentSort = sortOrder;

            if (searchString != null)
            {
                if (searchString.ToLower().Equals("incoming"))
                {
                    searchString = "0";
                }

                if (searchString.ToLower().Equals("outgoing"))
                {
                    searchString = "1";
                }
            }

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IList<Transaction> transactions = null;

            if (!String.IsNullOrEmpty(searchString))
            {
                // Caution, by calling ToList(), all transactions (and their line items) are loaded into the program.
                // As the Transactions table grows, this will progressively be more and more data.
                // Consider re-writing to have most of this happen in the SQL query.
                transactions = this._context.Transactions.Where(c => !c.IsArchived && (c.LineItems.Any(li => li.TransactionType.TransactionTypeName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)) || c.Agency.AgencyName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase) || c.CreatedBy.Contains(searchString, StringComparison.CurrentCultureIgnoreCase))).ToList();
            }

            if (transactions != null)
            {
                int pageSize = 10;
                TransactionList = new PaginatedList<Transaction>(transactions.ToList(), transactions.Count, pageIndex ?? 1, pageSize);
            }
        }
    }
}
