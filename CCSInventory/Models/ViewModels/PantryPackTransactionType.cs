using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Models.ViewModels
{
    public class PantryPackTransactionType : TrackedModel
    {
        public int TypeID { get; set; }
        public int TransactionID { get; set; }

        [Required]
        public string Name { get; set; }
        public string Note { get; set; }

        public int Qty { get; set; }
        public int PackTypeID { get; set; }
        public PantryPackType PackType { get; set; }
    }
}
