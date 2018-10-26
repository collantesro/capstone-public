using CCSInventory.Models;
using CCSInventory.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Controllers
{
    public class PantryPackController : Controller
    {

        private CCSDbContext _context; 

        public PantryPackController(CCSDbContext context)
        {
            _context = context;
        }

        public IActionResult LoadTransactionPage()
        {
            return View("Transaction");
        }

        public IActionResult LoadAllTransactionPage()
        {
            return View("AllTransactions");
        }

        public IActionResult LoadOutGoingTransactionPage()
        {
            return View("OutGoingTransactions");
        }

        public void OutGoingTransaction(PantryPackTransactionType transaction)
        {
            PantryPackTransactionType newTransaction = new PantryPackTransactionType();

            newTransaction.Qty = transaction.Qty;
            newTransaction.PackType = transaction.PackType; 


        }
    }
}
