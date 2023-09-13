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

            var categoryCounts = da.Products
                               .Join(da.Categories,
                                   p => p.CategoryID,
                                   c => c.CategoryID,
                                   (p, c) => new { Product = p, Category = c })
                               .GroupBy(x => new { x.Category.CategoryID, x.Category.CategoryName })
                               .Select(g => new CategoryProductCountModels
                               {
                                   CategoryName = g.Key.CategoryName,
                                   ProductCount = g.Count()
                               })
                               .ToList();

            return View(categoryCounts);
        }

        public ActionResult StatsByMonth()
        {
            var lsDataStatsByMonth = da.ThongKeDoanhThuTheoThang();

            return Json(lsDataStatsByMonth, JsonRequestBehavior.AllowGet);
        }

    }
}