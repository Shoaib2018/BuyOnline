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
    public class CategoryController : AdminAccessController
    {
        IRepository<Category> repo = new CategoryRepository(new BuyOnlineEntities());
        // GET: /Category/
        public ActionResult Index()
        {
            int i = 0;
            var cat = repo.GetAll().OrderBy(c => c.Title);
            int count = repo.GetAll().Count();
            Session["countcat"] = count;
            foreach (var item in cat)
            {
                i++;
                Session["id" + i.ToString()] = item.Id;
                Session["title" + i.ToString()] = item.Title;
            }
            return View(repo.GetAll().OrderBy(c=>c.Title));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost,ActionName("Create")]
        public ActionResult Create(Category c)
        {
            if (ModelState.IsValid)
            {
                repo.Insert(new Category { Title = c.Title });
                return RedirectToAction("Index");
            }
            else
            {
                return View("Create");
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(repo.GetById(id));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(repo.GetById(id));
        }

        [HttpPost, ActionName("edit")]
        public ActionResult FinalEdit(Category c)
        {

            if (ModelState.IsValid)
            {
                repo.Update(c);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Edit");
            }

        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View(repo.GetById(id));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleleConfirmed(int id)
        {
            repo.Delete(id);
            return RedirectToAction("Index");
        }
	}
}