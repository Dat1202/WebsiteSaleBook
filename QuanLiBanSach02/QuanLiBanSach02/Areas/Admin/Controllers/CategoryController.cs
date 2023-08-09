using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLiBanSach02.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        BookStoreDataContext da = new BookStoreDataContext();

        // GET: Admin/Category/ListCategory
        public ActionResult ListCategory()
        {
            List<Category> c = da.Categories.Select(s => s).ToList();
            return View(c);
        }

        // GET: Admin/Category/Create
        public ActionResult CreateCategory()
        {
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        public ActionResult CreateCategory(Category category, FormCollection collection)
        {
            try
            {
                bool categoryExists = da.Categories.Any(s => s.CategoryName == category.CategoryName);
                if (categoryExists)
                {
                    TempData["ErrorAddCateMessage"] = "Thể loại đã tồn tại.";
                }
                else
                {
                    Category c = new Category();
                    c = category;
                    da.Categories.InsertOnSubmit(c);
                    da.SubmitChanges();
                    TempData["SuccAddCateMessage"] = "Thêm thể loại thành công.";

                    return RedirectToAction("ListCategory");
                }
                return RedirectToAction("CreateCategory");

            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Category/Edit/5
        public ActionResult EditCategory(int id)
        {
            Category category = da.Categories.FirstOrDefault(s => s.CategoryID == id);

            return View(category);
        }

        

        // POST: Admin/Category/Edit/5
        [HttpPost]
        public ActionResult EditCategory(int id, Category category, FormCollection collection)
        {
            try
            {
                bool existingCategory = da.Categories.Any(c => c.CategoryName == category.CategoryName);

                if (existingCategory)
                {
                    TempData["ErrorEditCateMessage"] = "Thể loại đã tồn tại.";
                }
                else
                {
                    Category c = da.Categories.First(s => s.CategoryID == id);
                    c.CategoryName = category.CategoryName;
                    da.SubmitChanges();

                    TempData["SuccessEditCateMessage"] = "Cập nhật thể loại thành công.";
                    return RedirectToAction("ListCategory");
                }
                return RedirectToAction("EditCategory");

            }
            catch
            {
                return View();
            }
        }



        // GET: Admin/Category/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Admin/Category/Delete/5
        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            try
            {
                // Tìm thể loại dựa trên ID
                Category category = da.Categories.FirstOrDefault(c => c.CategoryID == id);

                if (category != null)
                {
                    // Xóa thể loại
                    da.Categories.DeleteOnSubmit(category);
                    da.SubmitChanges();

                    TempData["SuccessDelete"] = "Xóa thể loại thành công.";
                }
                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi: " + ex.Message;
            }

            return RedirectToAction("ListCategory");
        }
    }
}
