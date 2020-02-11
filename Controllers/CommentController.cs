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
    public class CommentController : CustomerAccessController
    {
        IRepository<Comment> repo = new CommentRepository(new BuyOnlineEntities());
        IRepository<Product> prorepo = new ProductRepository(new BuyOnlineEntities());

        public ActionResult Post(string comment,int? pid, string ur )
        {
            repo.Insert(new Comment { Comment1 = comment, ProductId = (int)pid, CustomerId = (int)Session["customerid"], CustomerName = Session["customername"].ToString() });
            return Redirect(ur);
        }

        public ActionResult Index()
        {
            return View();
        }
	}
}