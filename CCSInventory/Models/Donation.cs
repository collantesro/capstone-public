﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCSInventory.Models
{
    public class Donation : TrackedModel
    {
        public int Id { get; set; }
        public int DonorId { get; set; }
        public int USDACategoryId { get; set; }
        public int TypeId { get; set; }
        public int CategoryId { get; set; }
        public DateTime DonationDate { get; set; }
    }
}
