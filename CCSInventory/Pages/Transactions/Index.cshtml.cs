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
    public class IndexModel : PageModel
    {
        private readonly CCSInventory.Models.CCSDbContext _context;

        public IndexModel(CCSInventory.Models.CCSDbContext context)
        {
            _context = context;
        }

        public IList<Transaction> Transaction { get;set; }

        public async Task OnGetAsync()
        {
            Transaction = await _context.Transactions
                .Include(t => t.Agency)
                .Include(t => t.TransactionType).ToListAsync();
        }
    }
}
