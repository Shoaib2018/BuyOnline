using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BuyOnline.Repository;
using BuyOnline.Models;
using BuyOnline.Interface;

namespace BuyOnline.Controllers
{
    public class CustomerController : Controller
    {
        IRepository<Customer> repo = new CustomerRepository(new BuyOnlineEntities());
        // GET: /Customer/
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        public ActionResult SignedUp()
        {
            return View();
        }

        [HttpPost,ActionName("Registration")]
        public ActionResult SignUp(Customer p)
        {
            if (ModelState.IsValid)
            {
                if (repo.GetAll().Where(c => c.Email == p.Email.ToString()).FirstOrDefault() != null)
                {
                    ViewData["invalidregmsg"] = "This Email Got An Id";
                    return View("Registration");
                }
                if (repo.GetAll().Where(c => c.Phone == (int)p.Phone).FirstOrDefault() != null)
                {
                    ViewData["invalidregmsg"] = "This Phone Number Got An Id";
                    return View("Registration");
                }
                else
                {
                    repo.Insert(p);
                    return RedirectToAction("SignedUp");
                }               
            }
            else
            {
                return View("Registration");
            }
        }

	}
}