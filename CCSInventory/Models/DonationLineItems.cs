﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Models
{
    public class DonationLineItems
    {
        public int Id { get; set; }
        public int DonationId { get; set; }
        public int TypeId { get; set; }
        public decimal Weight { get; set; }
        public bool Taxable { get; set; }
        public bool USDA { get; set; }
        public String Note { get; set; }
    }
}
