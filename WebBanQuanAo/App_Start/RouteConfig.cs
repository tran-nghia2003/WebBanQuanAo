using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebBanQuanAo
{
    public class RouteConfig
    {
        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*botdetect}",
      new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
               name: "Product Detail",
               url: "chi-tiet/{metatitle}-{id}",
               defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional }


           );

            routes.MapRoute(
              name: "Product Category",
              url: "san-pham/{metatitle}-{cateId}",
              defaults: new { controller = "Product", action = "PageProduct", id = UrlParameter.Optional }


          );

            routes.MapRoute(
              name: "Add Cart",
              url: "them-gio-hang",
              defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional }


          );

            routes.MapRoute(
              name: "Register",
              url: "dang-ky",
              defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional }


          );

            routes.MapRoute(
                  name: "Login",
                  url: "dang-nhap",
                  defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional }


              );

            routes.MapRoute(
                  name: "Search",
                  url: "tim-kiem",
                  defaults: new { controller = "Product", action = "SearchPage", id = UrlParameter.Optional }


              );

            routes.MapRoute(
                  name: "SearchKeyword",
                  url: "Product/ListName",
                  defaults: new { controller = "Product", action = "ListName", id = UrlParameter.Optional }


              );

            routes.MapRoute(
                  name: "Home",
                  url: "trang-chu",
                  defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }


              );

            routes.MapRoute(
                  name: "Product",
                  url: "san-pham",
                  defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional }


              );

            routes.MapRoute(
                  name: "Contact",
                  url: "lien-he",
                  defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional }


              );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WebBanQuanAo.Controllers" }

            );


        }

    }
}
