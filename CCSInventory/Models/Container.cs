using System;

namespace CCSInventory.Models
{
    public class Container : TrackedModel
    {
        public int ContainerID { get; set; }
        public int BinNumber { get; set; }
        public int SubcategoryID { get; set; }
        public decimal Weight { get; set; }
        public int Cases { get; set; }
        public string Location { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsUSDA { get; set; }
        public bool IsArchived { get; set; }
        public string ContainerNote { get; set; }

        public Subcategory Subcategory { get; set; }
    }
}
