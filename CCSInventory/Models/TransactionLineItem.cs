using System;

namespace CCSInventory.Models
{
    public class TransactionLineItem : TrackedModel
    {
        public int TransactionLineItemID { get; set; }
        public int TransactionID { get; set; }
        public int TransactionTypeID { get; set; }
        public int SubcategoryID { get; set; }
        public decimal Weight { get; set; }
        public int Units { get; set; } = 0; // Display this as "Cases" when it's USDA, "Quantity" as PantryPack.
        public bool IsPantryPack { get; set; }
        public string TransactionLineItemNote { get; set; }

        // Navigation Properties for the above Foreign Keys
        public Transaction Transaction { get; set; }
        public TransactionType TransactionType { get; set; }
        public Subcategory Subcategory { get; set; }
    }
}
