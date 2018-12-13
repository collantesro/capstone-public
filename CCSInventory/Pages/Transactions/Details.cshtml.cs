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
    [Authorize("ReadonlyUser")]
    public class DetailsModel : PageModel
    {
        private readonly CCSInventory.Models.CCSDbContext _context;

        public DetailsModel(CCSInventory.Models.CCSDbContext context)
        {
            _context = context;
        }

        public Transaction Transaction { get; set; }

        public IQueryable<TransactionLineItem> LineItems { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Transaction = await _context.Transactions
                .Include(t => t.Agency)
                    .ThenInclude(a => a.Address)
                .FirstOrDefaultAsync(m => m.TransactionID == id);

            if (Transaction == null)
            {
                return NotFound();
            }

            this.LineItems = this._context.TransactionLineItems.Where(t => t.TransactionID == id);

            foreach (var item in this.LineItems)
            {
                item.Subcategory = this._context.Subcategories.FirstOrDefault(s => s.SubcategoryID == item.SubcategoryID);
                item.TransactionType = this._context.TransactionTypes.FirstOrDefault(t => t.TransactionTypeID == item.TransactionTypeID);
            }

            return Page();
        }

        public DateTime CbDate { get => Transaction.CreatedDate.ToLocalTime(); }
        public DateTime MbDate { get => Transaction.ModifiedDate.ToLocalTime(); }
    }
}
