using System;

namespace CCSInventory.Models
{
    public class TransactionLineItem : TrackedModel
    {
        public int TransactionLineItemID { get; set; }
        public int TransactionID { get; set; }
        public int SubcategoryID { get; set; }
        public decimal Weight { get; set; }
        public int? Cases { get; set; }
        public string USDANumber { get; set; }
        public string TransactionLineItemNote { get; set; }

        // Navigation Properties for the above Foreign Keys
        public Transaction Transaction { get; set; }
        public Subcategory Subcategory { get; set; }
    }
}
