using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Models
{
    public class TransactionLineItem : TrackedModel
    {
        public int ID { get; set; }
        public int FoodTransactionID { get; set; }
        public int TypeID { get; set; }
        public decimal Weight { get; set; }
        public bool Taxable { get; set; }
        public bool USDA { get; set; }
        public string Note { get; set; }

        // Navigation Properties for the above Foreign Keys
        public FoodTransaction FoodTransaction { get; set; }
        public Type Type { get; set; }
    }
}
