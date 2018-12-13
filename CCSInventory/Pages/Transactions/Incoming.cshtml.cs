using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CCSInventory.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;

namespace CCSInventory.Pages.Transactions
{
    [Authorize("StandardUser")]
    public class CreateModel : PageModel
    {
        public enum FormTypes
        {
            Regular,

            USDA,

            GroceryRescue
        }

        private readonly CCSInventory.Models.CCSDbContext context;

        public CreateModel(CCSInventory.Models.CCSDbContext context)
        {
            this.context = context;

            var removeTransType = this.context.TransactionTypes.Where(c => !string.Equals(c.TransactionTypeName, "USDA") && !string.Equals(c.TransactionTypeName, "Grocery Rescue") && c.IsOutgoing == false);

            CreateModel.Agencies = this.context.Agencies;
            CreateModel.USDASubcategories = this.context.Subcategories.Where(s => s.CategoryID == 4);
            CreateModel.TransactionTypes = removeTransType;
            CreateModel.Addresses = this.context.Addresses;
            CreateModel.Categories = this.context.Categories.Where(c => string.Equals(c.CategoryName, "Dry Goods") || string.Equals(c.CategoryName, "Perishable") || string.Equals(c.CategoryName, "Non-Food"));
            CreateModel.GroceryRescueSubcategories = this.context.Subcategories.Where(s => s.CategoryID == 5);
        }

        public IActionResult OnGet()
        {
            ViewData["TransactionTypeID"] = new SelectList(this.context.TransactionTypes, "TransactionTypeID", "TransactionTypeName");
            return Page();
        }


        [BindProperty]
        public Transaction Transaction { get; set; }

        [BindProperty]
        public DateTime Date { get; set; } = DateTime.Now.Date.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);

        [BindProperty]
        public static FormTypes Type { get; set; } = FormTypes.Regular;

        public static int TypeIndex { get; set; } = 0;

        public static DbSet<Agency> Agencies { get; set; }

        public static IQueryable<Category> Categories { get; set; }

        public static IQueryable<Subcategory> USDASubcategories { get; set; }

        public static IQueryable<Subcategory> GroceryRescueSubcategories { get; set; }

        public static DbSet<Address> Addresses { get; set; }

        public static IQueryable<TransactionType> TransactionTypes { get; set; }

        public List<FormTypes> FormTypesList { get; set; } = new List<FormTypes>() { FormTypes.Regular, FormTypes.USDA, FormTypes.GroceryRescue, };

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var partnerName = Request.Form["Transaction.Agency"].ToString();
            Agency partner = null;

            if (!string.IsNullOrWhiteSpace(partnerName))
            {
                var index = partnerName.IndexOf('-');
                var id = partnerName.Substring(0, index).Trim();
                partner = this.context.Agencies.FirstOrDefault(p => p.AgencyID.ToString() == id);
            }

            if (partner == null)
            {
                ModelState.AddModelError("", "Partner doesn't exist in the database.");
                return Page();
            }

            int formIndex;
            int.TryParse(Request.Form["TypeIndex"], out formIndex);
            var formType = (FormTypes)formIndex;

            Transaction transaction = new Transaction()
            {
                TransactionDate = Date,
                AgencyID = partner.AgencyID,
                IsOutgoing = false,
            };

            this.SaveTransaction(transaction);

            if (formType == FormTypes.Regular)
            {
                if (!this.SubmitRegular(partner, transaction))
                {
                    return Page();
                }
            }
            else if (formType == FormTypes.GroceryRescue)
            {
                if (!this.SubmitGroceryRescue(partner, transaction))
                {
                    return Page();
                }

            }
            else if (formType == FormTypes.USDA)
            {
                if(!this.SubmitUSDA(partner, transaction))
                {
                    return Page();
                }
            }

            //return RedirectToPage("./TransactionPrint", transaction.TransactionID);

            return RedirectToPage("./TransactionPrint", new
            {
                id = transaction.TransactionID
            });
        }

        private bool SubmitUSDA(Agency partner, Transaction transaction)
        {
            int lineCount = Request.Form["lineCount"].Count;
            var categories = Request.Form[$"categories"];
            var weights = Request.Form[$"weights"];
            var casesList = Request.Form[$"casesList"];

            for (int i = 0; i < lineCount; i++)
            {
                var c = categories[i].ToString();
                var w = weights[i].ToString();
                var cs = casesList[i].ToString();

                int weight = 0;
                int cases = 0;
                Int32.TryParse(w, out weight);
                Int32.TryParse(cs, out cases);
                string usdaNumber = c.Split(' ')[0];
                var foodCategory = this.context.Subcategories.FirstOrDefault(x => x.SubcategoryName == usdaNumber);

                if (foodCategory == null)
                {
                    ModelState.AddModelError("", "Food Category doesn't exist in the database.");
                    return false;
                }

                TransactionLineItem lineItem = new TransactionLineItem()
                {
                    TransactionID = transaction.TransactionID,
                    SubcategoryID = foodCategory.SubcategoryID,
                    TransactionTypeID = 3, //USDA
                    IsPantryPack = false,
                    Weight = weight,
                    Units = cases
                };

                this.SaveLineItem(lineItem);
            }

            return true;
        }

        private bool SubmitGroceryRescue(Agency partner, Transaction transaction)
        {
            int lineCount = Request.Form["lineCount"].Count;
            var categories = Request.Form[$"categories"];
            var weights = Request.Form[$"weights"];

            for (int i = 0; i < lineCount; i++)
            {
                var c = categories[i].ToString();
                var w = weights[i].ToString();

                int weight = 0;
                Int32.TryParse(w, out weight);

                if (string.IsNullOrWhiteSpace(c))
                {
                    continue;
                }

                var foodCategory = this.context.Subcategories.FirstOrDefault(x => x.SubcategoryName == c);

                if (foodCategory == null)
                {
                    ModelState.AddModelError("", "Food Category doesn't exist in the database.");
                    return false;
                }

                TransactionLineItem lineItem = new TransactionLineItem()
                {
                    TransactionID = transaction.TransactionID,
                    SubcategoryID = foodCategory.SubcategoryID,
                    TransactionTypeID = 4, //Grocery Rescue
                    IsPantryPack = false,
                    Weight = weight
                };

                this.SaveLineItem(lineItem);
            }

            return true;
        }

        private bool SubmitRegular(Agency partner, Transaction transaction)
        {
            int lineCount = Request.Form["lineCount"].Count;
            var categories = Request.Form[$"categories"];
            var weights = Request.Form[$"weights"];
            var quantities = Request.Form[$"quantities"];
            var types = Request.Form[$"lineItemTypes"];

            for (int i = 0; i < lineCount; i++)
            {
                var c = categories[i].ToString();
                var w = weights[i].ToString();
                var q = quantities[i].ToString();
                var t = types[i].ToString();

                if (!string.IsNullOrWhiteSpace(c) && !string.IsNullOrWhiteSpace(t))
                {
                    int weight = 0;
                    Int32.TryParse(w, out weight);
                    var foodCategory = this.context.Categories.FirstOrDefault(x => x.CategoryName == c);
                    var transactionType = this.context.TransactionTypes.FirstOrDefault(x => x.TransactionTypeName == t);

                    if (foodCategory == null || transactionType == null)
                    {
                        ModelState.AddModelError("", "Food Category or Line Item Type do not exist in the database.");
                        return false;
                    }

                    if (!string.IsNullOrWhiteSpace(q))
                    {
                        int quantity = 0;
                        Int32.TryParse(q, out quantity);

                        TransactionLineItem lineItem = new TransactionLineItem()
                        {
                            TransactionID = transaction.TransactionID,
                            SubcategoryID = foodCategory.CategoryID,
                            TransactionTypeID = transactionType.TransactionTypeID,
                            Units = quantity,
                            //Changed this... 
                            Weight = weight,
                            IsPantryPack = true

                        };

                        this.SaveLineItem(lineItem);
                    }
                    else
                    {
                        TransactionLineItem lineItem = new TransactionLineItem()
                        {
                            TransactionID = transaction.TransactionID,
                            SubcategoryID = foodCategory.CategoryID,
                            TransactionTypeID = transactionType.TransactionTypeID,
                            Weight = weight,
                            Units = 0,
                            IsPantryPack = false,
                        };

                        this.SaveLineItem(lineItem);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Needed to save the transaction to the context
        /// </summary>
        /// <param name="transaction"></param>
        public async void SaveTransaction(Transaction transaction)
        {
            if (transaction == null)
            {
                return;
            }

            this.context.Add(transaction);
            await this.context.SaveChangesAsync(User.Identity.Name);
        }

        /// <summary>
        /// Needed to save the line item to the context
        /// </summary>
        /// <param name="lineItem"></param>
        public async void SaveLineItem(TransactionLineItem lineItem)
        {
            if (lineItem == null)
            {
                return;
            }

            this.context.Add(lineItem);
            await this.context.SaveChangesAsync(User.Identity.Name);
        }
    }
}
