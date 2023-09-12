using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLiBanSach02.Models
{
    public class OrderDetailModel
    {
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
    }
}