using System;
using System.Linq;
using System.Web.Mvc;
using log4net;
using NongSanShop.Common;
using NongSanShop.Filters;
using NongSanShop.Models;
using NongSanShop.Models.Custom;

namespace NongSanShop.Controllers
{
    [UserAuthorizationFilter]
    public class CheckoutController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(nameof(CheckoutController));

        private NongSanDB dbContext = new NongSanDB();

        // GET
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(FormCollection formCollection)
        {
            var name = formCollection["name"];
            var phone = formCollection["phone_number"];
            var email = formCollection["email"];
            var address = formCollection["address"];
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(address))
            {
                ViewBag.Error = "Vui lòng điền đầy đủ thông tin nhận hàng ";
                return RedirectToAction("Index");
            }
            try
            {
                var userItem = Session[Constants.SessionItem.User];
                if (userItem is AppUser)
                {
                    var user = (AppUser) userItem;
                    var listCarts = dbContext.dh_cart.Select(p => p).ToList();
                    if (listCarts.Count == 0)
                    {
                        ViewBag.Error = "Vui lòng thêm hàng vào giỏ";
                        return RedirectToAction("Index");
                    }

                    dh_order dhOrder = new dh_order();
                    dhOrder.user_id = user.UserId;
                    dhOrder.name = name;
                    dhOrder.phone = phone;
                    dhOrder.email = email;
                    dhOrder.address = address;
                    dhOrder.status = 0;
                    dhOrder.created = DateTimeOffset.Now.ToUnixTimeSeconds();
                    dhOrder.code_name = RandomString(6).ToUpper();
                    dbContext.dh_order.Add(dhOrder);
                    listCarts.ForEach(val =>
                    {
                        dh_order_product dhOrderProduct = new dh_order_product();
                        dhOrderProduct.dh_order = dhOrder;
                        dhOrderProduct.dh_product = val.dh_product;
                        dhOrderProduct.quantity = val.quantity;
                        dhOrderProduct.price = val.price;
                        dbContext.dh_order_product.Add(dhOrderProduct);
                    });
                    dbContext.dh_cart.RemoveRange(listCarts);
                    dbContext.SaveChanges();
                }
                else
                {
                    ViewBag.Error = "Có lỗi xảy ra, bạn vui lòng đăng nhập lại";
                    return RedirectToAction("Index");
                }
                // result order page
                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                Logger.Error("Checkout and create order error", ex);
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index");
            }
        }
        
        public static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}