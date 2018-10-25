using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CCSInventory.Models;

namespace CCSInventory.Controllers
{

    /// <summary>
    /// This web api controller is for managing users through a webapi.  It's incomplete, however.
    /// </summary>
    [Route("api/User/[action]")]
    [Authorize("AdminUser")]
    public class UserApiController : ControllerBase
    {
        private readonly CCSDbContext _context;
        private readonly ILogger<UserApiController> _log;

        public UserApiController(CCSDbContext context, ILogger<UserApiController> logger)
        {
            _context = context;
            _log = logger;
        }

        //TODO: Use a ViewModel or something to hide the PasswordHash and the FullName* properties
        [HttpGet]
        public async Task<List<User>> AllUsers()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }
    }
}
