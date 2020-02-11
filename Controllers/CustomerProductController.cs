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
    public class CustomerProductController : CustomerAccessController
    {
        IRepository<Product> repo = new ProductRepository(new BuyOnlineEntities());
        IRepository<Comment> comrepo = new CommentRepository(new BuyOnlineEntities());
        // GET: /ProductForCustomer/
        public ActionResult Index()
        {
            return View(repo.GetAll());
        }

        public ActionResult Details(int? id)
        {
            int i = 0;
            var cat = comrepo.GetAll().Where(c => c.ProductId == (int)id).OrderByDescending(c => c.Id);
            int count = comrepo.GetAll().Where(c => c.ProductId == (int)id).Count();
            Session["countcomment"] = count;
            foreach (var item in cat)
            {
                i++;
                Session["comment" + i.ToString()] = item.Comment1;
                Session["c" + i.ToString()] = item.CustomerName;
            }
            return View(repo.GetById((int)id));
        }

        [HttpGet]
        public ActionResult ProductByCategory(int? cid)
        {
            if (cid != null)
            {
                return View(repo.GetAll().Where(p => p.CategoryId == (int)cid).ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ViewResult SearchProduct(string productname)
        {
            System.Threading.Thread.Sleep(2000);
            var data = repo.GetAll().Where(p => p.Title.StartsWith(productname) || p.Title.StartsWith(productname.ToLower()) || p.Title.StartsWith(productname.ToUpper())).ToList();
            return View(data);
        }

        public ActionResult SearchAuto(string term)
        {
            var names = repo.GetAll().Where(p => p.Title.Contains(term) ||  p.Title.Contains(term.ToLower()) || p.Title.Contains(term.ToUpper())).Select(p => p.Title).ToList();
            return Json(names, JsonRequestBehavior.AllowGet);
        }
	}
}