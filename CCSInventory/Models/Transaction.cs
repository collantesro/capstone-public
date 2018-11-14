using System;
using System.Collections.Generic;

namespace CCSInventory.Models
{
    public class Transaction : TrackedModel
    {
        public int TransactionID { get; set; }
        public int AgencyID { get; set; }
        public int TransactionTypeID { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsOutgoing { get; set; }

        // Navigation Properties for the above Foreign Key and associated LineItems
        public Agency Agency { get; set; }
        public TransactionType TransactionType { get; set; }
        public List<TransactionLineItem> LineItems;
    }
}
