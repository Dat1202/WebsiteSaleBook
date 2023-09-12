﻿using QuanLiBanSach02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLiBanSach02.Controllers
{
    public class BookController : Controller
    {
        private BookStoreEntities da = new BookStoreEntities();

        // GET: BooK
        public ActionResult Detail(int id)
        {
            Product p = da.Products.Where(s => s.ProductID == id).FirstOrDefault();
            return View(p);
        }
    }
}