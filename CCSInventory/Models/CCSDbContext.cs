using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CCSInventory.Models {
    public class CCSDbContext : DbContext {
        public CCSDbContext(DbContextOptions<CCSDbContext> options) : base(options){
            
        }

        public override int SaveChanges(){
            UpdateModifiedTimestamp();
            return base.SaveChanges();
        }
        private void UpdateModifiedTimestamp(){
            var entities = ChangeTracker.Entries().Where(
                x => x.Entity is TrackedModel && (x.State == EntityState.Added || x.State == EntityState.Modified)
            );

            foreach(var e in entities){
                if (e.State == EntityState.Added){
                    ((TrackedModel)e.Entity).Created = DateTime.UtcNow;
                }
                ((TrackedModel)e.Entity).Modified = DateTime.UtcNow;
            }
        }
        public DbSet<User> Users {get; set;}
    }
}
