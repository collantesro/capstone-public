using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Models
{
    public class ErrorLog
    {
        public int ErrorLogID { get; set; }
        public int UserID { get; set; }
        public string FileName { get; set; }
        public DateTime TimeStamp { get; set; }
        public string FunctionName { get; set; }
        public string LineNumber { get; set; }
        public string ErrorText { get; set; }

        // Navigation Property
        public User User { get; set; }
    }
}
