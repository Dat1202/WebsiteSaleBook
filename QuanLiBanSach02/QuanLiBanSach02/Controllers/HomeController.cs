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
        public HomeController()
        {
            ViewBag.Category = da.Categories.ToList();
        }
        private BookStoreEntities da = new BookStoreEntities();

        // GET: Home
        public ActionResult Index(int? page, int? pageSize, int? categoryId, float? fromPrice, float? toPrice, string sortOrder)
        {
            if (page == null)
            {
                page = 1;
            }
            if (pageSize == null)
            {
                pageSize = 8;
            }

            List<Product> products = GetProductsByCategory(categoryId);

            if (fromPrice != null || toPrice != null)
            {
                products = products.Where(p =>
                    (fromPrice == null || p.Price > fromPrice) &&
                    (toPrice == null || p.Price < toPrice)
                ).ToList();
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.PriceSortParam = string.IsNullOrEmpty(sortOrder) ? "price_desc" : "";

            switch (sortOrder)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                default:
                    // No sorting parameter specified, use default order
                    break;
            }

            return View(products.ToPagedList((int)page, (int)pageSize));
        }

        public ActionResult Search(String search = "")
        {
            if (search == "")
            {
                List<Product> p = da.Products.Where(s => s.ProductName.Contains(search)).ToList();
                search = ViewBag.Search;
                return View(p);
            }

             return RedirectToAction("Index");
        }

        public List<Product> GetProductsByCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                return da.Products.ToList();
            }

            return da.Products.Where(p => p.CategoryID == categoryId).ToList();
        }
    }
}