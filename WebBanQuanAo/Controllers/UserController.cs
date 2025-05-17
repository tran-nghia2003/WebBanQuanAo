using Models.Dao;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Common;
using WebBanQuanAo.Models;
using System.Data.Entity;
using PagedList;
using System.Net.Mail;
using BotDetect.Web.Mvc;

namespace WebBanQuanAo.Controllers
{
    public class UserController : Controller
    {
        public string OTP { get; set; }
        public DateTime? OTPExpiry { get; set; }

        private OnlineShopDbConext db = new OnlineShopDbConext();
        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tài");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new User();
                    user.UserName = model.UserName;
                    user.Password = Common.Encryptor.MD5Hash(model.Password);
                    user.Name = model.Name;
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreateDate = DateTime.Now;
                    user.Status = true;
                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        model = new Register();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công");
                    }
                }
            }
            return View(model);
        }


        public ActionResult Logout()
        {
            Session[Common.CommonConstants.USER_SESSION] = null;
            return Redirect("/");

        }


        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCapcha", "Mã xác nhận không đúng!")]
        public ActionResult Login(LoginModel model)
        {

            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Common.Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    userSession.GroupID = user.GroupID;
                    var listCredentials = dao.GetListCredential(model.UserName);

                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredentials);

                    Session.Add(Common.CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại !");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa !");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng !");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không đúng!");
                }
            }
            return View();
        }

        public ActionResult DetailUser(string UserName)
        {
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new UserDao();
            var account = dao.GetById(UserName);
            return View(account);
        }

        [HttpGet]
        public ActionResult EditUser(string UserName)
        {
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dao = new UserDao();
            var account = dao.GetById(UserName);
            return View(account);
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            var dao = new UserDao();
            if (ModelState.IsValid)
            {
                var result = dao.Update(user);
                if (result)
                {

                    return RedirectToAction("DetailUser", "User", new { UserName = session.UserName });
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật user không thành công");
                }
            }
            return View(user);
        }

        public ActionResult HistoryOrder(int? page)
        {
            var session = (UserLogin)Session[Common.CommonConstants.USER_SESSION];
            var UserID = session.UserID;
            var iteams = db.Orders.Where(x => x.CustomerID == UserID).OrderByDescending(x => x.CreatedDate).ToList();

            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 5;

            return View(iteams.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult DetailOrderUser(int OrderID)
        {
            var dao = new UserDao();
            var iteams = dao.GetOrdersAndDetailsByOrderId(OrderID);
            return View(iteams);
        }

        public ActionResult OrderUser(long customerID)
        {
            var dao = new UserDao();
            var iteams = dao.GetOrdersAndDetailsByCustomerId(customerID);
            return View(iteams);
        }

        public void sendMail(string to)
        {
            Random rd = new Random();
            int number = rd.Next(100000, 999999);
            Session["numb"] = number;
            string from, pass, content;
            from = "vutuannguyen9998@gmail.com";
            pass = "jhvxrdauzfyjlpdp";
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(to.Trim());
            mail.From = new MailAddress(from);
            mail.Subject = "Website Fashion Shop";
            mail.Body = "Mã xác nhận của bạn là: " + number;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.Timeout = 10000;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(from, pass);
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public ActionResult ForgotPassword()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string maill)
        {
            if (maill == null)
            {
                ViewData["null"] = "Không được bỏ trống mail!";
                return View();
            }
            else
            {
                sendMail(maill);
                var data = db.Users.Where(s => s.Email.Equals(maill)).ToList();
                if (data.Count > 0)
                {
                    ViewData["Success"] = "Kiểm tra mail của bạn để xác nhận!";
                    return RedirectToAction("ResetPass", "User", new { id = data.FirstOrDefault().ID });
                }
                else
                {
                    ViewData["ErrorMail"] = "Mail không tồn tại!";
                    return View();
                }
            }
        }

        public ActionResult ResetPass(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User account = db.Users.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Posts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPass(int? id, string maill, string password, string respassword, string confirm)
        {
            User account = db.Users.Find(id);
            string k = Session["numb"].ToString();


            if (confirm != k)
            {
                ViewData["Error1"] = "Mã xác nhận không chính xác!";
                return View(account);
            }
            else if (password != respassword)
            {
                ViewData["Error2"] = "Mật khẩu và không khớp!";
                return View(account);
            }
            else
            {
                account.Password = Common.Encryptor.MD5Hash(password);
                db.SaveChanges();
                return RedirectToAction("Login", "User");
            }

        }

         public ActionResult SendOTP()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendOTP(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Email không được để trống");
                return View();
            }

            var user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email không tồn tại");
                return View();
            }

            var otp = GenerateOTP();
            OTP = otp;
            OTPExpiry = DateTime.Now.AddMinutes(5);
            db.SaveChanges();

            SendEmail(email, otp);

            TempData["UserEmail"] = email;
            TempData["OTP"] = OTP;
            TempData["OTPExpiry"] = OTPExpiry;
            return RedirectToAction("VerifyOTP", "User");
        }

        public ActionResult VerifyOTP()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerifyOTP(string otp)
        {
            var email = TempData["UserEmail"] as string;
            var OPT = TempData["OTP"] as string;
            var storedOTPExpiry = TempData["OTPExpiry"] as DateTime?;


            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("SendOTP", "User");
            }

            var user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || OPT != otp || DateTime.Now > storedOTPExpiry)
            {
                ModelState.AddModelError("", "OTP không hợp lệ hoặc đã hết hạn");
                return View();
            }

            // OTP hợp lệ, đăng nhập thành công
            var dao = new UserDao();
            
            var userSession = new UserLogin();
            userSession.UserName = user.UserName;
            userSession.UserID = user.ID;
            userSession.GroupID = user.GroupID;
            var listCredentials = dao.GetListCredential(user.UserName);

            Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredentials);

            Session.Add(Common.CommonConstants.USER_SESSION, userSession);

            return RedirectToAction("Index", "Home");
        }

        private string GenerateOTP()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private void SendEmail(string email, string otp)
        {
            using (var message = new MailMessage("vutuannguyen9998@gmail.com", email))
            {
                message.Subject = "Your OTP Code";
                message.Body = $"Your OTP code is {otp}";
                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential("vutuannguyen9998@gmail.com", "jhvxrdauzfyjlpdp");
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(message);
                }
            }
        }
    }
}