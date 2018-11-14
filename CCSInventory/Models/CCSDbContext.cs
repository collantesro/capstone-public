using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CCSInventory.Models
{
    /// <summary>
    /// This class is the Entity Framework Core DbContext for the app's primary data.
    /// </summary>
    public class CCSDbContext : DbContext
    {
        // DbSets (pluralized names of the Entities):
        public DbSet<User> Users { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionLineItem> TransactionLineItems { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<PantryPackTransaction> PantryPackTransactions { get; set; }
        public DbSet<PantryPackType> PantryPackType { get; set; }

        public CCSDbContext(DbContextOptions<CCSDbContext> options) : base(options)
        {
        }

        // Annotations cannot mark certain things like Alternate Keys.
        /// <summary>
        /// This overridden method is used to define seed data and properties/relationships
        /// that do not have annotations.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*** Alternate Keys ***/
            // https://docs.microsoft.com/en-us/ef/core/modeling/alternate-keys
            modelBuilder.Entity<User>().HasAlternateKey(u => u.Username);
            modelBuilder.Entity<Agency>().HasAlternateKey(a => a.AgencyName);
            modelBuilder.Entity<Category>().HasAlternateKey(c => c.CategoryName);
            modelBuilder.Entity<Subcategory>().HasAlternateKey(s => s.SubcategoryName);
            modelBuilder.Entity<PantryPackType>().HasAlternateKey(t => t.PantryPackTypeName);
            modelBuilder.Entity<TransactionType>().HasAlternateKey(t => t.TransactionTypeName);

            /*** Indexes (Explicit) ***/
            modelBuilder.Entity<User>().HasIndex(u => u.Username);
            modelBuilder.Entity<Container>().HasIndex(u => u.BinNumber);
            modelBuilder.Entity<PantryPackType>().HasIndex(t => t.PantryPackTypeName);
            modelBuilder.Entity<TransactionType>().HasIndex(t => t.TransactionTypeName);

            /*** Seed Data ***/
            #region seeddata
            DateTime nov13 = DateTime.Parse("2018-11-13T20:40:00-07:00").ToUniversalTime();

            // Add a default user to the table.  Using an anonymous type instead of a User
            // object so the passwordHash and all other data is constant across migrations
            modelBuilder.Entity<User>().HasData(new
            {
                UserID = 1,
                FirstName = "Weber",
                LastName = "CS",
                Username = "skram",
                // This passwordhash is the hash for: M8/iq+W1
                PasswordHash = "$2a$10$/n.xV7jA5piJOZmfbT270eAKstycJ9WHqfpSttqz25ARWwnyLCyhu",
                UserRole = UserRole.ADMIN,
                UserNote = "Default user for an empty database",
                CreatedDate = DateTime.Parse("2018-10-18T12:30:18.051Z").ToUniversalTime(),
                CreatedBy = "Seeded Data",
                ModifiedDate = nov13,
                ModifiedBy = "Seeded Data",
            });

            // Default categories:
            modelBuilder.Entity<Category>().HasData(new Category[]{
                new Category {
                    CategoryID = 1,
                    CategoryName = "Dry Goods",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Category {
                    CategoryID = 2,
                    CategoryName = "Perishable",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Category {
                    CategoryID = 3,
                    CategoryName = "Non-Food",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Category {
                    CategoryID = 4,
                    CategoryName = "USDA",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Category {
                    CategoryID = 5,
                    CategoryName = "Grocery Rescue",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
            });
            // Default subcategories for above categories:
            modelBuilder.Entity<Subcategory>().HasData(new Subcategory[]{
                new Subcategory {
                    SubcategoryID = 1,
                    CategoryID = 1,
                    SubcategoryName = "Unsorted (Dry Goods)",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 2,
                    CategoryID = 2,
                    SubcategoryName = "Unsorted (Perishable)",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 3,
                    CategoryID = 3,
                    SubcategoryName = "Unsorted (Non-Food)",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 4,
                    CategoryID = 4,
                    SubcategoryName = "Unsorted (USDA)",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 5,
                    CategoryID = 4,
                    SubcategoryName = "Beans",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 6,
                    CategoryID = 4,
                    SubcategoryName = "Milk",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 7,
                    CategoryID = 4,
                    SubcategoryName = "Rice",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 8,
                    CategoryID = 4,
                    SubcategoryName = "Fruit",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 9,
                    CategoryID = 5,
                    SubcategoryName = "Bakery",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 10,
                    CategoryID = 5,
                    SubcategoryName = "Dairy",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 11,
                    CategoryID = 5,
                    SubcategoryName = "Produce",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 12,
                    CategoryID = 5,
                    SubcategoryName = "Deli",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 13,
                    CategoryID = 5,
                    SubcategoryName = "Meat",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 14,
                    CategoryID = 5,
                    SubcategoryName = "Frozen",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 15,
                    CategoryID = 5,
                    SubcategoryName = "Dry Grocery",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 16,
                    CategoryID = 5,
                    SubcategoryName = "Non-Food",
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data"
                },
            });

            modelBuilder.Entity<PantryPackType>().HasData(new PantryPackType
            {
                PantryPackTypeID = 1,
                PantryPackTypeName = "Generic",
                CreatedDate = nov13,
                CreatedBy = "Seeded Data",
                ModifiedDate = nov13,
                ModifiedBy = "Seeded Data"
            });

            modelBuilder.Entity<TransactionType>().HasData(new TransactionType[]{
                new TransactionType{
                    TransactionTypeID = 1,
                    TransactionTypeName = "In-Kind",
                    TransactionTypeNote = "Donations with where the items have not been taxed.",
                    IsOutgoing = false,
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data",
                },
                new TransactionType{
                    TransactionTypeID = 2,
                    TransactionTypeName = "Taxed",
                    TransactionTypeNote = "Donations with where the items have been taxed.",
                    IsOutgoing = false,
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data",
                },
                new TransactionType{
                    TransactionTypeID = 3,
                    TransactionTypeName = "USDA",
                    TransactionTypeNote = "Donations by the Utah Food Bank.",
                    IsOutgoing = false,
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data",
                },
                new TransactionType{
                    TransactionTypeID = 4,
                    TransactionTypeName = "Grocery Rescue",
                    TransactionTypeNote = "Donations from Grocery Rescue.",
                    IsOutgoing = false,
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data",
                },
                new TransactionType{
                    TransactionTypeID = 5,
                    TransactionTypeName = "Spoiled",
                    TransactionTypeNote = "Outgoing food due to spoilage.",
                    IsOutgoing = true,
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data",
                },
                new TransactionType{
                    TransactionTypeID = 6,
                    TransactionTypeName = "On-The-Line",
                    TransactionTypeNote = "Outgoing items to the pantry.",
                    IsOutgoing = true,
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data",
                },
                new TransactionType{
                    TransactionTypeID = 7,
                    TransactionTypeName = "Pantry Pack",
                    TransactionTypeNote = "Outgoing items for pantry packs.",
                    IsOutgoing = true,
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data",
                },
                new TransactionType{
                    TransactionTypeID = 8,
                    TransactionTypeName = "Organization Transfer",
                    TransactionTypeNote = "Outgoing items to other organizations.",
                    IsOutgoing = true,
                    CreatedDate = nov13,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov13,
                    ModifiedBy = "Seeded Data",
                },
            });

            modelBuilder.Entity<Address>().HasData(new Address{
                AddressID = 1,
                StreetAddress1 = "2504 F Ave",
                City = "Ogden",
                State = "UT",
                Zip = "84401",
                AddressNote = "Catholic Community Services Ogden",
                CreatedDate = nov13,
                CreatedBy = "Seeded Data",
                ModifiedDate = nov13,
                ModifiedBy = "Seeded Data"
            });

            modelBuilder.Entity<Agency>().HasData(new Agency{
                AgencyID = 1,
                AgencyName = "Catholic Community Services Ogden",
                AddressID = 1,
                MailingAddressID = 1,
                PhoneNumber = "+18013945944",
                AgencyNote = null,
                IsArchived = false,
                CreatedDate = nov13,
                CreatedBy = "Seeded Data",
                ModifiedDate = nov13,
                ModifiedBy = "Seeded Data"
            });


            #region demo_data
            modelBuilder.Entity<Address>().HasData(new Address
            {
                AddressID = 2,
                StreetAddress1 = "123 Memory Lane",
                City = "Ogden",
                State = "UT",
                Zip = "84401",
                AddressNote = "Walmart",
                CreatedDate = nov13,
                CreatedBy = "Seeded Data",
                ModifiedDate = nov13,
                ModifiedBy = "Seeded Data"
            });

            modelBuilder.Entity<Agency>().HasData(new Agency
            {
                AgencyID = 2,
                AgencyName = "Walmart",
                AddressID = 2,
                MailingAddressID = 2,
                PhoneNumber = "+18013551243",
                AgencyNote = null,
                IsArchived = false,
                CreatedDate = nov13,
                CreatedBy = "Seeded Data",
                ModifiedDate = nov13,
                ModifiedBy = "Seeded Data"
            });


            modelBuilder.Entity<Address>().HasData(new Address
            {
                AddressID = 3,
                StreetAddress1 = "543 n 250 e ",
                City = "Ogden",
                State = "UT",
                Zip = "84401",
                AddressNote = "Smiths",
                CreatedDate = nov13,
                CreatedBy = "Seeded Data",
                ModifiedDate = nov13,
                ModifiedBy = "Seeded Data"
            });

            modelBuilder.Entity<Agency>().HasData(new Agency
            {
                AgencyID = 3,
                AgencyName = "Smiths",
                AddressID = 3,
                MailingAddressID = 3,
                PhoneNumber = "+18017231598",
                AgencyNote = null,
                IsArchived = false,
                CreatedDate = nov13,
                CreatedBy = "Seeded Data",
                ModifiedDate = nov13,
                ModifiedBy = "Seeded Data"
            });


            modelBuilder.Entity<Address>().HasData(new Address
            {
                AddressID = 4,
                StreetAddress1 = "635 s 400 w",
                City = "Ogden",
                State = "UT",
                Zip = "84401",
                AddressNote = "Mark",
                CreatedDate = nov13,
                CreatedBy = "Seeded Data",
                ModifiedDate = nov13,
                ModifiedBy = "Seeded Data"
            });

            modelBuilder.Entity<Agency>().HasData(new Agency
            {
                AgencyID = 4,
                AgencyName = "Mark",
                AddressID = 4,
                MailingAddressID = 4,
                PhoneNumber = "+18013665201",
                AgencyNote = null,
                IsArchived = false,
                CreatedDate = nov13,
                CreatedBy = "Seeded Data",
                ModifiedDate = nov13,
                ModifiedBy = "Seeded Data"
            });


            modelBuilder.Entity<Address>().HasData(new Address
            {
                AddressID = 5,
                StreetAddress1 = "4567 washington blvd",
                City = "Ogden",
                State = "UT",
                Zip = "84401",
                AddressNote = "Kelly Vasquez",
                CreatedDate = nov13,
                CreatedBy = "Seeded Data",
                ModifiedDate = nov13,
                ModifiedBy = "Seeded Data"
            });

            modelBuilder.Entity<Agency>().HasData(new Agency
            {
                AgencyID = 5,
                AgencyName = "Kelly Vasquez",
                AddressID = 5,
                MailingAddressID = 5,
                PhoneNumber = "+180139918034",
                AgencyNote = null,
                IsArchived = false,
                CreatedDate = nov13,
                CreatedBy = "Seeded Data",
                ModifiedDate = nov13,
                ModifiedBy = "Seeded Data"
            });
            #endregion
            #endregion
        }

        /// <summary>
        /// This overridden method updates the Created and Modified timestamps before calling the
        /// base class' SaveChanges() method.  See DbContext.SaveChanges()
        /// </summary>
        /// <param name="username">Username to fill for Created/Modified fields of TrackedModel</param>
        /// <returns></returns>
        public int SaveChanges(string username = null)
        {
            UpdateModifiedTimestamp(username);
            return base.SaveChanges();
        }

        /// <summary>
        /// This overridden method updates the Created and Modified timestamps before calling
        /// the base class' SaveChangesAsync() method.  See DbContext.SaveChangesAsync()
        /// </summary>
        /// <param name="username">Username to fill for Created/Modified fields of TrackedModel</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> SaveChangesAsync(string username = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateModifiedTimestamp(username);
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// This method updates the Created and Modified timestamps for Entities extending from
        /// TrackedModel that are about to be saved.
        /// </summary>
        /// <param name="username">Username to fill for Created/Modified fields of TrackedModel</param>
        private void UpdateModifiedTimestamp(string username = null)
        {
            var entities = ChangeTracker.Entries().Where(
                x => x.Entity is TrackedModel && (x.State == EntityState.Added || x.State == EntityState.Modified)
            );

            username = string.IsNullOrWhiteSpace(username) ? "Unspecified" : username;

            foreach (var e in entities)
            {
                if (e.State == EntityState.Added)
                {
                    ((TrackedModel)e.Entity).CreatedDate = DateTime.UtcNow;
                    ((TrackedModel)e.Entity).CreatedBy = username;
                }
                ((TrackedModel)e.Entity).ModifiedDate = DateTime.UtcNow;
                ((TrackedModel)e.Entity).ModifiedBy = username;

            }
        }
    }
}
