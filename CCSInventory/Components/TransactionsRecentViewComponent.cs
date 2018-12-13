using System.Linq;
using System.Threading.Tasks;
using CCSInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CCSInventory.Components
{
    public class TransactionsRecentViewComponent : ViewComponent
    {

        private readonly CCSDbContext _context;
        private readonly ILogger<TransactionsRecentViewComponent> _log;
        private int maxRecents;

        public TransactionsRecentViewComponent(CCSDbContext context, IConfiguration config, ILogger<TransactionsRecentViewComponent> log)
        {
            _context = context;
            _log = log;

            int configRecents = config.GetValue<int>("Constants:Transactions:RecentMax");
            _log.LogDebug("Value interpreted from Constants:Transactions:RecentMax: " + configRecents);
            maxRecents = configRecents > 0 ? configRecents : 5;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var recentTransactions = await _context.Transactions.AsNoTracking()
                .OrderByDescending(t => t.ModifiedDate)
                .Take(maxRecents)
                .Include(t => t.Agency)
                .Include(t => t.LineItems)
                .ToListAsync();
            return View(recentTransactions);
        }
    }
}
