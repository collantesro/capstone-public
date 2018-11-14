using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CCSInventory.Models;
using Microsoft.AspNetCore.Authorization;

namespace CCSInventory.Pages.Agencies
{
    [Authorize("ReadonlyUser")]
    public class IndexModel : PageModel
    {
        private readonly CCSInventory.Models.CCSDbContext _context;

        public IndexModel(CCSInventory.Models.CCSDbContext context)
        {
            _context = context;
        }

        public IList<Agency> Agency { get;set; }

        public async Task OnGetAsync()
        {
            Agency = await _context.Agencies.Where(A=>A.IsArchived==false)
                .Include(a => a.Address)
                .Include(a => a.MailingAddress).ToListAsync();
        }
    }
}
