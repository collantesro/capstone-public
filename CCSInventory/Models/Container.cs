using System;

namespace CCSInventory.Models
{
    public class Container : TrackedModel
    {
        public int ID { get; set; }
        public int BinNumber { get; set; }
        public int SubcategoryID { get; set; }
        public decimal Weight { get; set; }

        #warning Is Cases required in Container?
        // public int Cases { get; set; }
        public string Note { get; set; }

        public Subcategory Subcategory {get;set;}
    }
}
