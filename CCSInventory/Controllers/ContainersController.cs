using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using CCSInventory.Models;
using CCSInventory.Models.ViewModels.Containers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.Extensions.Logging;

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

        public ContainersController(CCSDbContext context, ILogger<ContainersController> log)
        {
            _context = context;
            _log = log;
        }

        /// <summary>
        /// This action is the default index page.
        /// </summary>
        /// <returns></returns>
        [Route(""), Route("Index")]
        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// This action loads the view to display the specified container
        /// </summary>
        /// <param name="containerID">ContainerID of specific container</param>
        /// <returns></returns>
        [Route("ViewBin/{containerID}")]
        public async Task<IActionResult> ViewBin(int containerID)
        {
            Container container = await _context.Containers.AsNoTracking()
                .Include(c => c.Subcategory)
                    .ThenInclude(s => s.Category)
                .FirstOrDefaultAsync(c => c.ContainerID == containerID);
            if (container == null)
            {
                return NotFound();
            }
            return View(container);
        }

        /// <summary>
        /// This action is not yet implemented.  It will allow the user to edit a container.
        /// </summary>
        /// <param name="containerID"></param>
        /// <returns></returns>
        [Route("EditContainer/{containerID}")]
        [Authorize("StandardUser")]
        public async Task<IActionResult> EditContainer(int containerID)
        {
            return NotFound();
        }

        /// <summary>
        /// This action loads the view to create a new bin.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("NewBin")]
        [Authorize("StandardUser")]
        public IActionResult NewBin()
        {
            SubsToViewData();
            return View();
        }

        /// <summary>
        /// This action receives the form from the NewBin view, checks for validity, and inserts into the database.
        /// </summary>
        /// <param name="newBin"></param>
        /// <returns></returns>
        [HttpPost, Route("NewBin")]
        [Authorize("StandardUser")]
        public async Task<IActionResult> NewBin(NewBin newBin)
        {
            Container toInsert = new Container();
            if (newBin.AutoGenerate)
            {
                var rand = new Random();
                int bin = 0;
                do
                {
                    bin = rand.Next(1000, 9999);
                } while (
                    await _context.Containers.FirstOrDefaultAsync(
                        c => c.BinNumber == bin && !c.IsArchived) != null
                );
                toInsert.BinNumber = bin;
            }
            else
            {
                if (newBin.BinNumber == null)
                {
                    ModelState.AddModelError("BinNumber", "BinNumber must be specified when not auto-generated.");
                    SubsToViewData();
                    return View();
                }
                else if (newBin.BinNumber.Value < 1000 || newBin.BinNumber.Value > 9999)
                {
                    ModelState.AddModelError("BinNumber", "BinNumber must be between 1000 and 9999");
                    SubsToViewData();
                    return View();
                }

                Container oldBin = await _context.Containers.FirstOrDefaultAsync(
                        c => c.BinNumber == newBin.BinNumber && !c.IsArchived);
                if (oldBin != null)
                {
                    ModelState.AddModelError("BinNumber", "Error: This Bin Number is already in use.");
                    SubsToViewData();
                    return View();
                }
                else
                {
                    toInsert.BinNumber = newBin.BinNumber ?? 0;
                }
            }

            toInsert.Cases = newBin.Cases;
            toInsert.ContainerNote = newBin.ContainerNote;
            toInsert.SubcategoryID = newBin.SubcategoryID;
            toInsert.IsUSDA = newBin.IsUSDA;
            toInsert.Location = newBin.Location;
            toInsert.ExpirationDate = newBin.ExpirationDate;
            toInsert.Weight = newBin.Weight;
            Console.Error.Write(newBin);

            await _context.Containers.AddAsync(toInsert);
            await _context.SaveChangesAsync(User.Identity.Name);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// This private method is to fill the ViewData with the subcategories for dropdowns
        /// </summary>
        /// <param name="key">String value for key in ViewData[key]</param>
        private void SubsToViewData(string key = "Subcategories")
        {
            ViewData[key] = new SelectList(_context.Subcategories, "SubcategoryID", "SubcategoryName");
        }
    }
}
