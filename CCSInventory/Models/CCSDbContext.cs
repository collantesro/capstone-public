using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CCSInventory.Models;

namespace CCSInventory.Models
{
    /// <summary>
    /// This class is the Entity Framework Core DbContext for the app's primary data.
    /// </summary>
    public class CCSDbContext : DbContext
    {
        // DbSets (pluralized names of the Entities):
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionLineItem> TransactionLineItems { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<User> Users { get; set; }

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
            /*** Relationships ***/
            // Making it explicit, but EFCore should be able to guess it regardless
            modelBuilder.Entity<TransactionLineItem>()
                .HasOne(li => li.Transaction)
                .WithMany(t => t.LineItems);

            /*** Alternate Keys ***/
            // https://docs.microsoft.com/en-us/ef/core/modeling/alternate-keys
            modelBuilder.Entity<Agency>().HasAlternateKey(a => a.AgencyName);
            modelBuilder.Entity<Category>().HasAlternateKey(c => c.CategoryName);
            modelBuilder.Entity<Location>().HasAlternateKey(l => l.LocationName);
            modelBuilder.Entity<Subcategory>().HasAlternateKey(s => s.SubcategoryName);
            modelBuilder.Entity<TransactionType>().HasAlternateKey(t => t.TransactionTypeName);
            modelBuilder.Entity<User>().HasAlternateKey(u => u.Username);

            /*** Indexes (Explicit) ***/
            modelBuilder.Entity<Container>().HasIndex(u => u.BinNumber);
            modelBuilder.Entity<Location>().HasIndex(l => l.LocationName);
            modelBuilder.Entity<TransactionType>().HasIndex(t => t.TransactionTypeName);
            modelBuilder.Entity<User>().HasIndex(u => u.Username);

            /*** Seed Data ***/
            #region seeddata
            DateTime nov28 = DateTime.Parse("2018-11-28T09:38:00-07:00").ToUniversalTime();
            DateTime dec04 = DateTime.Parse("2018-12-04T21:27:00-07:00").ToUniversalTime();
            DateTime dec11 = DateTime.Parse("2018-12-11T20:00:00-07:00").ToUniversalTime();

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
                ModifiedDate = nov28,
                ModifiedBy = "Seeded Data",
            });

            // Default categories:
            modelBuilder.Entity<Category>().HasData(new Category[]{
                new Category {
                    CategoryID = 1,
                    CategoryName = "Dry Goods",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Category {
                    CategoryID = 2,
                    CategoryName = "Perishable",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Category {
                    CategoryID = 3,
                    CategoryName = "Non-Food",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Category {
                    CategoryID = 4,
                    CategoryName = "USDA",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Category {
                    CategoryID = 5,
                    CategoryName = "Grocery Rescue",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Category {
                    CategoryID = 6,
                    CategoryName = "Pantry Pack",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                }
            });
            // Default subcategories for above categories:
            modelBuilder.Entity<Subcategory>().HasData(new Subcategory[]{
                new Subcategory {
                    SubcategoryID = 1,
                    CategoryID = 1,
                    SubcategoryName = "Unsorted (Dry Goods)",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 2,
                    CategoryID = 2,
                    SubcategoryName = "Unsorted (Perishable)",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 3,
                    CategoryID = 3,
                    SubcategoryName = "Unsorted (Non-Food)",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 4,
                    CategoryID = 4,
                    SubcategoryName = "Unsorted (USDA)",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 9,
                    CategoryID = 5,
                    SubcategoryName = "Bakery",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 10,
                    CategoryID = 5,
                    SubcategoryName = "Dairy",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 11,
                    CategoryID = 5,
                    SubcategoryName = "Produce",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 12,
                    CategoryID = 5,
                    SubcategoryName = "Deli",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 13,
                    CategoryID = 5,
                    SubcategoryName = "Meat",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 14,
                    CategoryID = 5,
                    SubcategoryName = "Frozen",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 15,
                    CategoryID = 5,
                    SubcategoryName = "Dry Grocery",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 16,
                    CategoryID = 5,
                    SubcategoryName = "Non-Food",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 17,
                    CategoryID = 6,
                    SubcategoryName = "Pantry Pack",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                #region USDANumbers
                new Subcategory {
                    SubcategoryID = 18,
                    CategoryID = 4,
                    SubcategoryName = "110361",
                    SubcategoryNote = "Applesauce",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 19,
                    CategoryID = 4,
                    SubcategoryName = "100210",
                    SubcategoryNote = "Apricot Halves",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 20,
                    CategoryID = 4,
                    SubcategoryName = "100363",
                    SubcategoryNote = "Beans(vegetarian)",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 21,
                    CategoryID = 4,
                    SubcategoryName = "100435",
                    SubcategoryNote = "Whole Grain Rotini",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 22,
                    CategoryID = 4,
                    SubcategoryName = "100321",
                    SubcategoryNote = "Vegetable Soup",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 23,
                    CategoryID = 4,
                    SubcategoryName = "100328",
                    SubcategoryNote = "Tomatoes, diced",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 24,
                    CategoryID = 4,
                    SubcategoryName = "100333",
                    SubcategoryNote = "Tomato Sauce",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 25,
                    CategoryID = 4,
                    SubcategoryName = "100322",
                    SubcategoryNote = "Tomato Soup",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 26,
                    CategoryID = 4,
                    SubcategoryName = "100316",
                    SubcategoryNote = "Sweet Potatoes",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 27,
                    CategoryID = 4,
                    SubcategoryName = "100323",
                    SubcategoryNote = "Spinach",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 28,
                    CategoryID = 4,
                    SubcategoryName = "100335",
                    SubcategoryNote = "Spaghetti Sauce",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 29,
                    CategoryID = 4,
                    SubcategoryName = "100426",
                    SubcategoryNote = "Spaghetti, Noodles",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 30,
                    CategoryID = 4,
                    SubcategoryName = "100050",
                    SubcategoryNote = "Shelf Milk 1%",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 31,
                    CategoryID = 4,
                    SubcategoryName = "100526",
                    SubcategoryNote = "Beef stew",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 32,
                    CategoryID = 4,
                    SubcategoryName = "100491",
                    SubcategoryNote = "Rice, Long Grain",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 33,
                    CategoryID = 4,
                    SubcategoryName = "100293",
                    SubcategoryNote = "Raisins",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 34,
                    CategoryID = 4,
                    SubcategoryName = "100319",
                    SubcategoryNote = "Pumpkin",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 35,
                    CategoryID = 4,
                    SubcategoryName = "100139",
                    SubcategoryNote = "Pork",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 36,
                    CategoryID = 4,
                    SubcategoryName = "100290",
                    SubcategoryNote = "Plums Pitted Dried",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 37,
                    CategoryID = 4,
                    SubcategoryName = "110178",
                    SubcategoryNote = "Pistachios",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 38,
                    CategoryID = 4,
                    SubcategoryName = "110021",
                    SubcategoryNote = "Pinto Beans",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 39,
                    CategoryID = 4,
                    SubcategoryName = "100223",
                    SubcategoryNote = "Pears",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 40,
                    CategoryID = 4,
                    SubcategoryName = "100395",
                    SubcategoryNote = "Peanut Butter",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 41,
                    CategoryID = 4,
                    SubcategoryName = "100218",
                    SubcategoryNote = "Peaches # 300",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 42,
                    CategoryID = 4,
                    SubcategoryName = "100219",
                    SubcategoryNote = "Peaches",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 43,
                    CategoryID = 4,
                    SubcategoryName = "100314",
                    SubcategoryNote = "Peas",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 44,
                    CategoryID = 4,
                    SubcategoryName = "100897",
                    SubcategoryNote = "Orange Juice",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 45,
                    CategoryID = 4,
                    SubcategoryName = "100211",
                    SubcategoryNote = "Mixed Fruit",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 46,
                    CategoryID = 4,
                    SubcategoryName = "100306",
                    SubcategoryNote = "Green Beans",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 47,
                    CategoryID = 4,
                    SubcategoryName = "100896",
                    SubcategoryNote = "Grapefruit Juice",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 48,
                    CategoryID = 4,
                    SubcategoryName = "100589",
                    SubcategoryNote = "Fig Pieces",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 49,
                    CategoryID = 4,
                    SubcategoryName = "100433",
                    SubcategoryNote = "Egg Noodles",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 50,
                    CategoryID = 4,
                    SubcategoryName = "100213",
                    SubcategoryNote = "Cranberry Sauce",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 51,
                    CategoryID = 4,
                    SubcategoryName = "100899",
                    SubcategoryNote = "Cranberry Apple Juice",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 52,
                    CategoryID = 4,
                    SubcategoryName = "100311",
                    SubcategoryNote = "Corn Kernel",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 53,
                    CategoryID = 4,
                    SubcategoryName = "100308",
                    SubcategoryNote = "Carrots",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 54,
                    CategoryID = 4,
                    SubcategoryName = "100372",
                    SubcategoryNote = "Red Kidney Beans",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 55,
                    CategoryID = 4,
                    SubcategoryName = "100287",
                    SubcategoryNote = "Dates",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 56,
                    CategoryID = 4,
                    SubcategoryName = "100182",
                    SubcategoryNote = "Ham, water added",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 57,
                    CategoryID = 4,
                    SubcategoryName = "100145",
                    SubcategoryNote = "Pork Patties",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 58,
                    CategoryID = 4,
                    SubcategoryName = "110301",
                    SubcategoryNote = "Catfish Fillets",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 59,
                    CategoryID = 4,
                    SubcategoryName = "110092",
                    SubcategoryNote = "Chix Leg Quarters",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 60,
                    CategoryID = 4,
                    SubcategoryName = "100241",
                    SubcategoryNote = "peach cups",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 61,
                    CategoryID = 4,
                    SubcategoryName = "110094",
                    SubcategoryNote = "chicken legs",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 62,
                    CategoryID = 4,
                    SubcategoryName = "110332",
                    SubcategoryNote = "USDA Turkey Whole Bagged",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 63,
                    CategoryID = 4,
                    SubcategoryName = "100898",
                    SubcategoryNote = "Tomato Juice",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 64,
                    CategoryID = 4,
                    SubcategoryName = "100337",
                    SubcategoryNote = "Dehydrated Potatoes",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 65,
                    CategoryID = 4,
                    SubcategoryName = "100277",
                    SubcategoryNote = "Orange Juice singles",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 66,
                    CategoryID = 4,
                    SubcategoryName = "100263",
                    SubcategoryNote = "Blueberry F Cult",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 67,
                    CategoryID = 4,
                    SubcategoryName = "100310",
                    SubcategoryNote = "Corn Cream 24",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 68,
                    CategoryID = 4,
                    SubcategoryName = "100105",
                    SubcategoryNote = "Chicken, Leg Qrts 40 lb",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 69,
                    CategoryID = 4,
                    SubcategoryName = "100297",
                    SubcategoryNote = "Fruit and Nut Mix",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 70,
                    CategoryID = 4,
                    SubcategoryName = "100367",
                    SubcategoryNote = "Beans, Blackeye",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 71,
                    CategoryID = 4,
                    SubcategoryName = "100242",
                    SubcategoryNote = "Blueberries, Frozen",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 72,
                    CategoryID = 4,
                    SubcategoryName = "100375",
                    SubcategoryNote = "Red Beans",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 73,
                    CategoryID = 4,
                    SubcategoryName = "100342",
                    SubcategoryNote = "Tomatoes-perishable",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 74,
                    CategoryID = 4,
                    SubcategoryName = "100324",
                    SubcategoryNote = "Diced tomatoes",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 75,
                    CategoryID = 4,
                    SubcategoryName = "101017",
                    SubcategoryNote = "Fresh Potatoes-perishable",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 76,
                    CategoryID = 4,
                    SubcategoryName = "100125",
                    SubcategoryNote = "Turkey Roasts-perishable",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 77,
                    CategoryID = 4,
                    SubcategoryName = "110290",
                    SubcategoryNote = "Lamb Leg Roast",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 78,
                    CategoryID = 4,
                    SubcategoryName = "100382",
                    SubcategoryNote = "Beans, pinto 2lb. bags",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 79,
                    CategoryID = 4,
                    SubcategoryName = "100356",
                    SubcategoryNote = "Potatoes, Wedges",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 80,
                    CategoryID = 4,
                    SubcategoryName = "101020",
                    SubcategoryNote = "Garbanzo Beans",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 81,
                    CategoryID = 4,
                    SubcategoryName = "100361",
                    SubcategoryNote = "Refried Beans",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 82,
                    CategoryID = 4,
                    SubcategoryName = "100275",
                    SubcategoryNote = "Cranberry Juice Concentrate",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 83,
                    CategoryID = 4,
                    SubcategoryName = "100256",
                    SubcategoryNote = "Strawberry Frozen Cup",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 84,
                    CategoryID = 4,
                    SubcategoryName = "100895",
                    SubcategoryNote = "Grape Juice",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 85,
                    CategoryID = 4,
                    SubcategoryName = "110163",
                    SubcategoryNote = "Cream of Chicken Soup",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 86,
                    CategoryID = 4,
                    SubcategoryName = "110440",
                    SubcategoryNote = "Turkey Sliced Frozen",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 87,
                    CategoryID = 4,
                    SubcategoryName = "110431",
                    SubcategoryNote = "Turkey Breast Frozen",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 88,
                    CategoryID = 4,
                    SubcategoryName = "100331",
                    SubcategoryNote = "Potatoes, sliced",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 89,
                    CategoryID = 4,
                    SubcategoryName = "110164",
                    SubcategoryNote = "Cream of Mushroom Soup",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 90,
                    CategoryID = 4,
                    SubcategoryName = "100295",
                    SubcategoryNote = "Raisins",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 91,
                    CategoryID = 4,
                    SubcategoryName = "100380",
                    SubcategoryNote = "Beans Great North'",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 92,
                    CategoryID = 4,
                    SubcategoryName = "100880",
                    SubcategoryNote = "Chicken, Whole, Bagged",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 93,
                    CategoryID = 4,
                    SubcategoryName = "100123",
                    SubcategoryNote = "Turkey, Consumer Whole",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 94,
                    CategoryID = 4,
                    SubcategoryName = "100320",
                    SubcategoryNote = "Mix Veg",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 95,
                    CategoryID = 4,
                    SubcategoryName = "100894",
                    SubcategoryNote = "Cherry Apple Juice",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 96,
                    CategoryID = 4,
                    SubcategoryName = "100893",
                    SubcategoryNote = "Apple Juice",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 97,
                    CategoryID = 4,
                    SubcategoryName = "100198",
                    SubcategoryNote = "Salmon, canned",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 98,
                    CategoryID = 4,
                    SubcategoryName = "100300",
                    SubcategoryNote = "Dried Cranberries",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 99,
                    CategoryID = 4,
                    SubcategoryName = "100298",
                    SubcategoryNote = "Cherries, dried",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 100,
                    CategoryID = 4,
                    SubcategoryName = "100207",
                    SubcategoryNote = "Applesauce #300",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 101,
                    CategoryID = 4,
                    SubcategoryName = "100315",
                    SubcategoryNote = "Sweet Peas",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 102,
                    CategoryID = 4,
                    SubcategoryName = "100466",
                    SubcategoryNote = "Oats",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 103,
                    CategoryID = 4,
                    SubcategoryName = "100313",
                    SubcategoryNote = "Corn, whole kernel",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 104,
                    CategoryID = 4,
                    SubcategoryName = "110470",
                    SubcategoryNote = "Frozen Sliced Apples",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 105,
                    CategoryID = 4,
                    SubcategoryName = "100469",
                    SubcategoryNote = "Grits",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 106,
                    CategoryID = 4,
                    SubcategoryName = "100929",
                    SubcategoryNote = "Oat Circles",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 107,
                    CategoryID = 4,
                    SubcategoryName = "110260",
                    SubcategoryNote = "Beef Fine Ground",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 108,
                    CategoryID = 4,
                    SubcategoryName = "100441",
                    SubcategoryNote = "Vegetable Oil",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 109,
                    CategoryID = 4,
                    SubcategoryName = "110481",
                    SubcategoryNote = "Frozen Carrots",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 110,
                    CategoryID = 4,
                    SubcategoryName = "110450",
                    SubcategoryNote = "Pasta Spaghetti",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 111,
                    CategoryID = 4,
                    SubcategoryName = "100918",
                    SubcategoryNote = "Bakery flour mix",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 112,
                    CategoryID = 4,
                    SubcategoryName = "100514",
                    SubcategoryNote = "Red Apples",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 113,
                    CategoryID = 4,
                    SubcategoryName = "100236",
                    SubcategoryNote = "Cherries, rtp",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 114,
                    CategoryID = 4,
                    SubcategoryName = "100512",
                    SubcategoryNote = "Apples Granny Smith",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 115,
                    CategoryID = 4,
                    SubcategoryName = "110652",
                    SubcategoryNote = "Salmom canned",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 116,
                    CategoryID = 4,
                    SubcategoryName = "100521",
                    SubcategoryNote = "Gala Apples Fresh",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 117,
                    CategoryID = 4,
                    SubcategoryName = "110560",
                    SubcategoryNote = "Pears, Fresh",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 118,
                    CategoryID = 4,
                    SubcategoryName = "100523",
                    SubcategoryNote = "Apples Braeburn",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 119,
                    CategoryID = 4,
                    SubcategoryName = "110672",
                    SubcategoryNote = "Lamb Shoulder Roast",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 120,
                    CategoryID = 4,
                    SubcategoryName = "110651",
                    SubcategoryNote = "Orange Juice",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 121,
                    CategoryID = 4,
                    SubcategoryName = "110556",
                    SubcategoryNote = "Raisins unsweetened",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 122,
                    CategoryID = 4,
                    SubcategoryName = "100908",
                    SubcategoryNote = "Walnut Pieces",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 123,
                    CategoryID = 4,
                    SubcategoryName = "110390",
                    SubcategoryNote = "Catfish fillets, Unbreaded",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 124,
                    CategoryID = 4,
                    SubcategoryName = "100227",
                    SubcategoryNote = "Canned Cherries",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 125,
                    CategoryID = 4,
                    SubcategoryName = "110511",
                    SubcategoryNote = "Elbow Macaroni",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 126,
                    CategoryID = 4,
                    SubcategoryName = "100046",
                    SubcategoryNote = "Whole Eggs Frozen",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 127,
                    CategoryID = 4,
                    SubcategoryName = "100194",
                    SubcategoryNote = "Tuna Canned",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 128,
                    CategoryID = 4,
                    SubcategoryName = "100936",
                    SubcategoryNote = "Eggs",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 129,
                    CategoryID = 4,
                    SubcategoryName = "110623",
                    SubcategoryNote = "Blueberries, High Bush",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 130,
                    CategoryID = 4,
                    SubcategoryName = "110580",
                    SubcategoryNote = "Kosher Salmon, Canned",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 131,
                    CategoryID = 4,
                    SubcategoryName = "110845",
                    SubcategoryNote = "Whole Eggs Frozen",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 132,
                    CategoryID = 4,
                    SubcategoryName = "101035",
                    SubcategoryNote = "Whole Grain Spaghetti",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 133,
                    CategoryID = 4,
                    SubcategoryName = "110478",
                    SubcategoryNote = "Boned chicken canned",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 134,
                    CategoryID = 4,
                    SubcategoryName = "100003",
                    SubcategoryNote = "Shredded Yellow Cheddar",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 135,
                    CategoryID = 4,
                    SubcategoryName = "110741",
                    SubcategoryNote = "Whole Wheat Tortillas",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 136,
                    CategoryID = 4,
                    SubcategoryName = "100487",
                    SubcategoryNote = "Rice, Medium",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 137,
                    CategoryID = 4,
                    SubcategoryName = "110610",
                    SubcategoryNote = "KH Tomato Sauce",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 138,
                    CategoryID = 4,
                    SubcategoryName = "101024",
                    SubcategoryNote = "Macaroni and cheese",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 139,
                    CategoryID = 4,
                    SubcategoryName = "100233",
                    SubcategoryNote = "Plums",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 140,
                    CategoryID = 4,
                    SubcategoryName = "110561",
                    SubcategoryNote = "Apples Fresh",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 141,
                    CategoryID = 4,
                    SubcategoryName = "100492",
                    SubcategoryNote = "Rice, Long (heavy bag)",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 142,
                    CategoryID = 4,
                    SubcategoryName = "110777",
                    SubcategoryNote = "Whole Grain Pasta Rotini ",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 143,
                    CategoryID = 4,
                    SubcategoryName = "100384",
                    SubcategoryNote = "Dark Red Kidney Beans",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 144,
                    CategoryID = 4,
                    SubcategoryName = "110345",
                    SubcategoryNote = "Fish AK Plck Fillets",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 145,
                    CategoryID = 4,
                    SubcategoryName = "110903",
                    SubcategoryNote = "Turkey Breast Deli Sliced",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 146,
                    CategoryID = 4,
                    SubcategoryName = "110154",
                    SubcategoryNote = "Chicken Consumer Split Breast",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 147,
                    CategoryID = 4,
                    SubcategoryName = "100289",
                    SubcategoryNote = "Fig Pieces 24 1lb",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 148,
                    CategoryID = 4,
                    SubcategoryName = "110904",
                    SubcategoryNote = "Turkey Breast Smoked Sliced",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 149,
                    CategoryID = 4,
                    SubcategoryName = "110990",
                    SubcategoryNote = "Potatoes Red Fresh",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 150,
                    CategoryID = 4,
                    SubcategoryName = "110980",
                    SubcategoryNote = "Pork Pulled CKD",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 151,
                    CategoryID = 4,
                    SubcategoryName = "110880",
                    SubcategoryNote = "Farina Wheat Cereal",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 152,
                    CategoryID = 4,
                    SubcategoryName = "110380",
                    SubcategoryNote = "Pork Chops Boneless",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 153,
                    CategoryID = 4,
                    SubcategoryName = "100376",
                    SubcategoryNote = "Split Pea Dry",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 154,
                    CategoryID = 4,
                    SubcategoryName = "110843",
                    SubcategoryNote = "Cheese Ched Yei Shred",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                },
                new Subcategory {
                    SubcategoryID = 155,
                    CategoryID = 4,
                    SubcategoryName = "111008",
                    SubcategoryNote = "1% Milk Fresh",
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data"
                }
                #endregion
            });

            modelBuilder.Entity<TransactionType>().HasData(new TransactionType[]{
                new TransactionType{
                    TransactionTypeID = 1,
                    TransactionTypeName = "In-Kind",
                    TransactionTypeNote = "Donations with where the items have not been taxed.",
                    IsOutgoing = false,
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data",
                },
                new TransactionType{
                    TransactionTypeID = 2,
                    TransactionTypeName = "Taxed",
                    TransactionTypeNote = "Donations with where the items have been taxed.",
                    IsOutgoing = false,
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data",
                },
                new TransactionType{
                    TransactionTypeID = 3,
                    TransactionTypeName = "USDA",
                    TransactionTypeNote = "Donations by the Utah Food Bank.",
                    IsOutgoing = false,
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data",
                },
                new TransactionType{
                    TransactionTypeID = 4,
                    TransactionTypeName = "Grocery Rescue",
                    TransactionTypeNote = "Donations from Grocery Rescue.",
                    IsOutgoing = false,
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data",
                },
                new TransactionType{
                    TransactionTypeID = 5,
                    TransactionTypeName = "Spoiled",
                    TransactionTypeNote = "Outgoing food due to spoilage.",
                    IsOutgoing = true,
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data",
                },
                new TransactionType{
                    TransactionTypeID = 6,
                    TransactionTypeName = "On-The-Line",
                    TransactionTypeNote = "Outgoing items to the pantry.",
                    IsOutgoing = true,
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data",
                },
                new TransactionType{
                    TransactionTypeID = 7,
                    TransactionTypeName = "Organization Transfer",
                    TransactionTypeNote = "Outgoing items to other organizations.",
                    IsOutgoing = true,
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data",
                },
                 new TransactionType{
                    TransactionTypeID = 8,
                    TransactionTypeName = "Non-Taxable",
                    TransactionTypeNote = "Donations with where the items are not taxed",
                    IsOutgoing = false,
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = nov28,
                    ModifiedBy = "Seeded Data",
                },
            });

            modelBuilder.Entity<Address>().HasData(new Address
            {
                AddressID = 1,
                StreetAddress1 = "2504 F Ave",
                City = "Ogden",
                State = "UT",
                Zip = "84401",
                AddressNote = "Catholic Community Services Ogden",
                CreatedDate = nov28,
                CreatedBy = "Seeded Data",
                ModifiedDate = nov28,
                ModifiedBy = "Seeded Data"
            });

            modelBuilder.Entity<Agency>().HasData(new Agency[]
            {
                new Agency{
                    AgencyID = 1,
                    AgencyName = "Catholic Community Services Ogden",
                    AddressID = 1,
                    PhoneNumber = "+18013945944",
                    AgencyNote = null,
                    IsArchived = false,
                    HasAddress = true,
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = dec11,
                    ModifiedBy = "Seeded Data"
                },
                new Agency{
                    AgencyID = 2,
                    AgencyName = "Anonymous",
                    HasAddress = false,
                    AddressID = null,
                    PhoneNumber = "+10000000000",
                    AgencyNote = "This is a catch-all Agency for anonymous donations.",
                    IsArchived = false,
                    CreatedDate = nov28,
                    CreatedBy = "Seeded Data",
                    ModifiedDate = dec11,
                    ModifiedBy = "Seeded Data"
                },
            });

            modelBuilder.Entity<Location>().HasData(new Location[] {
                new Location {
                    LocationID = 1,
                    LocationName = "East Room",
                    LocationNote = null,
                    CreatedDate = dec04,
                    CreatedBy = "Seeded Data",
                    ModifiedBy = "Seeded Data",
                    ModifiedDate = dec04,
                },
                new Location {
                    LocationID = 2,
                    LocationName = "Main Room",
                    LocationNote = null,
                    CreatedDate = dec04,
                    CreatedBy = "Seeded Data",
                    ModifiedBy = "Seeded Data",
                    ModifiedDate = dec04,
                },
                new Location {
                    LocationID = 3,
                    LocationName = "South Room",
                    LocationNote = null,
                    CreatedDate = dec04,
                    CreatedBy = "Seeded Data",
                    ModifiedBy = "Seeded Data",
                    ModifiedDate = dec04,
                },
                new Location {
                    LocationID = 4,
                    LocationName = "(NONE)",
                    LocationNote = null,
                    CreatedDate = dec04,
                    CreatedBy = "Seeded Data",
                    ModifiedBy = "Seeded Data",
                    ModifiedDate = dec04,
                },
                new Location {
                    LocationID = 5,
                    LocationName = "USDA Room",
                    LocationNote = null,
                    CreatedDate = dec04,
                    CreatedBy = "Seeded Data",
                    ModifiedBy = "Seeded Data",
                    ModifiedDate = dec04,
                },
                new Location {
                    LocationID = 6,
                    LocationName = "Offsite Storage 1",
                    LocationNote = null,
                    CreatedDate = dec04,
                    CreatedBy = "Seeded Data",
                    ModifiedBy = "Seeded Data",
                    ModifiedDate = dec04,
                },
                new Location {
                    LocationID = 7,
                    LocationName = "South East",
                    LocationNote = null,
                    CreatedDate = dec04,
                    CreatedBy = "Seeded Data",
                    ModifiedBy = "Seeded Data",
                    ModifiedDate = dec04,
                },
                new Location {
                    LocationID = 8,
                    LocationName = "New Warehouse",
                    LocationNote = null,
                    CreatedDate = dec04,
                    CreatedBy = "Seeded Data",
                    ModifiedBy = "Seeded Data",
                    ModifiedDate = dec04,
                },
                new Location {
                    LocationID = 9,
                    LocationName = "Refrigerator 1",
                    LocationNote = null,
                    CreatedDate = dec04,
                    CreatedBy = "Seeded Data",
                    ModifiedBy = "Seeded Data",
                    ModifiedDate = dec04,
                },
                new Location {
                    LocationID = 10,
                    LocationName = "Refrigerator 2",
                    LocationNote = null,
                    CreatedDate = dec04,
                    CreatedBy = "Seeded Data",
                    ModifiedBy = "Seeded Data",
                    ModifiedDate = dec04,
                },
            });
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
