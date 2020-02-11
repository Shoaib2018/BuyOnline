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
    public class DiscountController : AdminAccessController
    {
        IRepository<Discount> repo = new DiscountRepository(new BuyOnlineEntities());
        IRepository<Customer> cusrepo = new CustomerRepository(new BuyOnlineEntities());
        public ActionResult Index()
        {
            return View(repo.GetAll());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost,ActionName("Create")]
        public ActionResult FinalCreate(Discount d)
        {
            repo.Insert(d);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            Discount d = repo.GetById((int)id);
            return View(d);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult FinalDelete(int? id)
        {
            var customer = cusrepo.GetAll().Where(c => c.DiscountId == (int)id);
            
            foreach (var item in customer)
            {
                //item.Discount = null;
                item.DiscountId = null;
                cusrepo.Update(item);
            }

            repo.Delete((int)id);
            return RedirectToAction("Index");
        }

	}
}