using Microsoft.EntityFrameworkCore;

namespace CCSInventory.Models
{
    /// <summary>
    /// (Currently unused) This DbContext is for logs of actions performed by users.
    /// </summary>
    public class CCSLogDbContext : DbContext
    {
        public CCSLogDbContext(DbContextOptions<CCSLogDbContext> options) : base(options) { }
    }
}
