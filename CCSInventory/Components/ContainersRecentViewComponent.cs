using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

using CCSInventory.Models;

namespace CCSInventory.Components
{
    public class ContainersRecentViewComponent : ViewComponent
    {
        private readonly CCSDbContext _context;
        private readonly ILogger<ContainersRecentViewComponent> _log;

        // overridable in appsettings.json under Constants:Containers:RecentMax
        private int maxRecents;

        /// <summary>
        /// Constructor for the ContainersRecentView that make use of Dependency Injection
        /// </summary>
        /// <param name="context">DbContext of our database</param>
        /// <param name="config">Access to configuration from appsettings.json</param>
        /// <param name="log"></param>
        public ContainersRecentViewComponent(CCSDbContext context, IConfiguration config, ILogger<ContainersRecentViewComponent> log)
        {
            _context = context;
            _log = log;

            int configRecents = config.GetValue<int>("Constants:Containers:RecentMax");
            _log.LogDebug("Value interpreted from Constants:Containers:RecentMax: " + configRecents);
            maxRecents = configRecents > 0 ? configRecents : 5;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var recentContainers = await _context.Containers.AsNoTracking()
                .OrderByDescending(c => c.ModifiedDate)
                .Take(maxRecents)
                .Include(c => c.Subcategory)
                    .ThenInclude(s => s.Category)
                .ToListAsync();
            return View(recentContainers);
        }
    }
}
