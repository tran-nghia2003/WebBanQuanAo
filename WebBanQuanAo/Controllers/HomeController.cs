using Models.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo.Common;
using WebBanQuanAo.Models;

namespace WebBanQuanAo.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly]
        public PartialViewResult MainMenu()
        {
            var model = new MenuDao().ListByGroupId(1);
            return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult Footer()
        {
            var model = new FooterDao().GetFooter();
            return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult Slide()
        {
            var model = new SlideDao().ListAll();
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult NewProduct()
        {
            var model = new ProductDao().ListNewProduct(4);
            return PartialView(model);
        }
        [ChildActionOnly]
        public PartialViewResult HotProduct()
        {
            var model = new ProductDao().ListFeatureProduct(4);
            return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CommonConstants.CartSession];
            var list = new List<CartItem>();
            if(cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }
    }
}