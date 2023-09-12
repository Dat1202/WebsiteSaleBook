using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLiBanSach02.Models;
using System.Security.Cryptography;
using System.Text;

namespace QuanLiBanSach02.Controllers
{
    public class AccountController : Controller
    {
        private BookStoreEntities da = new BookStoreEntities();

        // GET: Account
        public ActionResult Index()
        {
            return View(da.Users.ToList());
        }

        // GET: User/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: User/Login
        [HttpPost]
        public ActionResult Login(string Email, string password)
        {
            //Mã hóa mật khẩu
            password = GetMD5(password);

            if (IsValidLogin(Email, password))
            {
                var data = da.Users.Where(u => u.Email.Equals(Email) && u.Password.Equals(password));

                //add session 
                Session["UserName"] = data.FirstOrDefault().UserName;
                Session["Email"] = data.FirstOrDefault().Email;
                Session["UserID"] = data.FirstOrDefault().UserID;
                ViewBag.UserName = Session["UserName"];


                // Thực hiện đăng nhập thành công
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Thông tin đăng nhập không hợp lệ
                ModelState.AddModelError("", "Thông tin tài khoản hoặc mật khẩu không chính xác");
                return View();
            }
        }

        private bool IsValidLogin(string Email, string password)
        {
            // Thực hiện truy vấn cơ sở dữ liệu để kiểm tra thông tin đăng nhập
            var user = da.Users.FirstOrDefault(u => u.Email.Equals(Email) && u.Password.Equals(password));

            return user != null;
        }

        // GET: User/Register
        public ActionResult Register()
        {
            return View();
        }
        // POST: User/Register
        [HttpPost]
        public ActionResult Register(User user, string password2)
        {
            try
            {
                if (user.Email == null || user.UserName == null || user.Password == null || password2 == null)
                {
                    ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin");
                    return View();
                }
                else
                {
                    if (user.Password == password2)
                    {
                        user.Password = GetMD5(user.Password);
                        User s = user;
                        da.Users.Add(s);
                        da.SaveChanges();
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mật khẩu xác nhận không khớp");
                        return View();
                    }
                }


            }
            catch
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();//Remove session

            return RedirectToAction("Login");
        }

        public string GetMD5(string pass)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(pass);
            byte[] targetData = md5.ComputeHash(fromData);

            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
    }
}
