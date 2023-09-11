using QuanLiBanSach02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Transactions;

namespace QuanLiBanSach02.Areas.Admin.Controllers
{
    public class CartController : Controller
    {
        BookStoreDataContext da = new BookStoreDataContext();
        // GET: Admin/Cart
        public ActionResult Cart()
        {
            List<CartModels> cart = Session["cart"] as List<CartModels>;
            return View(cart);
        }

        public RedirectToRouteResult AddToCart(int id)
        {
            if (Session["cart"] == null)
            {
                Session["cart"] = new List<CartModels>();
            }
            List<CartModels> cart = Session["cart"] as List<CartModels>;

            if (cart.FirstOrDefault(s => s.ProductID == id) == null)
            {
                Product p = da.Products.FirstOrDefault(s => s.ProductID == id);
                CartModels newCart = new CartModels();
                newCart.ProductID = id;
                newCart.ProductName = p.ProductName;
                newCart.Quantity = 1;
                newCart.Price = Convert.ToDouble(p.Price);
                cart.Add(newCart);
            }
            else
            {
                CartModels cartItem = cart.FirstOrDefault(s => s.ProductID == id);
                cartItem.Quantity++;
            }
            Session["cart"] = cart;

            return RedirectToAction("Details","Product", new { id = id });
        }

        public RedirectToRouteResult UpdateCart(int id, int txtSoLuong)
        {
            List<CartModels> cart = Session["cart"] as List<CartModels>;
            CartModels item = cart.FirstOrDefault(m => m.ProductID == id);
            if (item != null)
            {
                item.Quantity = txtSoLuong;
                Session["cart"] = cart;
            }
            return RedirectToAction("Cart");
        }

        public RedirectToRouteResult DeleteCartItem(int id)
        {
            List<CartModels> cart = Session["cart"] as List<CartModels>;
            CartModels item = cart.FirstOrDefault(m => m.ProductID == id);
            if (item != null)
            {
                cart.Remove(item);
                Session["cart"] = cart;
            }
            return RedirectToAction("Cart");
        }

        //[HttpPost]
        public ActionResult Order(string email, string phone)
        {
            List<CartModels> cart = Session["cart"] as List<CartModels>;


            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    if (!cart.Any())
                    {
                        TempData["cartIsEmpty"] = "Không có đơn hàng";
                    }
                    else
                    {
                        Order order = new Order();
                        order.OrderDate = DateTime.Now;

                        da.Orders.InsertOnSubmit(order);
                        da.SubmitChanges();

                        var idOrder = order.OrderID;

                        foreach (CartModels item in cart)
                        {
                            OrderDetail orderDetail = new OrderDetail();
                            orderDetail.OrderID = idOrder;
                            orderDetail.ProductID = item.ProductID;
                            orderDetail.UnitPrice = item.Price;
                            orderDetail.Quantity = item.Quantity;
                            da.OrderDetails.InsertOnSubmit(orderDetail);
                            da.SubmitChanges();

 
                        }


                        scope.Complete();
                        cart.Clear();

                        return RedirectToAction("Cart");

                    }
                    return RedirectToAction("Cart");

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    //ModelState.AddModelError("", "Ban chưa nhập thông tin.");
                    return View("Cart");
                }
            }
        }
    }
}
