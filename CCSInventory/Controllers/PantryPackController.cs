using CCSInventory.Models;
using CCSInventory.Models.ViewModels;
using CCSInventory.Models.ViewModels.PantryPack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Controllers 
{
    [Route("PantryPack")]
    [Authorize("ReadonlyUser")]
    public class PantryPackController : Controller 
    {

        private CCSDbContext _context;
        private ILogger<PantryPackController> _log;

        public PantryPackController(CCSDbContext context, ILogger<PantryPackController> log)
        {
            _context = context;
            _log = log;
        }

        #region Pages

        [Route("AllTransaction")]
        [HttpGet]
        public IActionResult LoadAllTransactionPage()
        {
            return View("AllTransactions");
        }

        [Route("OutGoingTransactions")]
        [HttpGet]
        [Authorize("StandardUser")]
        public IActionResult LoadOutGoingTransactionPage()
        {
            ViewData["types"] = _context.PantryPackType.ToList();
            return View("AddOutGoingTransaction");
        }

        [Route("InGoingTransactions")]
        [HttpGet]
        [Authorize("StandardUser")]
        public IActionResult LoadInGoingTransactionPage()
        {
            ViewData["types"] = _context.PantryPackType.ToList();
            return View("AddInGoingTransaction");
        }

        [Route("AddOutGoingTransaction")]
        [HttpGet]
        [Authorize("StandardUser")]
        public IActionResult LoadAddOutGoingTransactionPage()
        {
            ViewData["types"] = _context.PantryPackType.ToList();

            return View("AddOutGoingTransaction");
        }

        [Route("AddInGoingTransaction")]
        [HttpGet]
        public IActionResult LoadAddInGoingTransactionPage()
        {
            ViewData["types"] = _context.PantryPackType.ToList();
            return View("AddInGoingTransaction");
        }

        [Route("AddNewTransaction")]
        [HttpGet]
        [Authorize("StandardUser")]
        public IActionResult LoadNewTransaction()
        {
            return View("AddNewTransaction");
        }

        [Route("PantryPackTypes")]
        [HttpGet]
        public IActionResult LoadPantryPackType()
        {
            ViewData["types"] = _context.PantryPackType.ToList();
            return View("PantryPackTypes");
        }

        //[Route("EditPantryPackTypes/{PantryPackTypeId}")]'
        [Route("EditPantryPackTypes")]
        [HttpGet]
        [Authorize("StandardUser")]
        public IActionResult LoadUpdatePantryPackTypes() //int PantryPackTypeId
        {
            ViewData["types"] = _context.PantryPackType.ToList();
            return View("EditPantryPackTypes");
        }

        [Route("ViewAllOutGoingTransactions")]
        [HttpGet]
        public IActionResult ViewAllOutGoingTransactions()
        {
            List<PantryPackTransactionType> transactionList = new List<PantryPackTransactionType>(); 
            return ViewComponent("ViewAllOutGoingTransactions", transactionList);
        }

        #endregion

        [Route("AddNewPantryPackType")]
        [HttpPost]
        [Authorize("StandardUser")]
        public IActionResult AddNewPantryPackType(PantryPackType packType)
        {
            PantryPackType newPackType = packType;

            _context.PantryPackType.Add(newPackType);
            _context.SaveChanges();

            return Redirect("/index");
        }

        [Route("UpdatePantryPackType")]
        [HttpPost]
        [Authorize("StandardUser")]
        public void UpdatePantryPackType(EditPantryPackType NewPackType)
        {
            EditPantryPackType update = NewPackType;
            int test = 0;
            //_context.PantryPackType.Update(packType.PantryPackTypeName);
        }

        [Route("OutGoingTransactions")]
        [HttpPost]
        [Authorize("StandardUser")]
        public IActionResult OutGoingTransaction(PantryPackTransaction transaction)
        {
            PantryPackTransaction newTransaction = transaction;
            
            if(transaction.Qty < 0)
            {
                newTransaction.Qty = transaction.Qty - (transaction.Qty * 2);
            }
            else
            {
                newTransaction.Qty = transaction.Qty;
            }

            _context.PantryPackTransactions.Add(newTransaction);

            _context.SaveChanges(User.Identity.Name);

            return Redirect("/index");
        }

        [Route("InGoingTransaction")]
        [HttpPost]
        [Authorize("StandardUser")]
        public IActionResult InGoingTransaction(PantryPackTransaction transaction)
        {
            PantryPackTransaction newTransaction = transaction;

            _context.PantryPackTransactions.Add(newTransaction);

            _context.SaveChanges(User.Identity.Name);

            return Redirect("/index");
        }

        [Route("NewTransaction")]
        [HttpPost]
        [Authorize("StandardUser")]
        public void NewTransaction(PantryPackTransactionType transaction)
        {
            PantryPackTransaction newTransaction = new PantryPackTransaction();
            PantryPackType newPackType = new PantryPackType();

            if (transaction.TransactionType == 0)
            {
                newTransaction.Qty = transaction.Qty;

                newPackType.PantryPackTypeName = transaction.Name;
                newTransaction.PantryPackType = newPackType;

                _context.PantryPackType.Add(newPackType);
                _context.PantryPackTransactions.Add(newTransaction);

                _context.SaveChanges(User.Identity.Name);
            }
            else if (transaction.TransactionType == 1)
            {
                if (transaction.Qty < 0)
                {
                    newTransaction.Qty = transaction.Qty - (transaction.Qty * 2);
                }
                else
                {
                    newTransaction.Qty = transaction.Qty;
                }

                newPackType.PantryPackTypeName = transaction.Name;
                newTransaction.PantryPackType = newPackType;

                _context.PantryPackType.Add(newPackType);
                _context.PantryPackTransactions.Add(newTransaction);

                _context.SaveChanges(User.Identity.Name);

                Redirect("Views/PantryPack/AllTransactions");

            }

        }
    }
}
