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
    public class CustomerLoginController : Controller
    {
        IRepository<Customer> repo = new CustomerRepository(new BuyOnlineEntities());

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost, ActionName("Login")]
        public ActionResult Signin(int? CustomerInput, string Password, FormCollection form)
        {
            if (repo.GetAll().Where(c => c.Phone == CustomerInput && c.Password == form["Password"] || c.Email == form["CustomerInput"].ToString() && c.Password == form["Password"].ToString()).FirstOrDefault() != null)
            {
                var cus = repo.GetAll().Where(c => c.Phone == CustomerInput && c.Password == form["Password"] || c.Email == form["CustomerInput"].ToString() && c.Password == form["Password"].ToString()).FirstOrDefault();
                        //repo.GetAll().Where(c => c.Phone == AdminInput && c.Password == form["Password"] || c.Email == form["AdminInput"].ToString() && c.Password == form["Password"].ToString()).FirstOrDefault(); 
                Session["customerid"] = cus.Id;
                Session["customername"] = cus.Name;
                return RedirectToAction("Index", "CustomerProduct");
            }
            else
            {
                ViewData["invalidlogin"] = "Invalid Login!Try Again";
                return View("Login");
            }
        }
	}
}