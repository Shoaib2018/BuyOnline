using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BuyOnline.Interface;
using BuyOnline.Models;
using BuyOnline.Repository;

namespace BuyOnline.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Product> repo = new ProductRepository(new BuyOnlineEntities());
        IRepository<Category> catrepo = new CategoryRepository(new BuyOnlineEntities());
        IRepository<Comment> comrepo = new CommentRepository(new BuyOnlineEntities());

        public ActionResult Index()
        {
            int i = 0;
            var cat = catrepo.GetAll().OrderBy(c => c.Title);
            int count = catrepo.GetAll().Count();
            Session["countcat"] = count;
            foreach (var item in cat)
            {
                i++;
                Session["id" + i.ToString()] = item.Id;
                Session["title" + i.ToString()] = item.Title;
            }
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
            var names = repo.GetAll().Where(p => p.Title.Contains(term) || p.Title.Contains(term.ToLower()) || p.Title.Contains(term.ToUpper())).Select(p => p.Title).ToList();
            return Json(names, JsonRequestBehavior.AllowGet);
        }
    }
}