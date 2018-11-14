using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CCSInventory.Models;

namespace CCSInventory.Pages.Transactions
{
    public class OutgoingModel : PageModel
    {
        private readonly CCSInventory.Models.CCSDbContext _context;

        public OutgoingModel(CCSInventory.Models.CCSDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["AgencyID"] = new SelectList(_context.Agencies, "AgencyID", "AgencyName");
            ViewData["TransactionTypeID"] = new SelectList(_context.TransactionTypes, "TransactionTypeID", "TransactionTypeName");
            ViewData["CategoryID"] = new SelectList(_context.Categories.Where(s => s.CategoryID != 4 && s.CategoryID != 5), "CategoryID", "CategoryName");

            return Page();
        }


        [BindProperty]
        public Transaction Transaction { get; set; }

        public async Task<IActionResult> OnPostAsync(int[] categories, decimal[] weights)
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
                    Weight = weights[i]

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