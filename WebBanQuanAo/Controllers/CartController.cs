using Models.Dao;        // Import các namespace cần thiết
using Models.EF;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo.Common;
using WebBanQuanAo.Models;
using WebBanQuanAo.Models.Payment;
using Order = Models.EF.Order;

namespace WebBanQuanAo.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";  // Khai báo tên cho Session lưu trữ giỏ hàng

        // GET: Cart
        public ActionResult Index()
        {
            // Lấy giỏ hàng từ Session
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);  // Hiển thị danh sách sản phẩm trong giỏ hàng
        }

        // Action để thêm sản phẩm vào giỏ hàng
        public ActionResult AddItem(long productId, int quantity)
        {
            // Lấy thông tin sản phẩm dựa trên productId
            var product = new ProductDao().ViewDetail(productId);

            // Lấy giỏ hàng từ Session
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.ID == productId))
                {
                    // Nếu sản phẩm đã tồn tại trong giỏ hàng, tăng số lượng
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity += quantity;
                            item.TotalPrice = (decimal)(product.PromotionPrice ?? product.Pice) * quantity;
                        }
                    }
                }
                else
                {
                    // Nếu sản phẩm chưa tồn tại trong giỏ hàng, tạo một mục mới
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    item.TotalPrice = (decimal)(product.PromotionPrice ?? product.Pice) * quantity;
                    list.Add(item);
                }
            }
            else
            {
                // Nếu giỏ hàng không tồn tại, tạo giỏ hàng mới
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                item.TotalPrice = (decimal)(product.PromotionPrice ?? product.Pice);
                var list = new List<CartItem>();
                list.Add(item);

                // Gán giỏ hàng vào Session
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");  // Điều hướng về trang hiển thị giỏ hàng
        }

        // Action để cập nhật số lượng sản phẩm trong giỏ hàng
        public ActionResult UpdateItem(long productId, int quantity)
        {
            var cart = Session[CartSession] as List<CartItem>;
            if (cart != null)
            {
                // Tìm sản phẩm trong giỏ hàng dựa trên productId
                var cartItem = cart.FirstOrDefault(x => x.Product.ID == productId);
                if (cartItem != null)
                {
                    // Cập nhật số lượng sản phẩm
                    cartItem.Quantity = quantity;
                    cartItem.TotalPrice = (decimal)(cartItem.Product.PromotionPrice ?? cartItem.Product.Pice) * quantity;
                }
            }
            return RedirectToAction("Index"); // Sau khi cập nhật, điều hướng về trang hiển thị giỏ hàng
        }

        // Action để xóa sản phẩm khỏi giỏ hàng
        public ActionResult RemoveItem(long productId)
        {
            var cart = Session[CartSession] as List<CartItem>;
            if (cart != null)
            {
                // Tìm sản phẩm trong giỏ hàng dựa trên productId
                var cartItem = cart.FirstOrDefault(x => x.Product.ID == productId);
                if (cartItem != null)
                {
                    // Xóa sản phẩm khỏi giỏ hàng
                    cart.Remove(cartItem);
                }
            }
            return RedirectToAction("Index"); // Sau khi xóa, điều hướng về trang hiển thị giỏ hàng
        }

        // Action để hiển thị giỏ hàng và thông tin đơn hàng trước khi thanh toán
        [HttpGet]
        public ActionResult Payment()
        {
            if (Session[CommonConstants.USER_SESSION] == null || Session[CommonConstants.USER_SESSION].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            List<CartItem> ShoppingCart = Session[CartSession] as List<CartItem>;

            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new UserDao();
            ViewBag.User = dao.GetById(session.UserName);
            return View(ShoppingCart);  // Hiển thị trang thanh toán với giỏ hàng và thông tin đơn hàng
        }





        // Action để xử lý thanh toán
        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string email, string address)
        {
            //var code = new { Success = false, Code = -1, Url = "" };

            List<CartItem> Cart = Session[CartSession] as List<CartItem>;

            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                using (var context = new OnlineShopDbConext())
                {
                    // Tạo đơn hàng mới
                    var order = new Order
                    {
                        CustomerID = session.UserID,
                        ShipName = shipName,
                        CreatedDate = DateTime.Now,
                        ShipMobile = mobile,
                        ShipEmail = email,
                        ShipAddress = address,
                        TotalPrice = Cart.Sum(x => x.TotalPrice),
                        TypePayment = 1,
                        Status = 1
                    };

                    // Lưu đơn hàng vào cơ sở dữ liệu
                    context.Orders.Add(order);
                    context.SaveChanges();

                    // Lấy giỏ hàng từ Session
                    List<CartItem> ShoppingCart = Session[CartSession] as List<CartItem>;

                    // Lưu chi tiết đơn hàng từ giỏ hàng vào cơ sở dữ liệu
                    foreach (var cartItem in ShoppingCart)
                    {
                        var orderDetail = new OrderDetail
                        {
                            OrderID = order.ID,
                            ProductID = cartItem.Product.ID,
                            Price = cartItem.Product.PromotionPrice ?? cartItem.Product.Pice,
                            Quantity = cartItem.Quantity,
                            NameProduct = cartItem.Product.Name,
                        };
                        context.OrderDetails.Add(orderDetail);

                        var product = context.Products.Find(cartItem.Product.ID);
                        if (product != null)
                        {
                            product.Quantity -= cartItem.Quantity;
                        }
                    }

                    context.SaveChanges();


                    Session[CartSession] = null;
                    return RedirectToAction("SuccessOCD", "Cart");
                }
            }
            else
            {
                return Content("Xảy ra lỗi, mời bạn kiểm tra lại thông tin đã điền");
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
            // lấy apiContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                // Tài nguyên đại diện cho Người trả tiền tài trợ cho Phương thức thanh toán thanh toán dưới dạng paypal
                // Id người thanh toán sẽ được trả về khi tiến hành thanh toán hoặc nhấp để thanh toán
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    // phần này sẽ được thực thi đầu tiên vì PayerID không tồn tại
                    // nó được trả về bởi lệnh gọi hàm tạo của lớp thanh toán
                    // Tạo thanh toán
                    // baseURL là url mà paypal gửi lại dữ liệu. 
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Cart/PaymentWithPayPal?";
                    // ở đây chúng tôi đang tạo hướng dẫn lưu trữ PaymentID nhận được trong phiên
                    // sẽ được sử dụng trong quá trình thực hiện thanh toán 
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //Hàm CreatePayment cung cấp cho chúng ta url phê duyệt thanh toán
                    //người trả tiền được chuyển hướng để thanh toán tài khoản paypal 
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    // nhận các liên kết được trả về từ paypal để đáp lại lệnh gọi hàm Tạo
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            // lưu URL chuyển hướng payapal tới nơi người dùng sẽ được chuyển hướng để thanh toán
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // lưu PaymentID trong hướng dẫn chính
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // Hàm này thực thi sau khi nhận được tất cả các tham số cho việc thanh toán
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    // Nếu thanh toán được thực hiện không thành công thì chúng tôi sẽ hiển thị thông báo thanh toán không thành công cho người dùng
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }

            //on successful payment, show success page to user.

            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var dao = new UserDao();
            var User = dao.GetById(session.UserName);

            try
            {

                if (ModelState.IsValid)
                {
                    List<CartItem> ShoppingCart = Session[CartSession] as List<CartItem>;
                    using (var context = new OnlineShopDbConext())
                    {
                        // Tạo đơn hàng mới
                        var order = new Order
                        {
                            CustomerID = User.ID,
                            ShipName = User.Name,
                            CreatedDate = DateTime.Now,
                            ShipMobile = User.Phone,
                            ShipEmail = User.Email,
                            ShipAddress = User.Address,
                            TotalPrice = ShoppingCart.Sum(x => x.TotalPrice),
                            Status = 2,
                            TypePayment = 2
                        };

                        // Lưu đơn hàng vào cơ sở dữ liệu
                        context.Orders.Add(order);
                        context.SaveChanges();

                       

                        // Lấy giỏ hàng từ Session


                        // Lưu chi tiết đơn hàng từ giỏ hàng vào cơ sở dữ liệu
                        foreach (var cartItem in ShoppingCart)
                        {
                            var orderDetail = new OrderDetail
                            {
                                OrderID = order.ID,
                                ProductID = cartItem.Product.ID,
                                Price = cartItem.Product.PromotionPrice ?? cartItem.Product.Pice,
                                Quantity = cartItem.Quantity,
                                NameProduct = cartItem.Product.Name,
                            };
                            context.OrderDetails.Add(orderDetail);

                            var product = context.Products.Find(cartItem.Product.ID);
                            if (product != null)
                            {
                                product.Quantity -= cartItem.Quantity;
                            }
                        }

                        context.SaveChanges();
                        Session[CartSession] = null;

                        RedirectToAction("SuccessView", "Cart");
                    }
                }
            }
            catch
            {
                return Content("Xảy ra lỗi, mời bạn kiểm tra lại thông tin đã điền");

            }

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

            List<CartItem> Cart = Session[CartSession] as List<CartItem>;
            //tạo danh sách vật phẩm và thêm các đối tượng vật phẩm vào đó
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            foreach (var item in Cart)
            {
                itemList.items.Add(new Item()
                {
                    name = item.Product.Name,
                    currency = "USD",
                    price = Math.Round((double)(item.Product.PromotionPrice ?? item.Product.Pice) / 24000, 2).ToString(),
                    quantity = item.Quantity.ToString(),
                    sku = item.Product.ID.ToString()
                });
            }
            // Thêm thông tin chi tiết về mặt hàng như tên, tiền tệ, giá cả, v.v. 

            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Định cấu hình Url chuyển hướng tại đây với đối tượng RedirectUrls
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Thêm chi tiết về Thuế, vận chuyển và Tổng phụ
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = Cart.Sum(x => Math.Round(x.TotalPrice / 24000, 2)).ToString()
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = Cart.Sum(x => Math.Round(x.TotalPrice / 24000, 2)).ToString(), // Tổng số phải bằng tổng thuế, phí vận chuyển và tổng phụ.
                details = details
            };
            var transactionList = new List<Transaction>();
            // Thêm mô tả về giao dịch
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Tạo hóa đơn số  
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
            // Tạo thanh toán bằng APIContext
            return this.payment.Create(apiContext);
        }



        public ActionResult SuccessOCD()
        {
            return View();  // Hiển thị trang xác nhận đặt hàng
        }


    }
}