using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuyOnline.Controllers
{
    public class CustomerAccessController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string cusid = Session["customername"] as string;
            if (string.IsNullOrEmpty(cusid))
            {
                Response.Redirect("/Home");
            }
        }
	}
}