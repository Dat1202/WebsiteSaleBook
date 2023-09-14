using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLiBanSach02.Models;

namespace QuanLiBanSach02.Areas.Admin.Controllers
{
    public class DashBoardController : Controller
    {
        private BookStoreEntities da = new BookStoreEntities();

        // GET: Admin/DashBroad
        public ActionResult DashBoard()
        {
            int productCount = da.Products.Count();
            int cateCount = da.Categories.Count();

            ViewBag.ProductCount = productCount;
            ViewBag.CateCount = cateCount;

            return View();
        }

        public ActionResult StatsByMonth()
        {
            var lsDataStatsByMonth = da.ThongKeDoanhThuTheoThang();

            return Json(lsDataStatsByMonth, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ThongKeSoLuongSachTheoTheLoai()
        {
            var ThongKeSoLuongSachTheoTheLoai = da.ThongKeSoLuongSachTheoTheLoai();

            return Json(ThongKeSoLuongSachTheoTheLoai, JsonRequestBehavior.AllowGet);
        }

    }
}