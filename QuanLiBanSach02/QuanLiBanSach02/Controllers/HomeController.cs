using QuanLiBanSach02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace QuanLiBanSach02.Controllers
{
    public class HomeController : Controller
    {
        private BookStoreEntities da = new BookStoreEntities();
        // GET: Home
        public ActionResult Index(int? page, int? pageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 6;
            }
            List<Product> p = da.Products.ToList();
            return View(p.ToPagedList((int)page, (int)pageSize));
        }
        public ActionResult Search(String search = "")
        {
            if (search == "")
            {
                return RedirectToAction("Index");
            }
            else
            {
                List<Product> p = da.Products.Where(s => s.ProductName.Contains(search)).ToList();
                ViewBag.Search = search;
                return View(p);
            }
        }
    }
}