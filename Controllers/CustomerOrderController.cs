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
    public class CustomerOrderController : CustomerAccessController
    {
        IRepository<Order> repo = new OrderRepository(new BuyOnlineEntities());
        IRepository<Cart> cartrepo = new CartRepository(new BuyOnlineEntities());
        IRepository<Product> prorepo = new ProductRepository(new BuyOnlineEntities());
        IRepository<Customer> cusrepo = new CustomerRepository(new BuyOnlineEntities());
        IRepository<Notification> notrepo = new NotificationRepository(new BuyOnlineEntities());
        // GET: /CustomerOrder/
        public ActionResult Index()
        {
            return View(repo.GetAll().Where(o => o.CustomerId == (int)Session["customerid"]));
        }

        [HttpGet]
        public ActionResult PlaceOrder()
        {
            int? total = cartrepo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.OrderId == null).Sum(c => c.TotatlPrice);
            TempData["totalammount"] = total;
            var customer = cusrepo.GetAll().Where(c => c.Id == (int)Session["customerid"]).Select(c => c.Discount);
            foreach (var cus in customer)
            {
                if (cus != null)
                {
                    int? discountrate = (int)cus.Rate;
                    TempData["discountrate"] = discountrate;
                    int? discount = ((int)total / 100) * (int)discountrate;
                    TempData["discount"] = discount;
                    int finalammount = (int)total - (int)discount;
                    TempData["finalammount"] = finalammount;
                }
                else
                {
                    TempData["discountrate"] = 0;
                    TempData["discount"] = 0;
                    TempData["finalammount"] = total;
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult ThankYou()
        {
            return View();
        }


        [HttpPost,ActionName("PlaceOrder")]
        public ActionResult OrderPlace(FormCollection form, DateTime? DeliveryDay)
        {
            if(ModelState.IsValid)
            {
                int fammount = Convert.ToInt32(Request.Params["FinalAmmount"]);
                int tammount = Convert.ToInt32(Request.Params["TotalAmmount"]);
                int dammount = Convert.ToInt32(Request.Params["Discount"]);
                repo.Insert(new Order { PlacedDay = DateTime.Now, DeliveryDay = DeliveryDay, DeliveryTime = form["DeliveryTime"], Ammount = (int)tammount, Discount = (int)dammount, FinalAmmount = (int)fammount, CustomerId = (int)Session["customerid"], CustomerName = Session["customername"].ToString(), Status = "Pending" });
                var lastinsertedId = repo.GetAll().Select(o => o.Id).Last();
                var c = cartrepo.GetAll().Where(u => u.CustomerId == (int)Session["customerid"] && u.OrderId == null);
                foreach (var item in c)
                {
                    item.OrderId = lastinsertedId;
                    cartrepo.Update(item);
                }
                notrepo.Insert(new Notification { Notice = "Your Order Has Benn Placed!Happy Shopping!", CustomerName = Session["customername"].ToString(), CustomerId = (int)Session["customerid"], DateTime = DateTime.Now, OrderId = Convert.ToInt32(lastinsertedId) });
                return RedirectToAction("ThankYou");
            }
            else
            {
                return View("PlaceOrder");
            }
            
        }

        public ActionResult Details(int? id)
        {
            if(id!=null)
            {
                var cart = cartrepo.GetAll().Where(c => c.OrderId == (int)id);
                return View(cart);
            }
            else
            {
                return RedirectToAction("Index","ProductForCustomer");
            }
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if(id!=null)
            {
                var order = repo.GetById((int)id);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "ProductForCustomer");
            }
            
        }

        [HttpPost,ActionName("Delete")]

        public ActionResult FinalDelete(int? id)
        {
            if (id != null)
            {
                var cartid = cartrepo.GetAll().Where(c => c.OrderId == (int)id).Select(c => c.Id);
                var nid = notrepo.GetAll().Where(n => n.OrderId == (int)id).Select(n=>n.Id);
                foreach (var itemid in nid)
                {
                    notrepo.Delete((int)itemid);
                }
                var pro = cartrepo.GetAll().Where(c => c.OrderId == (int)id).Select(y => new Cart
                   {
                       ProductId = y.ProductId,
                       Quantity = y.Quantity
                   }).ToList();
                foreach (var cid in cartid)
                {
                    cartrepo.Delete((int)cid);
                }

                foreach (var item in pro)
                {
                    var product = prorepo.GetById(item.ProductId);
                    product.Unit = product.Unit + (int)item.Quantity;
                    prorepo.Update(product);
                }
                repo.Delete((int)id);
                
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "ProductForCustomer");
            }
        }
	}
}