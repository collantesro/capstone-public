using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Models
{
    public class Type
    {
        public int Id { get; set; }
        public int ContainerId { get; set; }
        public int RecipientId { get; set; }
        public string TypeName { get; set; }
    }
}
