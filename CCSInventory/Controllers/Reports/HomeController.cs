using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CCSInventory.Controllers.Reports
{
    [Authorize("ReadonlyUser")]
    [Route("Reports/Home")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Returns the homepage
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("Index"), Route("")]
        public IActionResult Index()
        {
            return View("Views/Reports/Home/Index.cshtml");
        }
    }
}
