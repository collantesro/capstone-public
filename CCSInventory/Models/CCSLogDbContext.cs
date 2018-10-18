using Microsoft.EntityFrameworkCore;

namespace CCSInventory.Models
{
    public class CCSLogDbContext : DbContext
    {
        public CCSLogDbContext(DbContextOptions<CCSLogDbContext> options) : base(options) { }
    }
}
