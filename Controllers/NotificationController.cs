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
    public class NotificationController : CustomerAccessController
    {
        IRepository<Notification> repo = new NotificationRepository(new BuyOnlineEntities());
        // GET: /Notification/
        public ActionResult Index()
        {
            return View(repo.GetAll().Where(n => n.CustomerId == (int)Session["customerid"]));
        }

        [HttpGet]
        public void OrderPlace(int oid)
        {
            repo.Insert(new Notification { Notice = "Your Order Has Benn Placed!Happy Shopping!", CustomerName = Session["customername"].ToString(), CustomerId = (int)Session["customerid"], DateTime = DateTime.Now, OrderId = Convert.ToInt32(oid) });
        }
	}
}