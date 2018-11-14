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
        public string TransactionNote { get; set; }
        public string TypeNote { get; set; }

        //if it is a 0, it will be in going, 
        //if it is a 1, it will be out going
        public int TransactionType { get; set; }

        public int Qty { get; set; }
        public int PackTypeID { get; set; }
        public PantryPackType PackType { get; set; }
    }
}
