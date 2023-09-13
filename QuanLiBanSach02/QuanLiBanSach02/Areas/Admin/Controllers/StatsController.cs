using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLiBanSach02.Models;

namespace QuanLiBanSach02.Areas.Admin.Controllers
{
    public class StatsController : Controller
    {
        private BookStoreEntities da = new BookStoreEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StatsByYear(int? year)
        {
            var lsDataCategoryByYear = da.spThongKeDoanhThuTheLoaiTheoNam(year);

            return Json(lsDataCategoryByYear, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StatsNumberProductByMonth(int? month)
        {
            var lsDataNumberProduct = da.spThongKeSoLuongSanPhamTheoThang(month);

            return Json(lsDataNumberProduct, JsonRequestBehavior.AllowGet);
        }
    }
}
