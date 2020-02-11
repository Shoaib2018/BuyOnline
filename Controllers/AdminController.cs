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
    public class AdminController : AdminAccessController
    {
        IRepository<Admin> repo = new AdminRepository(new BuyOnlineEntities());
        IRepository<Product> prorepo = new ProductRepository(new BuyOnlineEntities());
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View(prorepo.GetAll());
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult SignedUp()
        {
            return View();
        }


        [HttpPost, ActionName("Create")]
        public ActionResult Signup(Admin a)
        {
            
            if (ModelState.IsValid)
            {
                if (repo.GetAll().Where(c => c.Email == a.Email.ToString()).FirstOrDefault() != null)
                {
                    ViewData["invalidaddmsg"] = "This Email Got An Id";
                    return View("Create");
                }
                if (repo.GetAll().Where(c => c.Phone == (int)a.Phone).FirstOrDefault() != null)
                {
                    ViewData["invalidaddmsg"] = "This Phone Number Got An Id";
                    return View("Create");
                }
                else 
                {
                    repo.Insert(a);
                    return RedirectToAction("SignedUp");
                }
                
            }
            else
            {
                return View("Create");
            }
        }

        
	}
}