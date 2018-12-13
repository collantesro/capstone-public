using System;
using System.Collections.Generic;

namespace CCSInventory.Models
{
    public partial class Log
    {
        public int LogID { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int UserID { get; set; }
        public bool IsArchived { get; set; }

        // Navigation Property
        public User User { get; set; }
    }
}
