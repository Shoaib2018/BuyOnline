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
    public class OrderController : AdminAccessController
    {
        IRepository<Order> repo = new OrderRepository(new BuyOnlineEntities());
        IRepository<Cart> catrepo = new CartRepository(new BuyOnlineEntities());
        IRepository<Customer> cusrepo = new CustomerRepository(new BuyOnlineEntities());
        public ActionResult Index()
        {
            return View(repo.GetAll().OrderByDescending(o=>o.PlacedDay));
        }

        [HttpGet]
        public ActionResult Details(int? id, int? cid)
        {
            int i = 0;
            var cat = catrepo.GetAll().Where(c=>c.OrderId == (int)id);
            int count = cat.Count();
            Session["countcart"] = count;
            foreach (var item in cat)
            {
                i++;
                Session["ocpid" + i.ToString()] = item.ProductId;
                Session["ocpt" + i.ToString()] = item.ProductTitle;
                Session["ocup" + i.ToString()] = item.UnitPrice;
                Session["ocq" + i.ToString()] = item.Quantity;
                Session["octp" + i.ToString()] = item.TotatlPrice;
            }

            Customer customer = cusrepo.GetById((int)cid);
            Session["customerid"] = customer.Id;
            Session["customername"] = customer.Name;
            Session["customercontact"] = customer.Phone;
            Session["customeraddressbook"] = customer.AddressBook;

            return View(repo.GetById((int)id));
        }

        [HttpGet]
        public ActionResult Ready(int? id)
        {
            Order o = repo.GetById((int)id);
            o.Status = "Ready For Shippment";
            repo.Update(o);
            return RedirectToAction("Index");            
        }

        [HttpGet]
        public ActionResult Delivered(int? id)
        {
            Order o = repo.GetById((int)id);
            o.Status = "Delivered";
            repo.Update(o);
            return RedirectToAction("Index");
        }
	}
}