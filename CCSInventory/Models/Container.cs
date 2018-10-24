using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace CCSInventory.Models
{
    public class Container : TrackedModel
    {
        public int ID { get; set; }
        public int BinNumber { get; set; }
        public int USDACategoryID { get; set; }
        public decimal Weight { get; set; }
        public int Cases { get; set; }
        public string Note { get; set; }
    }
}
