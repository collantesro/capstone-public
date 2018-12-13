using CCSInventory.Models;
using CCSInventory.Models.Reports;
using CCSInventory.ViewModels.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CCSInventory.Controllers.Reports
{
    [Route("Reports/Transactions/Incoming")]
    [Authorize("ReadonlyUser")]
    public class IncomingReportsController : Controller
    {
        /// <summary>
        /// The database context
        /// </summary>
        private CCSDbContext context;

        /// <summary>
        /// Initalizes a new instance of the reports controller class
        /// </summary>
        /// <param name="context"></param>
        public IncomingReportsController(CCSDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the index page for the incoming templates
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route(""), Route("Index")]
        public async Task<IActionResult> Index()
        {
            var templateList = await this.context.Templates
                .AsNoTracking()
                .Where(t => t.TemplateType == TemplateType.Incoming)
                .ToListAsync();
            return View("Views/Reports/Transactions/Index.cshtml", templateList);
        }

        /// <summary>
        /// Redirects to a new incoming report
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("NewIncomingReport")]
        public IActionResult NewIncomingReport()
        {
            var incomingOptions = new IncomingOptions();
            incomingOptions.Context = this.context;
            return View("Views/Reports/Transactions/NewReportIncoming.cshtml", incomingOptions);
        }

        /// <summary>
        /// New incoming food report from an existing incoming options
        /// </summary>
        /// <param name="incomingOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("NewIncomingReport")]
        public async Task<IActionResult> NewIncomingReport(IncomingOptions incomingOptions)
        {
            incomingOptions.Context = this.context;
            if (!ModelState.IsValid)
            {
                return View("Views/Reports/Transactions/NewReportIncoming.cshtml", incomingOptions);
            }
            else
            {
                IncomingTemplate temp = new IncomingTemplate();
                temp.CategoryID = incomingOptions.CategoryID;
                //temp.Start = incomingOptions.Start;
                //temp.End = incomingOptions.End;

                var serializedTemplate = JsonConvert.SerializeObject(temp as IncomingTemplate);
                this.context.Templates.Add(new Template
                {
                    TemplateName = incomingOptions.ReportName,
                    TemplateData = serializedTemplate,
                    TemplateType = TemplateType.Incoming,
                });
                await this.context.SaveChangesAsync(User.Identity.Name);
                return await GenerateReport(incomingOptions);
            }
        }

        /// <summary>
        /// Runs an existing template with incoming options
        /// </summary>
        /// <param name="templateID"></param>
        /// <param name="incomingOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("RunExisting/{templateID}")]
        public async Task<IActionResult> RunExisting(int templateID, IncomingOptions incomingOptions)
        {
            incomingOptions.Context = this.context;
            Template template = this.context.Templates.AsNoTracking().FirstOrDefault(t => t.TemplateID == templateID);
            if (template == null)
            {
                ModelState.AddModelError("", "Error: Template does not exist in Database.");
                ViewData["TemplateName"] = "Error";
                return View("Views/Reports/Transactions/RunExisting.cshtml", incomingOptions);
            }

            //_log.LogDebug("ModelState is not valid in RunExisting()");
            ViewData["TemplateName"] = template.TemplateName;
            var incomingTemplate = JsonConvert.DeserializeObject<IncomingTemplate>(template.TemplateData);
            //incomingOptions.Start = incomingTemplate.Start;
            //incomingOptions.End = incomingTemplate.End;
            incomingOptions.CategoryID = incomingTemplate.CategoryID;
            return await GenerateReport(incomingOptions);
        }

        /// <summary>
        /// Runs an existing template
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        [HttpGet, Route("RunExisting/{templateID}")]
        public async Task<IActionResult> RunExisting(int templateID)
        {
            var template = await this.context.Templates
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TemplateID == templateID);
            if (template == null)
            {
                return NotFound();
            }
            else
            {
                var incomingTemplate = JsonConvert.DeserializeObject<IncomingTemplate>(template.TemplateData);
                var incomingOptions = new IncomingOptions(incomingTemplate);
                incomingOptions.Context = this.context;
                return View("Views/Reports/Transactions/RunExisting.cshtml", incomingOptions);
            }
        }

        /// <summary>
        /// Generates a Report
        /// </summary>
        /// <param name="incomingOptions"></param>
        /// <returns></returns>
        private async Task<IActionResult> GenerateReport(IncomingOptions incomingOptions)
        {

            List<TransactionLineItem> relevantTransactions = null;
            if (!incomingOptions.CategoryID.Contains(-1)) //All not selected
            {
                if (incomingOptions.CategoryID.Contains(6)) //Pantry Packs
                {
                    relevantTransactions = await this.context.TransactionLineItems.AsNoTracking()
               .Include(c => c.Transaction)
                   .ThenInclude(a => a.Agency)
                   .ThenInclude(a => a.Address)
               .Include(i => i.Subcategory)
                    .ThenInclude(s => s.Category)
               .Include(i => i.TransactionType)
               .Where(c => (c.Transaction.TransactionDate >= incomingOptions.Start && c.Transaction.TransactionDate < incomingOptions.End.AddDays(1) && c.Transaction.IsOutgoing == false && (c.IsPantryPack || incomingOptions.CategoryID.Contains(c.Subcategory.CategoryID))))
               .ToListAsync();
                }
                else if (incomingOptions.CategoryID.Contains(5)) //Grocery Rescue
                {
                    relevantTransactions = await this.context.TransactionLineItems.AsNoTracking()
               .Include(c => c.Transaction)
                   .ThenInclude(a => a.Agency)
                   .ThenInclude(a => a.Address)
               .Include(i => i.Subcategory)
                    .ThenInclude(s => s.Category)
               .Include(i => i.TransactionType)
               .Where(c => (c.Transaction.TransactionDate >= incomingOptions.Start && c.Transaction.TransactionDate < incomingOptions.End.AddDays(1) && c.Transaction.IsOutgoing == false && (c.TransactionTypeID == 4 || incomingOptions.CategoryID.Contains(c.Subcategory.CategoryID))))
               .ToListAsync();
                }
                else if (incomingOptions.CategoryID.Contains(4)) //USDA
                {
                    relevantTransactions = await this.context.TransactionLineItems.AsNoTracking()
               .Include(c => c.Transaction)
                   .ThenInclude(a => a.Agency)
                   .ThenInclude(a => a.Address)
               .Include(i => i.Subcategory)
                    .ThenInclude(s => s.Category)
               .Include(i => i.TransactionType)
               .Where(c => (c.Transaction.TransactionDate >= incomingOptions.Start && c.Transaction.TransactionDate < incomingOptions.End.AddDays(1) && c.Transaction.IsOutgoing == false && (c.TransactionTypeID == 3 || incomingOptions.CategoryID.Contains(c.Subcategory.CategoryID))))
               .ToListAsync();
                }
                else
                {
                    relevantTransactions = await this.context.TransactionLineItems.AsNoTracking()
               .Include(c => c.Transaction)
                   .ThenInclude(a => a.Agency)
                   .ThenInclude(a => a.Address)
               .Include(i => i.Subcategory)
                    .ThenInclude(s => s.Category)
               .Include(i => i.TransactionType)
               .Where(c => (c.Transaction.TransactionDate >= incomingOptions.Start && c.Transaction.TransactionDate < incomingOptions.End.AddDays(1) && c.Transaction.IsOutgoing == false && incomingOptions.CategoryID.Contains(c.Subcategory.CategoryID)))
               .ToListAsync();
                }
            }
            else
            {
                relevantTransactions = await this.context.TransactionLineItems.AsNoTracking()
                .Include(c => c.Transaction)
                    .ThenInclude(a => a.Agency)
                    .ThenInclude(a => a.Address)
                .Include(i => i.Subcategory)
                        .ThenInclude(s => s.Category)
                .Include(i => i.TransactionType)
                .Where(c => (c.Transaction.TransactionDate >= incomingOptions.Start && c.Transaction.TransactionDate < incomingOptions.End.AddDays(1) && c.Transaction.IsOutgoing == false))
                .ToListAsync();
            }

            //_log.LogDebug($"For date range, sql query returned {relevantContainers.Count} containers");
            if (incomingOptions.CSVDesired)
            {
                // When the list of containers is serialized as-is, it also serializes the navigation properties and it makes a mess.
                // The following Linq statement instead transforms the list of containers into what the output should be.
                var readableList = relevantTransactions.Select(e => new
                {
                    Donor = e.Transaction.Agency.AgencyName,
                    DonorAddress = e.Transaction.Agency.Address.StreetAddress1 + " " + e.Transaction.Agency.Address.StreetAddress2 + " " + e.Transaction.Agency.Address.City + " " + e.Transaction.Agency.Address.State + " " + e.Transaction.Agency.Address.Zip.ToString(),
                    USDA = e.Subcategory.Category.CategoryName == "USDA" ? "Yes" : "No",
                    DateModified = e.ModifiedDate.ToString(),
                    ModifiedBy = e.ModifiedBy.ToString(),
                    Weight = e.Weight.ToString(),
                    Units = e.Units.ToString(),
                    Note = e.TransactionLineItemNote,
                    Category = e.Subcategory.Category.CategoryName,
                    Subcategory = $"{e.Subcategory.SubcategoryName} {e.Subcategory.SubcategoryNote}"
                });

                var csvData = CsvSerializer.SerializeToCsv(readableList);
                // Setting the filename for the upcoming download:
                var contentDisposition = new ContentDisposition
                {
                    FileName = HttpUtility.UrlEncode(incomingOptions.ReportName + ".csv"),
                    Inline = false,
                    DispositionType = "attachment",
                };
                Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
                return Content(csvData, "text/csv; header=present", Encoding.ASCII);
            }
            else
            {
                if (relevantTransactions != null)
                {
                    var grouping = relevantTransactions
                    .GroupBy(g => g.Transaction.Agency.AgencyName)
                    .OrderBy(g => g.Key)
                    .ToList();

                    ViewData["Start"] = incomingOptions.Start;
                    ViewData["End"] = incomingOptions.End;

                    decimal weight = 0;
                    int units = 0;
                    foreach (var item in relevantTransactions)
                    {
                        units += item.Units;
                        weight += item.Weight;
                    }

                    ViewData["Units"] = units;
                    ViewData["Weight"] = weight;

                    string filter = string.Empty;
                    if (incomingOptions.CategoryID.Contains(-1))
                    {
                        filter += "All ";
                    }

                    if (incomingOptions.CategoryID.Contains(1))
                    {
                        filter += "Dry Goods ";
                    }

                    if (incomingOptions.CategoryID.Contains(2))
                    {
                        filter += "Perishable ";
                    }

                    if (incomingOptions.CategoryID.Contains(3))
                    {
                        filter += "Non-Food ";
                    }

                    if (incomingOptions.CategoryID.Contains(4))
                    {
                        filter += "USDA ";
                    }

                    if (incomingOptions.CategoryID.Contains(5))
                    {
                        filter += "Grocery Rescue ";
                    }

                    if (incomingOptions.CategoryID.Contains(6))
                    {
                        filter += "Pantry Pack ";
                    }

                    ViewData["Filter"] = filter;
                    return this.Display(grouping);
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a display
        /// </summary>
        /// <param name="reportContent"></param>
        /// <returns></returns>
        public ViewResult Display(List<IGrouping<string, TransactionLineItem>> reportContent)
        {
            return View("Views/Reports/Transactions/IncomingDisplay.cshtml", reportContent);
        }
    }
}
