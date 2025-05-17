using Models.Dao;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanQuanAo.Controllers
{
    public class ProductController : Controller
    {

        [ChildActionOnly]
        public PartialViewResult ProductCategory()
        {
            var model = new ProductCategoryDao().ListAll();
            return PartialView(model);
        }

        // GET: Product
        public ActionResult Index(int ?page)
        {
            var model = new ProductDao().ListProductAll();
            if (page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 4;

            return View(model.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult Detail(long id)
        {
            var product = new ProductDao().ViewDetail(id);
            ViewBag.Category = new ProductCategoryDao().ViewDetail(product.CategoryID.Value);
            ViewBag.RelatedProducts = new ProductDao().ListRelatedProduct(id);
            ViewBag.ProductImages = new ProductDao().ListImageProduct(product.Code);
            return View(product);
        }

        public ActionResult PageProduct(long cateId, int? page )
        {

            int pageSize = 4; 

            int pageNumber = page ?? 1;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            var category = new ProductCategoryDao().ViewDetail(cateId);
            ViewBag.Category = category;

            var model = new ProductDao().ListByCategoryId(cateId, pageNumber, pageSize);

            ViewBag.CategoryID = cateId;


            return View(model);
        }

        public ActionResult SearchPage(string KeyWord,int? page)
        {

            ViewBag.keyword = KeyWord;
            int pageSize = 4; 

            int pageNumber = page ?? 1;
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            var model = new ProductDao().SearchPage(KeyWord, pageNumber, pageSize);

            return View(model);
        }

        public JsonResult ListName(string q)
        {
            var data = new ProductDao().ListName(q);
            return Json(new
            {
                data = data,
                status = true
            },JsonRequestBehavior.AllowGet);
        }

    }

}