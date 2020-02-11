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
    public class ProductController : AdminAccessController
    {

        IRepository<Product> repo = new ProductRepository(new BuyOnlineEntities());
        IRepository<Category> repocat = new CategoryRepository(new BuyOnlineEntities());
        IRepository<Comment> comrepo = new CommentRepository(new BuyOnlineEntities());

        public ActionResult Index()
        {
            return View(repo.GetAll());
        }

        [HttpGet]
        public ActionResult SoldOut()
        {
            return View(repo.GetAll().Where(c=>c.Unit<1).ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost,ActionName("Create")]
        public ActionResult FinalCreate(FormCollection form, HttpPostedFileBase Image, Product pr)
        {
            if(ModelState.IsValid)
            {
                int unitl = (int)pr.Unit;
                int unitpricel = (int)pr.UnitPrice;
                int catid = (int)pr.CategoryId;
                Category c = repocat.GetById((int)pr.CategoryId);
                TempData["Category"] = c.Title;

                string pic = System.IO.Path.GetFileName(Image.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Image"), pic);
                Image.SaveAs(path);
                TempData["Image"] = pic.ToString();

                repo.Insert(new Product
                {
                    Title = form["Title"].ToString(),
                    Details = form["Details"].ToString(),
                    Unit = unitl,
                    UnitPrice = unitpricel,
                    Image = TempData["Image"].ToString(),
                    CategoryId = catid,
                    Category = TempData["Category"].ToString()
                });
                return RedirectToAction("Index");
            }
            else
            {
                return View("Create");
            }
            
        } 

        public ActionResult Details(int id)
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
            Product p = repo.GetById(id);
            return View(p);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Product p = repo.GetById((int)id);
            return View(p);
        }

        [HttpPost,ActionName("Edit")]
        public ActionResult FinalEdit(FormCollection form, HttpPostedFileBase Image,int id, int? Unit, int UnitPrice)
        {
            if(Image == null)
            {
                Product p = repo.GetById(id);
                p.Title = form["Title"];
                if(Unit!=null)
                {
                    p.Unit = p.Unit + (int)Unit;
                }
                p.UnitPrice = UnitPrice;
                p.Details = form["Details"];
                repo.Update(p);
                return RedirectToAction("Index");
            }

            else
            {
                string pic = System.IO.Path.GetFileName(Image.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Image"), pic);
                Image.SaveAs(path);

                Product p = repo.GetById(id);
                p.Title = form["Title"];
                if (Unit!=null)
                {
                    p.Unit = p.Unit + (int)Unit;
                }
                p.UnitPrice = UnitPrice;
                p.Details = form["Details"];
                p.Image = pic.ToString();
                repo.Update(p);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            Product p = repo.GetById((int)id);
            return View(p);
        }

        [HttpPost,ActionName("Delete")]
        public ActionResult FinalDelete(int? id)
        {
            repo.Delete((int)id);
            return RedirectToAction("Index");
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