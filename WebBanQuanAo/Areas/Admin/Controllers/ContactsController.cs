using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace WebBanQuanAo.Areas.Admin.Controllers
{
    public class ContactsController : Controller
    {
        private OnlineShopDbConext db = new OnlineShopDbConext();
        // GET: Admin/Contacts
        public ActionResult Index(int? page)
        {
            var iteams = db.Feedbacks.OrderByDescending(x => x.CreateDate).ToList();

            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 4;
            return View(iteams.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approval(int? id)
        {

            Feedback feedback = db.Feedbacks.Find(id);
            feedback.Status = true;
            db.SaveChanges();
            return View(feedback);
        }

        public ActionResult SendMail(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        public ActionResult WriteMail(string to, string content)
        {
            string from, pass;
            from = "vutuannguyen9998@gmail.com";
            pass = "jhvxrdauzfyjlpdp";
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(to.Trim());
            mail.From = new MailAddress(from);
            mail.Subject = "Website Fashion Shop";
            mail.Body = content;

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
                @ViewBag.success = "Gửi thành công!";
            }
            catch (Exception)
            {
                throw;
            }
            return RedirectToAction("Index", "Contacts");
        }
    }
}