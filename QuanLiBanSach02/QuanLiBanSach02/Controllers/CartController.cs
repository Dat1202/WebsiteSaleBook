using QuanLiBanSach02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Transactions;
using PayPal.Api;

namespace QuanLiBanSach02.Areas.Admin.Controllers
{
    public class CartController : Controller
    {
        private BookStoreEntities da = new BookStoreEntities();
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

            return RedirectToAction("Detail", "Book", new { id = id });
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
        public ActionResult Order()
        {
            List<CartModels> cart = Session["cart"] as List<CartModels>;
            string sMsg = "<html><body><table border= \"1\" ><caption>Thông tin đặt hàng</caption><tr><th>STT</th><th>Tên hàng</th><th>Số lượng</th><th>Đơn giá</th><th>Thành tiền</th></tr>";
            int i = 0;
            double tongTien = 0;
            string email = (string)Session["Email"];

            using (TransactionScope scope = new TransactionScope())
            {
                int userID = (int)Session["UserID"];
                try
                {
                    if (!cart.Any())
                    {
                        TempData["cartIsEmpty"] = "Chưa có đơn hàng";
                    }
                    else
                    {
                        Models.Order order = new Models.Order();
                        order.OrderDate = DateTime.Now;
                        order.UserID = userID;
                        da.Orders.Add(order);
                        da.SaveChanges();

                        var idOrder = order.OrderID;

                        foreach (CartModels item in cart)
                        {
                            OrderDetail orderDetail = new OrderDetail();
                            orderDetail.OrderID = idOrder;
                            orderDetail.ProductID = item.ProductID;
                            orderDetail.UnitPrice = item.Price;
                            orderDetail.Quantity = item.Quantity;
                            da.OrderDetails.Add(orderDetail);
                            da.SaveChanges();

                            i++;
                            sMsg += "<tr>";
                            sMsg += "<td>" + i.ToString() + "</td>";
                            sMsg += "<td>" + item.ProductName + "</td>";
                            sMsg += "<td>" + item.Quantity.ToString() + "</td>";
                            sMsg += "<td>" + item.Price.ToString() + "</td>";
                            sMsg += "<td>" + String.Format("{0:#,###}", item.Quantity * item.Price) + "</td>";
                            sMsg += "</tr>";
                            tongTien += item.Quantity * item.Price;
                        }

                        sMsg += "<tr><th colspan='5'>Tổng cộng: " + String.Format("{0:#,###}", tongTien) + "</th></tr></table></body></html>";

                        MailMessage mail = new MailMessage("2051052027dat@ou.edu.vn", email, "Thông tin đơn hàng", sMsg);
                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                        client.EnableSsl = true;
                        client.Credentials = new NetworkCredential("2051052027dat@ou.edu.vn", "Dat?123456789");
                        mail.IsBodyHtml = true;
                        client.Send(mail);

                        scope.Complete();
                        cart.Clear();

                        return RedirectToAction("Cart");

                    }
                    return RedirectToAction("Cart");

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    return View("Cart");
                }
            }

        }

        public ActionResult FailureView()
        {
            return View();
        }
        public ActionResult SuccessView()
        {
            return View();
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Cart/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }

                    ClearCart();
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            //on successful payment, show success page to user.  
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            List<CartModels> cart = Session["cart"] as List<CartModels>;
            double subtotal = cart.Sum(item => item.Price * item.Quantity);

            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc
            foreach (var item in cart)
            {
                itemList.items.Add(new Item()
                {
                    name = item.ProductName,
                    currency = "USD",
                    price = item.Price.ToString(),
                    quantity = item.Quantity.ToString(),
                    sku = item.ProductID.ToString(),
                });
            }

            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = subtotal.ToString()
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = subtotal.ToString(), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<PayPal.Api.Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new PayPal.Api.Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }

        private void ClearCart()
        {
            Session["cart"] = new List<CartModels>();
        }
    
}
}
