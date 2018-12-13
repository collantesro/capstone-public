using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCSInventory.Models;

namespace CCSInventory.ViewModels
{
    public class TransactionData
    {
        public IEnumerable<Transaction> Transactions { get; set; }
        public IEnumerable<TransactionLineItem> LineItems { get; set; }
        public IEnumerable<Subcategory> Subcategories { get; set; }
    }
}
