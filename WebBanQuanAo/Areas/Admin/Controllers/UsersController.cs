using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Models.EF;
using PagedList;
using PagedList.Mvc;
using WebBanQuanAo.Common;

namespace WebBanQuanAo.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        private OnlineShopDbConext db = new OnlineShopDbConext();

        // GET: Admin/Users
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(int? page)
        {
            int pageSize = 3; 
            int pageNumber = (page ?? 1); 

           
            var users = db.Users.OrderBy(u => u.ID).ToPagedList(pageNumber, pageSize);
            return View(users);
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Users/Create
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create([Bind(Include = "ID,UserName,Password,Address,Email,Phone,CreateDate,CreateBy,ModifierDate,ModifierBy,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                var encryptedMd5Pas = Encryptor.MD5Hash(user.Password);
                user.Password = encryptedMd5Pas;
                db.Users.Add(user);
                db.SaveChanges();
                ViewBag.SuccessMessage = "Thêm user thành công";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMessage = "Thêm user không thành công"; 
            }

            return View(user);
        }

        // GET: Admin/Users/Edit/5
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit([Bind(Include = "ID,UserName,Password,Address,Email,Phone,CreateDate,CreateBy,ModifierDate,ModifierBy,Status")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Admin/Users/Delete/5
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HasCredential(RoleID = "DELETE_USER")]
        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
