using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models
{
    public class Transaction : TrackedModel
    {
        public int TransactionID { get; set; }
        public int AgencyID { get; set; }
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }
        public bool IsOutgoing { get; set; }
        public bool IsArchived { get; set; }
        [Display(Name = "Note")]
        public string TransactionNote { get; set; }

        // Navigation Properties for the above Foreign Key and associated LineItems
        public Agency Agency { get; set; }
        public List<TransactionLineItem> LineItems { get; set; }
    }
}
