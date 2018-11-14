using System;
using System.ComponentModel.DataAnnotations;

namespace CCSInventory.Models
{
    public class TransactionType : TrackedModel
    {
        public int TransactionTypeID { get; set; }
        [Required]
        public string TransactionTypeName { get; set; }
        public bool IsOutgoing { get; set; }
        public string TransactionTypeNote { get; set; }
    }
}