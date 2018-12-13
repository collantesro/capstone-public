using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CCSInventory.Models;
using CCSInventory.Models.Reports;
using CCSInventory.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CCSInventory.Controllers.Reports
{

    /// <summary>
    /// This controller handles reporting functionality for Containers Reports.
    /// These reports query the Containers DbSet in CCSDbContext according to parameters requested by the user.
    /// </summary>
    [Route("Reports/Containers")]
    [Authorize("ReadonlyUser")]
    public class ContainersReportController : Controller
    {
        private readonly CCSDbContext _context;
        private readonly ILogger<ContainersReportController> _log;

        public ContainersReportController(CCSDbContext context, ILogger<ContainersReportController> log)
        {
            _context = context;
            _log = log;
        }

        /// <summary>
        /// This Index action displays a page with a button for a new report and a list of templates.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route(""), Route("Index")]
        public async Task<IActionResult> Index()
        {
            var templateList = await _context.Templates
                .AsNoTracking()
                .Where(t => t.TemplateType == TemplateType.Containers)
                .ToListAsync();
            return View("Views/Reports/Containers/Index.cshtml", templateList);
        }

        /// <summary>
        /// This action displays the view that allows the user to specify their report parameters for a new template.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("NewReport")]
        public IActionResult NewReport()
        {
            return View("Views/Reports/Containers/NewReport.cshtml", new ContainerOptions());
        }

        /// <summary>
        /// This action receives the form data and processes the report.  It saves the template for reuse.
        /// </summary>
        /// <param name="reportOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("NewReport")]
        public async Task<IActionResult> NewReport(ContainerOptions reportOptions)
        {
            _log.LogDebug("Value for options: " + reportOptions.ToString());
            if (!ModelState.IsValid)
            {
                return View("Views/Reports/Containers/NewReport.cshtml", reportOptions);
            }
            else
            {
                /**** CHECK HERE IF SAVE TEMPLATE BOX IS CHECKED ****/
                var serializedTemplate = JsonConvert.SerializeObject(reportOptions as ContainerTemplate);
                _context.Templates.Add(new Template
                {
                    TemplateName = reportOptions.ReportName,
                    TemplateData = serializedTemplate,
                    TemplateType = TemplateType.Containers,
                });
                await _context.SaveChangesAsync(User.Identity.Name);
                return await GenerateReport(reportOptions);
            }
        }

        /// <summary>
        /// This action displays a view showing a previously-run template and allows the user to specify the start and end ranges.
        /// </summary>
        /// <param name="templateID"></param>
        /// <returns></returns>
        [HttpGet, Route("RunExisting/{templateID}")]
        public async Task<IActionResult> RunExisting(int templateID)
        {
            var template = await _context.Templates
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TemplateID == templateID);
            if (template == null)
            {
                return NotFound();
            }
            else
            {
                var containerTemplate = JsonConvert.DeserializeObject<ContainerTemplate>(template.TemplateData);
                return View("Views/Reports/Containers/RunExisting.cshtml", new ContainerOptions(containerTemplate));
            }
        }

        /// <summary>
        /// This action receives the form data for an existing template and runs the report.
        /// </summary>
        /// <param name="templateID"></param>
        /// <param name="reportOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("RunExisting/{templateID}")]
        public async Task<IActionResult> RunExisting(int templateID, ContainerOptions reportOptions)
        {
            Template template = _context.Templates.AsNoTracking().FirstOrDefault(t => t.TemplateID == templateID);
            if (template == null)
            {
                ModelState.AddModelError("", "Error: Template does not exist in Database.");
                ViewData["TemplateName"] = "Error";
                return View("Views/Reports/Containers/RunExisting.cshtml", reportOptions);
            }
            if (ModelState.IsValid)
            { // since the View surrounds part of the form in a disabled fieldset, this is unlikely to be true.
                _log.LogDebug("ModelState is valid in RunExisting()");
                return await (GenerateReport(reportOptions));
            }
            else
            {
                _log.LogDebug("ModelState is not valid in RunExisting()");
                ViewData["TemplateName"] = template.TemplateName;
                var containerTemplate = JsonConvert.DeserializeObject<ContainerTemplate>(template.TemplateData);
                reportOptions.ReportName = containerTemplate.ReportName;
                reportOptions.ExpirationStart = containerTemplate.ExpirationStart;
                reportOptions.ExpirationEnd = containerTemplate.ExpirationEnd;
                reportOptions.CategoryIDs = containerTemplate.CategoryIDs;
                reportOptions.SubcategoryIDs = containerTemplate.SubcategoryIDs;
                reportOptions.LocationIDs = containerTemplate.LocationIDs;
                reportOptions.WeightLowerBound = containerTemplate.WeightLowerBound;
                reportOptions.WeightUpperBound = containerTemplate.WeightUpperBound;
                reportOptions.UnitsLowerBound = containerTemplate.UnitsLowerBound;
                reportOptions.UnitsUpperBound = containerTemplate.UnitsUpperBound;
                reportOptions.IncludeArchived = containerTemplate.IncludeArchived;
                return await GenerateReport(reportOptions);
            }
        }

        /// <summary>
        /// This private method is what actually performs the report filtering based on the reportOptions specified.
        /// </summary>
        /// <param name="reportOptions"></param>
        /// <returns></returns>
        private async Task<IActionResult> GenerateReport(ContainerOptions reportOptions)
        {
            List<Container> relevantContainers = await _context.Containers.AsNoTracking()
                    .Include(c => c.Subcategory)
                        .ThenInclude(s => s.Category)
                    .Include(c => c.Location)
                    .Where(c => (c.CreatedDate >= reportOptions.Start && c.CreatedDate < reportOptions.End.AddDays(1)))
                    .ToListAsync();
            _log.LogDebug($"For date range, sql query returned {relevantContainers.Count} containers");

            IEnumerable<Container> filteredContainers = relevantContainers
                .Where(c => c.Weight >= reportOptions.WeightLowerBound && c.Weight <= reportOptions.WeightUpperBound)
                .Where(c => c.Units >= reportOptions.UnitsLowerBound && c.Units <= reportOptions.UnitsUpperBound);

            // If IncludeArchived is false, remove all containers where its IsArchived is true.
            if (!reportOptions.IncludeArchived)
            {
                filteredContainers = filteredContainers.Where(c => !c.IsArchived);
            }

            // (Logic incomplete) If the LocationIDs contains locations, only keep those containers in those desired locations.
            if (reportOptions.LocationIDs.Count > 0)
            {
                var locationsDesired = new HashSet<int>(reportOptions.LocationIDs);
                filteredContainers = filteredContainers.Where(c => locationsDesired.Contains(c.LocationID));
            }

            // Combine both categoryIDs and subcategoryIDs into one HashSet:

            HashSet<int> subcategoriesDesired = new HashSet<int>(reportOptions.SubcategoryIDs);
            foreach (int categoryID in reportOptions.CategoryIDs)
            {
                foreach (int subcategory in _context.Subcategories.AsNoTracking().Where(s => s.CategoryID == categoryID).Select(s => s.SubcategoryID))
                {
                    subcategoriesDesired.Add(subcategory);
                }
            }

            // Only filter for containers where their SubcategoryID is in the hash set from above.
            if (subcategoriesDesired.Count > 0)
            {
                filteredContainers = filteredContainers.Where(c => subcategoriesDesired.Contains(c.SubcategoryID));
            }

            relevantContainers = filteredContainers.ToList();

            _log.LogDebug("Filtered Containers: " + relevantContainers.Count);

            if (reportOptions.ExcelFormat)
            {
                // When the list of containers is serialized as-is, it also serializes the navigation properties and it makes a mess.
                // The following Linq statement instead transforms the list of containers into what the output should be.
                var readableList = relevantContainers.Select(e => new
                {
                    ContainerID = e.ContainerID,
                    BinNumber = e.BinNumber,
                    Archived = e.IsArchived ? "Yes" : "No",
                    USDA = e.Subcategory.Category.CategoryName == "USDA" ? "Yes" : "No",
                    DateModified = e.ModifiedDate.ToLocalTime(),
                    ExpirationDate = e.ExpirationDate.HasValue ? e.ExpirationDate.Value.ToString("MM/dd/yyyy") : "",
                    ModifiedBy = e.ModifiedBy,
                    Weight = e.Weight,
                    Units = e.Units,
                    Note = e.ContainerNote,
                    Location = e.Location.LocationName,
                    Category = e.Subcategory.Category.CategoryName,
                    Subcategory = $"{e.Subcategory.SubcategoryName} {e.Subcategory.SubcategoryNote}"
                });

                using (MemoryStream output = new MemoryStream())
                {
                    ExcelUtil.GenerateExcel(output, readableList);
                    output.Seek(0, SeekOrigin.Begin);
                    return File(output.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        HttpUtility.UrlEncode(reportOptions.ReportName + ".xlsx"), false);
                }
            }
            else
            {
                var grouping = relevantContainers
                    .GroupBy(c => c.Subcategory.Category.CategoryName)
                    .OrderBy(g => g.Key)
                    .ToList();

                ViewData["Start"] = reportOptions.Start;
                ViewData["End"] = reportOptions.End;
                return Display(grouping);
            }
        }

        /// <summary>
        /// Returns the ViewResult for a list of containers
        /// </summary>
        /// <param name="reportContent"></param>
        /// <returns></returns>
        public ViewResult Display(List<IGrouping<string, Container>> reportContent)
        {
            return View("Views/Reports/Containers/Display.cshtml", reportContent);
        }
    }
}
