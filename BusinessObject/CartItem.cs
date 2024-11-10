﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CartItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; } 
        public double? Price { get; set; }
        public int Quantity { get; set; }
        public byte[]? ImageData { get; set; }
    }
}
