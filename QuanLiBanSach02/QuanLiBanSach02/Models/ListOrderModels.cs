using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLiBanSach02.Models
{
    public class ListOrderModels
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public List<OrderDetailModel> OrderDetails { get; set; }
    }
}
