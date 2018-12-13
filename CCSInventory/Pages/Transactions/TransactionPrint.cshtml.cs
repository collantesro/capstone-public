using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCSInventory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CCSInventory.Pages.Transactions
{
    [Authorize("ReadonlyUser")]
    public class TransactionPrintModel : PageModel
    {
        [BindProperty]
        public Transaction Transaction { get; set; }

        [BindProperty]
        public Address Address { get; set; }

        [BindProperty]
        public Agency Agency { get; set; }

        [BindProperty]
        public IQueryable<TransactionLineItem> LineItems { get; set; }

        private CCSDbContext context;

        private static int? TransID;

        public TransactionPrintModel(CCSInventory.Models.CCSDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                if (TransID == null)
                {
                    return NotFound();
                }
            }

            if (id != null)
            {
                TransID = (int)id;
            }

            Transaction = await this.context.Transactions
                .Include(t => t.Agency)
                .Include(t => t.LineItems)
                .FirstOrDefaultAsync(m => m.TransactionID == TransID);

            if (Transaction == null)
            {
                return NotFound();
            }

            this.Agency = await this.context.Agencies
                .FirstOrDefaultAsync(m => m.AgencyID == Transaction.AgencyID);

            this.Address = await this.context.Addresses
                .FirstOrDefaultAsync(m => m.AddressID == Agency.AddressID);

            this.LineItems = this.context.TransactionLineItems.Where(t => t.TransactionID == Transaction.TransactionID);
            foreach (var item in this.LineItems)
            {
                item.Subcategory = this.context.Subcategories.FirstOrDefault(s => s.SubcategoryID == item.SubcategoryID);
                item.TransactionType = this.context.TransactionTypes.FirstOrDefault(t => t.TransactionTypeID == item.TransactionTypeID);
            }

            return Page();
        }
    }
}
