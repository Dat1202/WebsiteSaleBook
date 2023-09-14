using QuanLiBanSach02.Models;
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
            Session["productID"] = id;
            Product pro = da.Products.Where(s => s.ProductID == id).FirstOrDefault();
            ViewBag.Products = da.Products.Where(p => p.CategoryID == pro.CategoryID).ToList();

            return View(pro);
        }

        public ActionResult ViewComments(int id)
        {
            List<Comment> comments = da.Comments.Where(c => c.ProductID == id).ToList();

            return View(comments);
        }

        public ActionResult AddComment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddComment(Comment comment, FormCollection collection)
        {
            Comment c = new Comment();
            c = comment;
            c.ProductID = (int)Session["productID"];
            c.UserID = (int)Session["UserID"];
            c.CreatedAt = DateTime.Now;
            da.Comments.Add(c);
            da.SaveChanges();
            return View();

            //return Redirect("~/Book/Detail/" + (int)Session["productID"]);
        }
    }
}