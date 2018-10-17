using Microsoft.EntityFrameworkCore;

namespace CCSInventory.Models {
    public class CCSLogDbContext : DbContext {
        public CCSLogDbContext(DbContextOptions<CCSLogDbContext> o) : base(o) {}
        
    }
}
