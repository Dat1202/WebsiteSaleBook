using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLiBanSach02.Models;

namespace QuanLiBanSach02.Controllers
{
    public class ListOrderController : Controller
    {
        private BookStoreEntities da = new BookStoreEntities();

        public ActionResult ListOrderDetail()
        {
            if (Session["UserID"] != null)
            {
                int userID = (int)Session["UserID"];
                var danhSachDonHang = (
                    from o in da.Orders
                    join od in da.OrderDetails on o.OrderID equals od.OrderID
                    join u in da.Users on o.UserID equals u.UserID
                    join p in da.Products on od.ProductID equals p.ProductID
                    where o.UserID == userID
                    group new { o, od, p } by o.OrderID into grouped
                    select new ListOrderModels
                    {
                OrderId = grouped.Key,
                        Status = grouped.FirstOrDefault().o.Status,
                        OrderDetails = grouped.Select(g => new OrderDetailModel
                {
                    ProductName = g.p.ProductName,
                    UnitPrice = (double?)g.od.UnitPrice ?? 0.0,
                    Quantity = (int?)g.od.Quantity ?? 0,
                    Image = g.p.Image,
                    Status = g.o.Status,
                    ProductID = (int)g.p.ProductID
                }).ToList()
            }
        ).ToList();
                return View(danhSachDonHang);

            }
            return View();
        }
    }
}