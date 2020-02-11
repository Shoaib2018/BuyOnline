using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuyOnline.Controllers
{
    public class LogoutController : Controller
    {
        //
        // GET: /Default1/
        public ActionResult Index()
        {
            Session["customerid"] = "";
            Session["customername"] = "";
            Session["adminid"] = "";
            Session["adminname"] = "";
            return RedirectToAction("Index", "Home");
        }
	}
}