using System;
using System.Collections.Generic;

namespace CCSInventory.Models
{
    public class FoodTransaction : TrackedModel
    {
        public int ID { get; set; }
        public int AgencyID { get; set; }
        public int USDACategoryID { get; set; }
        public int TypeID { get; set; }
        public DateTime TransactionDate { get; set; }

        // Navigation Properties for the above Foreign Key and associated LineItems
        public Agency Agency { get; set; }
        public ICollection<TransactionLineItem> LineItems;
    }
}
