using System;
using System.Collections.Generic;

namespace CCSInventory.Models
{
    public class Transaction : TrackedModel
    {
        public int ID { get; set; }
        public int AgencyID { get; set; }
        public DateTime Date { get; set; }
        public bool IsOutgoing { get; set; }

        // Navigation Properties for the above Foreign Key and associated LineItems
        public Agency Agency { get; set; }
        public ICollection<TransactionLineItem> LineItems;
    }
}
