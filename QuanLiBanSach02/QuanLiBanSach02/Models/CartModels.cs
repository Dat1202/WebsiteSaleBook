using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLiBanSach02.Models
{
    [Serializable]
    public class CartModels
    {
        BookStoreDataContext da = new BookStoreDataContext();

        public int ProductID { get; set; }
        public String ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Total { get { return Price * Quantity; } }

        //public Cart(int productID)
        //{
        //    Product p = da.Products.FirstOrDefault(s => s.ProductID == productID);
        //    ProductID = p.ProductID;
        //    ProductName = p.ProductName;
        //    UnitPrice = p.UnitPrice;
        //    Quantity = 1;
        //}
    }
}