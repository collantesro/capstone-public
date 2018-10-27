using CCSInventory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Components
{
    public class PantryPackTransactionsViewComponent : ViewComponent
    {
        private CCSDbContext _context; 

        public PantryPackTransactionsViewComponent(CCSDbContext dbcontext)
        {
            _context = dbcontext;
        }

        public IViewComponentResult Invoke(int inOrOut)
        {
            //return all
            if(inOrOut == 0)
            {
                List<PantryPackTransaction> transactions = _context.PantryPackTransactions.Include(t => t.PackType).ToList(); 

                return View(transactions);
            }
            //return incoming
            else if(inOrOut == 1)
            {
                List<PantryPackTransaction> transactions = _context.PantryPackTransactions.Where(t => t.Qty > 0).Include(t => t.PackType).ToList();

                return View(transactions);
            }
            //return outgoing
            else
            {
                List<PantryPackTransaction> transactions = _context.PantryPackTransactions.Where(t => t.Qty < 0).Include(t => t.PackType).ToList();

                return View(transactions);
            }
        }
    }
}
