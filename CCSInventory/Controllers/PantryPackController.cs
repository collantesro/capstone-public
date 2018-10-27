using CCSInventory.Models;
using CCSInventory.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Controllers 
{
    [Route("PantryPack")]
    public class PantryPackController : Controller 
    {

        private CCSDbContext _context; 

        public PantryPackController(CCSDbContext context)
        {
            _context = context;
        }
        
        [Route("AllTransaction")]
        [HttpGet]
        public IActionResult LoadAllTransactionPage()
        {
            return View("AllTransactions");
        }

        [Route("OutGoingTransactions")]
        [HttpGet]
        public IActionResult LoadOutGoingTransactionPage()
        {
            return View("OutGoingTransactions");
        }

        [Route("InGoingTransactions")]
        [HttpGet]
        public IActionResult LoadInGoingTransactionPage()
        {
            return View("OutGoingTransactions");
        }

        [Route("AddOutGoingTransaction")]
        [HttpGet]
        public IActionResult LoadAddOutGoingTransactionPage()
        {
            return View("AddOutGoingTransaction");
        }

        [Route("ViewAllOutGoingTransactions")]
        [HttpGet]
        public IActionResult ViewAllOutGoingTransactions()
        {
            List<PantryPackTransactionType> transactionList = new List<PantryPackTransactionType>(); 
            return ViewComponent("ViewAllOutGoingTransactions", transactionList);
        }

        [Route("OutGoingTransactions")]
        [HttpPost]
        public void OutGoingTransaction(PantryPackTransactionType transaction)
        {
            PantryPackTransaction newTransaction = new PantryPackTransaction();
            PantryPackType newPackType = new PantryPackType(); 

            if(transaction.Qty < 0)
            {
                newTransaction.Qty = transaction.Qty - (transaction.Qty * 2);
            }
            else
            {
                newTransaction.Qty = transaction.Qty;
            }

            newPackType.Name = transaction.Name;
            newTransaction.PackType = newPackType;

            _context.PantryPackType.Add(newPackType);
            _context.PantryPackTransactions.Add(newTransaction);

            _context.SaveChanges(User.Identity.Name); 

        }

        [Route("InGoingTransaction")]
        [HttpPost]
        public void InGoingTransaction(PantryPackTransactionType transaction)
        {
            PantryPackTransaction newTransaction = new PantryPackTransaction();
            PantryPackType newPackType = new PantryPackType();

            newTransaction.Qty = transaction.Qty;

            newPackType.Name = transaction.Name;
            newTransaction.PackType = newPackType;

            _context.PantryPackType.Add(newPackType);
            _context.PantryPackTransactions.Add(newTransaction);

            _context.SaveChanges(User.Identity.Name);

        }
    }
}
