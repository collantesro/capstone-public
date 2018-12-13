using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CCSInventory.Models;
using CCSInventory.ViewModels.Containers;
using CCSInventory.Utilities;

namespace CCSInventory.Controllers
{
    /// <summary>
    /// This controller manages the creation of bins to track donations in the warehouses.
    /// </summary>
    [Authorize("ReadonlyUser")]
    [Route("Containers")]
    public class ContainersController : Controller
    {
        private readonly CCSDbContext _context;
        private readonly ILogger<ContainersController> _log;

        // These three are overridable in appsettings.json
        private readonly int binNumberMin; // Constants:Containers:BinNumberMin
        private readonly int binNumberMax; // Constants:Containers:BinNumberMax
        private readonly int pageSize; // Constants:Containers:PageSize

        private readonly static HashSet<string> acceptedSorts = new HashSet<string>{
            "binNumber",
            "cases",
            "category",
            "created",
            "expiration",
            "location",
            "modified",
            "subcategory",
            "weight",
        };

        public ContainersController(CCSDbContext context, IConfiguration config,
            ILogger<ContainersController> log)
        {
            _context = context;
            _log = log;

            // Configuration of certain settings from appsettings.json
            binNumberMin = config.GetValue<int>("Constants:Containers:BinNumberMin");
            binNumberMax = config.GetValue<int>("Constants:Containers:BinNumberMax");
            pageSize = config.GetValue<int>("Constants:Containers:PageSize");

            // Sanity checks in case above isn't defined, or max <= min;
            if (binNumberMin < 1) binNumberMin = 1;
            if (binNumberMax <= binNumberMin) binNumberMax = Int32.MaxValue - 1;
            if (pageSize < 1) pageSize = 1;

            _log.LogDebug($"Value interpreted from Constants:Containers:BinNumberMin: {binNumberMin}.");
            _log.LogDebug($"Value interpreted from Constants:Containers:BinNumberMax: {binNumberMax}.");
            _log.LogDebug($"Value interpreted from Constants:Containers:PageSize: {pageSize}.");
        }

        /// <summary>
        /// This action is the default index page.  It displays all the containers with pagination.
        /// It also supports ordering.
        /// </summary>
        /// <param name="p">From Route: Page Index requested. Defaults to 1.</param>
        /// <param name="searchString">From Route: String to filter presented containers</param>
        /// <param name="sortOrder">From Route: Key and order to sort by (e.g. "weight_desc")</param>
        /// <returns></returns>
        [HttpGet, Route(""), Route("Index")]
        public async Task<IActionResult> Index(int p = 1, string searchString = null, string sortOrder = null)
        {
            if (p < 1) // The PageIndex is named just "p" for shorter URLs, and "page" isn't allowed in asp-route-page, apparently
            {
                return NotFound();
            }

            var filtered = _context.Containers.AsNoTracking()
                .Include(c => c.Subcategory)
                    .ThenInclude(s => s.Category)
                .Include(c => c.Location)
                .Where(c => !c.IsArchived);

            if (!String.IsNullOrEmpty(searchString))
            {
                //SubcategoryName (and note), CategoryName, ContainerNote
                filtered = filtered.Where(f =>
                f.BinNumber.ToString().Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                || f.Location.LocationName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                || (f.Subcategory.SubcategoryName != null && f.Subcategory.SubcategoryName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase))
                || (f.Subcategory.Category.CategoryName != null && f.Subcategory.Category.CategoryName.Contains(searchString, StringComparison.CurrentCultureIgnoreCase))
                || (f.Subcategory.Category.CategoryNote != null && f.Subcategory.Category.CategoryNote.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)));
                //|| c.SubcategoryNote.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                // SubcategoryNote doesn't work because some of the elements are null and throw a null exception pointer. 
            }

            if (string.IsNullOrWhiteSpace(sortOrder))
            {
                filtered = filtered.OrderByDescending(c => c.CreatedDate);
            }
            else
            {
                // sortOrder = "binNumber_asc" or "location_desc" or "expiration_asc"
                var sort = sortOrder.Split('_', 2);
                if (sort.Length != 2) return RedirectToAction("Index", new { p = 1, searchString = searchString });
                var key = sort[0];
                var order = sort[1];
                _log.LogDebug($"sortOrder={sortOrder}, key={key}, order={order}");
                if (!acceptedSorts.Contains(key) || (order != "asc" && order != "desc"))
                {
                    return NotFound();
                }
                else
                {
                    // key is fine, order is fine:
                    bool asc = order == "asc";

                    switch (key)
                    {
                        // Cases from this.acceptedSorts:
                        case "binNumber":
                            filtered = asc ? filtered.OrderBy(c => c.BinNumber) :
                                filtered.OrderByDescending(c => c.BinNumber);
                            break;
                        case "cases":
                            filtered = asc ? filtered.OrderBy(c => c.Units) :
                                filtered.OrderByDescending(c => c.Units);
                            break;
                        case "category":
                            filtered = asc ?
                                filtered.OrderBy(c => c.Subcategory.Category.CategoryName).ThenBy(c => c.Subcategory.SubcategoryName) :
                                filtered.OrderByDescending(c => c.Subcategory.Category.CategoryName).ThenByDescending(c => c.Subcategory.SubcategoryName);
                            break;
                        case "created":
                            filtered = asc ? filtered.OrderBy(c => c.CreatedDate) :
                                filtered.OrderByDescending(c => c.CreatedDate);
                            break;
                        case "expiration":
                            filtered = asc ? filtered.OrderBy(c => c.ExpirationDate) :
                                filtered.OrderByDescending(c => c.ExpirationDate);
                            break;
                        case "location":
                            filtered = asc ? filtered.OrderBy(c => c.Location.LocationName) :
                                filtered.OrderByDescending(c => c.Location.LocationName);
                            break;
                        case "modified":
                            filtered = asc ? filtered.OrderBy(c => c.ModifiedDate) :
                                filtered.OrderByDescending(c => c.ModifiedDate);
                            break;
                        case "subcategory":
                            filtered = asc ? filtered.OrderBy(c => c.Subcategory.SubcategoryName) :
                                filtered.OrderByDescending(c => c.Subcategory.SubcategoryName);
                            break;
                        case "weight":
                            filtered = asc ? filtered.OrderBy(c => c.Weight) :
                                filtered.OrderByDescending(c => c.Weight);
                            break;
                    }
                }
            }
            ViewData["searchString"] = searchString;
            ViewData["sortOrder"] = sortOrder;
            return View(await PaginatedList<Container>.CreateAsync(filtered, p, pageSize));
        }

        /// <summary>
        /// This action loads the view to display the specified container
        /// </summary>
        /// <param name="containerID">ContainerID of specific container</param>
        /// <returns></returns>
        [HttpGet, Route("Details/{containerID}")]
        public async Task<IActionResult> Details(int containerID)
        {
            Container container = await _context.Containers.AsNoTracking()
                .Include(c => c.Subcategory)
                    .ThenInclude(s => s.Category)
                .Include(c => c.Location)
                .FirstOrDefaultAsync(c => c.ContainerID == containerID && !c.IsArchived);
            if (container == null)
            {
                return NotFound();
            }
            return View(container);
        }

        /// <summary>
        /// This action loads the view to edit the container, provided it's not already archived.
        /// </summary>
        /// <param name="containerID"></param>
        /// <returns></returns>
        [HttpGet, Route("Edit/{containerID}")]
        [Authorize("StandardUser")]
        public async Task<IActionResult> Edit(int containerID)
        {
            Container container = await _context.Containers.AsNoTracking()
                .FirstOrDefaultAsync(c => c.ContainerID == containerID && !c.IsArchived);
            if (container == null)
            {
                return NotFound();
            }
            else
            {
                DropdownsViewData();
                return View(new NewBin(container));
            }
        }

        /// <summary>
        /// This action performs the edit functionality on the container.  If the edit is done on another day
        /// than when it was created, a copy is made and the old one is archived.
        /// </summary>
        /// <param name="containerID"></param>
        /// <param name="newBin"></param>
        /// <returns></returns>
        [HttpPost, Route("Edit/{containerID}")]
        [Authorize("StandardUser")]
        public async Task<IActionResult> Edit(int containerID, NewBin newBin)
        {
            Container oldContainer = await _context.Containers
                .FirstOrDefaultAsync(c => c.ContainerID == containerID && !c.IsArchived);
            if (oldContainer == null)
            {
                return NotFound();
            }
            else
            {
                if (oldContainer.CreatedDate.ToLocalTime().Date == DateTime.Today)
                {
                    if (await IsBinValid(newBin))
                    {
                        oldContainer.Weight = newBin.Weight;
                        oldContainer.Units = newBin.Units;
                        oldContainer.SubcategoryID = newBin.SubcategoryID;
                        oldContainer.ContainerNote = newBin.ContainerNote;
                        oldContainer.LocationID = newBin.LocationID;
                        oldContainer.ExpirationDate = newBin.ExpirationDate;
                        await _context.SaveChangesAsync(User.Identity.Name);
                        _log.LogInformation($"User {User.Identity.Name} edited container in place with ID={containerID}");
                        return RedirectToAction("Details", new { containerID = containerID });
                    }
                    else
                    {
                        DropdownsViewData();
                        return View(newBin);
                    }
                }
                else
                {
                    if (await IsBinValid(newBin))
                    {
                        oldContainer.IsArchived = true;
                        Container replacement = new Container();
                        replacement.BinNumber = oldContainer.BinNumber;
                        replacement.Weight = newBin.Weight;
                        replacement.Units = newBin.Units;
                        replacement.SubcategoryID = newBin.SubcategoryID;
                        replacement.ContainerNote = newBin.ContainerNote;
                        replacement.LocationID = newBin.LocationID;
                        replacement.ExpirationDate = newBin.ExpirationDate;
                        _context.Add(replacement);
                        await _context.SaveChangesAsync(User.Identity.Name);
                        _log.LogInformation($"User {User.Identity.Name} edited container by duplication with ID={containerID}, new ID={replacement.ContainerID}");
                        return RedirectToAction("Details", new { containerID = replacement.ContainerID });
                    }
                    else
                    {
                        DropdownsViewData();
                        return View(newBin);
                    }
                }
            }
        }

        /// <summary>
        /// This action is a confirmation before marking a container as Archived.  It displays a container
        /// similar to the Details action but with a button to delete the bin.  Deleting the bin runs the
        /// ConfirmDelete action, which has no view.
        /// </summary>
        /// <param name="containerID">The ContainerID of the container to display the Delete view for.</param>
        /// <returns></returns>
        [Route("Delete/{containerID}")]
        [Authorize("StandardUser")]
        public async Task<IActionResult> Delete(int containerID)
        {
            Container container = await _context.Containers
                .AsNoTracking()
                .Include(c => c.Subcategory)
                    .ThenInclude(s => s.Category)
                .Include(c => c.Location)
                .FirstOrDefaultAsync(c => c.ContainerID == containerID && !c.IsArchived);
            if (container == null)
            {
                return NotFound();
            }
            else return View(container);
        }

        /// <summary>
        /// This action is a partner to the Delete action.  It does the actual Archive of the Delete.
        /// On "Delete" it redirects to the Index action.
        /// </summary>
        /// <param name="containerID">The ContainerID of the container to mark as Archived.</param>
        /// <returns></returns>
        [Route("ConfirmDelete/{containerID}")]
        [Authorize("StandardUser")]
        public async Task<IActionResult> ConfirmDelete(int containerID)
        {
            Container container = await _context.Containers
                .FirstOrDefaultAsync(c => c.ContainerID == containerID);
            if (container == null)
            {
                return NotFound();
            }
            else
            {
                container.IsArchived = true;
                await _context.SaveChangesAsync(User.Identity.Name);
                _log.LogInformation($"User {User.Identity.Name} has archived container with ContainerID={containerID}");
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// This action loads the view to create a new bin.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("Create")]
        [Authorize("StandardUser")]
        public IActionResult Create()
        {
            DropdownsViewData();
            return View(new NewBin());
        }

        /// <summary>
        /// This action receives the form from the NewBin view, checks for validity, and inserts into the database.
        /// </summary>
        /// <param name="newBin"></param>
        /// <returns></returns>
        [HttpPost, Route("Create")]
        [Authorize("StandardUser")]
        public async Task<IActionResult> Create(NewBin newBin)
        {
            _log.LogDebug("Values of newBin: " + newBin);

            // Container object that will be added in the DB
            bool goodModel = await IsBinValid(newBin);

            Container container = new Container();

            if (!newBin.AutoGenerate)
            {
                if (newBin.BinNumber == null)
                {
                    ModelState.AddModelError("BinNumber", "Bin Number must be specified when not auto-generated.");
                    goodModel = false;
                }
                else if (newBin.BinNumber.Value < binNumberMin || newBin.BinNumber.Value > binNumberMax)
                {
                    ModelState.AddModelError("BinNumber", $"Bin Number must be between {binNumberMin} and {binNumberMax}.");
                    goodModel = false;
                }
                else // BinNumber is within range, good
                {
                    Container oldBin = await _context.Containers.AsNoTracking().FirstOrDefaultAsync(
                            c => c.BinNumber == newBin.BinNumber && !c.IsArchived);
                    if (oldBin != null) // A container with that BinNumber exists.
                    {
                        ModelState.AddModelError("BinNumber", "Error: This Bin Number is already in use.");
                        goodModel = false;
                    }
                    else
                    {
                        container.BinNumber = newBin.BinNumber.Value;
                    }
                }
            }
            // Going to insert if good:
            if (goodModel)
            {
                // Transaction used so another container doesn't get created with the same Bin Number while searching for one.
                using (var transaction = _context.Database.BeginTransaction())
                {
                    if (newBin.AutoGenerate)
                    {
                        var rand = new Random();
                        int binNum = 0;
                        int attempts = 0;
                        bool found = false;
                        while (attempts++ < 10)
                        {
                            binNum = rand.Next(binNumberMin, binNumberMax + 1);
                            if (!await _context.Containers.AsNoTracking().AnyAsync(c => c.BinNumber == binNum && !c.IsArchived))
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            // After 10 attempts at finding a random BinNumber, it didn't find one.
                            // Now we switch sequentially:
                            _log.LogInformation("After 10 attempts, a random BinNumber wasn't found");
                            List<int> usedBinNumbers = await _context.Containers.AsNoTracking()
                                .Where(c => !c.IsArchived)
                                .OrderBy(c => c.BinNumber)
                                .Select(c => c.BinNumber)
                                .ToListAsync();
                            int firstUnused = binNumberMin;
                            while (usedBinNumbers.Contains(firstUnused))
                            {
                                firstUnused++;
                            }
                            if (firstUnused > binNumberMax)
                            {
                                _log.LogError("Next available BinNumber is greater than binNumberMax.  Range exceeded.");
                            }
                            binNum = firstUnused;
                        }
                        container.BinNumber = binNum;
                    } // if not AutoGenerate, the Bin Number should have already been set.
                    container.SubcategoryID = newBin.SubcategoryID;
                    container.LocationID = newBin.LocationID;
                    container.Units = newBin.Units;
                    container.Weight = newBin.Weight;
                    container.ContainerNote = newBin.ContainerNote;
                    container.ExpirationDate = newBin.ExpirationDate;
                    _context.Containers.Add(container);
                    _context.SaveChanges(User.Identity.Name);
                    transaction.Commit();
                    _log.LogInformation($"User {User.Identity.Name} created a new container with ID {container.ContainerID}");
                    return RedirectToAction("Details", new { containerID = container.ContainerID });
                }
            }
            else
            {
                DropdownsViewData();
                return View(newBin);
            }
        }

        /// <summary>
        /// This private method is to fill the ViewData with the Categories and locations for view dropdowns
        /// </summary>
        /// <param name="subkey">String value for Categories key in ViewData[key]</param>
        /// <param name="lockey">String value for Locations key in ViewData[key]</param>
        private void DropdownsViewData(string catkey = "Categories", string lockey = "Locations")
        {
            var categories = _context.Categories
                .AsNoTracking()
                .Include(c => c.Subcategories)
                .Select(c => new
                {
                    CategoryID = c.CategoryID,
                    CategoryName = c.CategoryName,
                    Subcategories = c.Subcategories
                        .Where(s => !s.IsArchived)
                        .OrderBy(s => s.SubcategoryName)
                        .Select(s => new
                        {
                            SubcategoryID = s.SubcategoryID,
                            SubcategoryName = $"{s.SubcategoryName} {s.SubcategoryNote}"
                        }),
                })
                .OrderBy(c => c.CategoryName).ToList();

            // this is creating a JSON string that the view needs to put in a <script> tag
            // The items here need to be inserted with JavaScript to allow the checkbox to
            // change the available subcategories.
            ViewData[catkey] = JsonConvert.SerializeObject(categories);
            ViewData[lockey] = new SelectList(_context.Locations.AsNoTracking(), "LocationID", "LocationName");
        }

        /// <summary>
        /// This private method checks a bin for validity.  It modifies ModelState to add errors.
        /// </summary>
        /// <param name="bin">Bin to check.  Can be from actions Create() or Edit()</param>
        /// <returns>Returns a bool indicating true: bin is fine, false: bin is bad.</returns>
        private async Task<bool> IsBinValid(NewBin bin)
        {
            bool goodModel = true;

            // Checking for the Subcategory:
            Subcategory subcategory = await _context.Subcategories.AsNoTracking()
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.SubcategoryID == bin.SubcategoryID && !s.IsArchived);

            if (subcategory == null)
            {
                ModelState.AddModelError("SubcategoryID", "Subcategory doesn't exist.");
                goodModel = false;
            }

            // Checking for the Location:
            Location location = await _context.Locations.AsNoTracking()
                .FirstOrDefaultAsync(l => l.LocationID == bin.LocationID);
            if (location == null)
            {
                ModelState.AddModelError("LocationID", "Location doesn't exist.");
                goodModel = false;
            }

            // Units/Quantity/Cases and Weight are positive:
            if (bin.Units < 0)
            {
                ModelState.AddModelError("Cases", "Cases must be greater than or equal to 0.");
                goodModel = false;
            }

            if (bin.Weight < 0m)
            {
                ModelState.AddModelError("Weight", "Weight must be greater than or equal to 0.");
                goodModel = false;
            }
            return goodModel;
        }
    }
}
