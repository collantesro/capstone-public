using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCSInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CCSInventory.Pages.Transactions
{
    public class OutgoingUSDAModel : PageModel
    {
        private readonly CCSInventory.Models.CCSDbContext _context;

        public OutgoingUSDAModel(CCSInventory.Models.CCSDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["AgencyID"] = new SelectList(_context.Agencies, "AgencyID", "AgencyName");
            ViewData["TransactionTypeID"] = new SelectList(_context.TransactionTypes, "TransactionTypeID", "TransactionTypeName");
            ViewData["SubcategoryID"] = new SelectList(_context.Subcategories.Where(s => s.CategoryID == 4 && s.SubcategoryID != 4), "SubcategoryID", "SubcategoryName");

            return Page();
        }


        [BindProperty]
        public Transaction Transaction { get; set; }

        public async Task<IActionResult> OnPostAsync(int[] categories, decimal[] weights, string[] usdaNums, int[] cases)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newTransaction = Transaction;
            newTransaction.TransactionDate = DateTime.Now;
            newTransaction.IsOutgoing = true;
            newTransaction.LineItems = new List<TransactionLineItem>();

            _context.Transactions.Add(newTransaction);


            for (int i = 0; i < weights.Length; i++)
            {
                newTransaction.LineItems.Add(new TransactionLineItem
                {
                    TransactionID = newTransaction.TransactionID,
                    SubcategoryID = categories[i],
                    Weight = weights[i],
                    USDANumber = usdaNums[i],
                    Cases = cases[i]
                });
            }

            foreach (var line in newTransaction.LineItems)
            {
                _context.TransactionLineItems.Add(line);
            }


            await _context.SaveChangesAsync(User.Identity.Name);

            return RedirectToPage("./Index");
        }
    }
}