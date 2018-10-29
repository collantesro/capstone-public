using System;

namespace CCSInventory.Models
{
    public class TransactionLineItem : TrackedModel
    {
        public int ID { get; set; }
        public int TransactionID { get; set; }
        public int SubcategoryID { get; set; }
        public decimal Weight { get; set; }
        public bool IsTaxable { get; set; }
        public bool IsUSDA { get; set; }
        public string Note { get; set; }

        // Navigation Properties for the above Foreign Keys
        public Transaction Transaction { get; set; }
        public Subcategory Subcategory { get; set; }
    }
}
