using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLiBanSach02.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        BookStoreDataContext da = new BookStoreDataContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        } 
    }
}