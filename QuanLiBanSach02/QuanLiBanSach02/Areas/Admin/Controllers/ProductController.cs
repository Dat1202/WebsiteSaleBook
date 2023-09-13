using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLiBanSach02.Models;

namespace QuanLiBanSach02.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private BookStoreEntities da = new BookStoreEntities();

        // GET: Admin/Product
        public ActionResult ListProduct()
        {
            List<Product> c = da.Products.Select(s => s).ToList();
            return View(c);
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int id)
        {
            Product p = da.Products.FirstOrDefault(s => s.ProductID == id);
            return View(p);
        }

        // GET: Admin/Product/Create
        public ActionResult CreateProduct()
        {
            ViewData["LoaiSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName"); ;

            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct(Product product, HttpPostedFileBase uploadhinh, FormCollection collection)
        {
            try
            {
                bool productExists = da.Products.Any(s => s.ProductName == product.ProductName);
                if (productExists)
                {
                    TempData["ErrorAddProductMessage"] = "Sản phẩm đã tồn tại.";
                }
                else
                {
                    Product p = new Product();

                    p = product;
                    p.CategoryID = int.Parse(collection["LoaiSP"]);

                    da.Products.Add(p);
                    da.SaveChanges();
                    TempData["SuccAddProductMessage"] = "Thêm sản phẩm thành công.";

                    if (uploadhinh != null && uploadhinh.ContentLength > 0)
                    {
                        int id = int.Parse(da.Products.ToList().Last().ProductID.ToString());

                        string _FileName = "";
                        int index = uploadhinh.FileName.IndexOf('.');
                        _FileName = "products" + id.ToString() + "." + uploadhinh.FileName.Substring(index + 1);
                        string _path = Path.Combine(Server.MapPath("~/Content/Images"), _FileName);
                        uploadhinh.SaveAs(_path);

                        Product unv = da.Products.FirstOrDefault(x => x.ProductID == id);
                        unv.Image = _FileName;
                        da.SaveChanges();
                    }

                    return RedirectToAction("ListProduct");
                }
                return RedirectToAction("CreateProduct");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Product/Edit/5
        public ActionResult EditProduct(int id)
        {
            Product p = da.Products.FirstOrDefault(s => s.ProductID == id);

            ViewData["LoaiSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName");

            return View(p);
        }

        // POST: Admin/Product/Edit/5
        [HttpPost]
        public ActionResult EditProduct(int id, Product product, FormCollection collection, HttpPostedFileBase uploadhinh)
        {
            try
            {
                Product p = da.Products.First(s => s.ProductID == id);
                p.ProductName = product.ProductName;
                p.Author = product.Author;
                p.Price = product.Price;
                p.Description = product.Description;
                p.CategoryID = int.Parse(collection["LoaiSP"]);
                TempData["SuccessEditProductMessage"] = "Cập nhật sản phẩm thành công.";

                if (uploadhinh != null && uploadhinh.ContentLength > 0)
                {
                    int idImg = id;

                    string _FileName = "";
                    int index = uploadhinh.FileName.IndexOf('.');
                    _FileName = "products" + idImg.ToString() + "." + uploadhinh.FileName.Substring(index + 1);
                    string _path = Path.Combine(Server.MapPath("~/Content/Images"), _FileName);
                    uploadhinh.SaveAs(_path);
                    p.Image = _FileName;
                }
                da.SaveChanges();
                return RedirectToAction("ListProduct");
            }

            catch
            {
                return View();
            }
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Product/Delete/5
        [HttpPost]
        public ActionResult DeleteProduct(int id, FormCollection collection)
        {
            try
            {
                // Tìm thể loại dựa trên ID
                Product product = da.Products.FirstOrDefault(c => c.ProductID == id);

                if (product != null)
                {
                    // Xóa thể loại
                    da.Products.Remove(product);
                    da.SaveChanges();

                    TempData["SuccessDeleteProduct"] = "Xóa sản phẩm thành công.";
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
            }

            return RedirectToAction("ListProduct");
        }
    }
}
