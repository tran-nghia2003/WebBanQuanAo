﻿using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanQuanAo.Models
{
    [Serializable]
    public class CartItem
    {  
        public Product Product { set; get; }
        public int Quantity { set; get; }

        public decimal TotalPrice { set; get; }
        
    }
}