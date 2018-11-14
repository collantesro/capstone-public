using CCSInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Components
{
    public class ContainersRecentViewComponent : ViewComponent
    {
        private readonly CCSDbContext _context;
        // Used to get the appsettings.json value for Constants.ContainersRecentMax
        private readonly IConfiguration _config;
        private readonly ILogger<ContainersRecentViewComponent> _log;
        
        // overridable in appsettings.json under Constants:ContainersRecentMax
        private int maxRecents;

        public ContainersRecentViewComponent(CCSDbContext context, IConfiguration config, ILogger<ContainersRecentViewComponent> log)
        {
            _context = context;
            _config = config;
            _log = log;

            int configRecents = _config.GetValue<int>("Constants:ContainersRecentMax");
            _log.LogInformation("Value for Constants:ContainersRecentMax=" + configRecents);
            maxRecents = configRecents > 0 ? configRecents : 7;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var recentContainers = await _context.Containers.AsNoTracking()
                .OrderByDescending(c => c.ModifiedDate)
                .Take(maxRecents)
                .Include(c => c.Subcategory)
                .ToListAsync();
            return View(recentContainers);
        }
    }
}
