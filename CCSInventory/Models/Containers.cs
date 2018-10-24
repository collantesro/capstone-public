using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Models
{
    public class Containers
    {
        public int Id{ get; set; }
        public int BinNumber { get; set; }
        public int USDACategoryId { get; set; }
        public int TypeId { get; set; }
        public bool USDA { get; set; }
        public decimal Weight { get; set; }
        public int Cases { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

    }
}
