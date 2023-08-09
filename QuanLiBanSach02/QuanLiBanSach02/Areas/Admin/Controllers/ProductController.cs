using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLiBanSach02.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        BookStoreDataContext da = new BookStoreDataContext();

        // GET: Admin/Product
        public ActionResult ListProduct()
        {
            List<Product> c = da.Products.Select(s => s).ToList();
            return View(c);
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Product/Create
        public ActionResult CreateProduct()
        {
            ViewData["LoaiSP"] = new SelectList(da.Categories, "CategoryID", "CategoryName");
            return View();
        }

        public ActionResult ChooseCate()
        {
            Category cate = new Category();
            cate.CateCollection = da.Categories.ToList<Category>();
            return View(cate);
        }

        // POST: Admin/Product/Create
        [HttpPost]
        public ActionResult CreateProduct(Product product, HttpPostedFileBase uploadhinh, FormCollection collection)
        {
            try
            {
                bool productExists = da.Products.Any(s => s.ProductName == product.ProductName);
                if (productExists)
                {
                    TempData["ErrorAddProductMessage"] = "Thể loại đã tồn tại.";
                }
                else
                {
                    Product p = new Product();

                    p = product;
                    p.CateID = int.Parse(collection["LoaiSP"]);

                    da.Products.InsertOnSubmit(p);
                    da.SubmitChanges();
                    TempData["SuccAddCateMessage"] = "Thêm sản phẩm thành công.";

                    if (uploadhinh != null && uploadhinh.ContentLength > 0)
                    {
                        int id = int.Parse(da.Products.ToList().Last().ProductID.ToString());

                        string _FileName = "";
                        int index = uploadhinh.FileName.IndexOf('.');
                        _FileName = "products" + id.ToString() + "."  + uploadhinh.FileName.Substring(index+1);
                        string _path = Path.Combine(Server.MapPath("~/Content/Images"), _FileName);
                        uploadhinh.SaveAs(_path);

                        Product unv = da.Products.FirstOrDefault(x => x.ProductID == id);
                        unv.Image = _FileName;
                        da.SubmitChanges();

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

            return View(p);
        }

        // POST: Admin/Product/Edit/5
        [HttpPost]
        public ActionResult EditProduct(int id, Product product, FormCollection collection)
        {
            try
            {
                bool productExists = da.Products.Any(c => c.ProductName == product.ProductName);

                if (productExists)
                {
                    TempData["ErrorEditProductMessage"] = "Sản phẩm đã tồn tại.";
                }
                else
                {
                    Product c = da.Products.First(s => s.ProductID == id);
                    c.ProductName = product.ProductName;
                    c.Author = product.Author;
                    c.Price = product.Price;
                    c.Description = product.Description;
                    c.CateID = product.CateID;

                    da.SubmitChanges();

                    TempData["SuccessEditProductMessage"] = "Cập nhật sản phẩm thành công.";
                    return RedirectToAction("ListProduct");
                }
                return RedirectToAction("EditProduct");
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
                    da.Products.DeleteOnSubmit(product);
                    da.SubmitChanges();

                    TempData["SuccessDelete"] = "Xóa thể loại thành công.";
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
