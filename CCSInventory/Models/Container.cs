using System;

namespace CCSInventory.Models
{
    public class Container : TrackedModel
    {
        public int ContainerID { get; set; }
        public int BinNumber { get; set; }
        public int SubcategoryID { get; set; }
        public decimal Weight { get; set; }
        public int Units { get; set; } // This will be displayed as "Cases" for most cases, but "Quantity" when it's Pantry Pack.
        public int LocationID { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsArchived { get; set; }
        public string ContainerNote { get; set; }

        public Subcategory Subcategory { get; set; }
        public Location Location { get; set; }
    }

    public enum EContainerType {
        Standard,
        USDA,
        GroceryRescue,
        PantryPack,
    }
}
