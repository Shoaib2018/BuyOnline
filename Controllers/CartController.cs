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
    public class CartController : CustomerAccessController
    {
        IRepository<Cart> repo = new CartRepository(new BuyOnlineEntities());
        IRepository<Product> prorepo = new ProductRepository(new BuyOnlineEntities());
        IRepository<Customer> cusrepo = new CustomerRepository(new BuyOnlineEntities());
        // GET: /Cart/
        public ActionResult Index()
        {
            int? total = repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.OrderId == null).Sum(c => c.TotatlPrice);
            TempData["totalammount"] = total;
            var customer = cusrepo.GetAll().Where(c=> c.Id == (int)Session["customerid"]).Select(c=>c.Discount);
            foreach(var cus in customer)
            {
                if(cus!=null)
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
            return View(repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.OrderId == null));
        }

        public ActionResult Increment(int? id, int?pid, int? unitprice, string title, string ur)
        {
            if (id != null)
            {
                if(prorepo.GetById((int)pid).Unit>0)
                {
                    if (repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.ProductId == pid && c.OrderId == null).FirstOrDefault() != null)
                    {
                        var cart = repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.ProductId == pid && c.OrderId == null).FirstOrDefault();
                        cart.Quantity = cart.Quantity + 1;
                        cart.TotatlPrice = cart.TotatlPrice + (int)unitprice;
                        repo.Update(cart);
                        
                        var product = prorepo.GetById((int)pid);
                        product.Unit = product.Unit - 1;
                        prorepo.Update(product);
                        return Redirect(ur);
                    }
                    else
                    {
                        repo.Insert(new Cart { ProductTitle = title.ToString(), Quantity = 1, ProductId = (int)pid, CustomerId = (int)Session["customerid"], TotatlPrice = (int)unitprice, UnitPrice = (int)unitprice });
                        
                        var product = prorepo.GetById((int)pid);
                        product.Unit = product.Unit - 1;
                        prorepo.Update(product);
                        return Redirect(ur);
                    }
                }
                else
                {
                    return Redirect(ur);
                }
            }
            else
            {
                return Redirect(ur);
            }
        }

        public ActionResult Decrement(int? id, int? pid, int? unitprice, string ur)
        {
            if (id != null)
            {
                if (repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.ProductId == pid && c.OrderId == null).FirstOrDefault() != null)
                {
                    var cart = repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.ProductId == pid && c.OrderId == null).FirstOrDefault();
                    cart.Quantity = cart.Quantity - 1;
                    cart.TotatlPrice = cart.TotatlPrice - (int)unitprice;
                    repo.Update(cart);
                    if(repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.ProductId == pid && c.OrderId == null).FirstOrDefault().Quantity==0)
                    {
                        //int cartid = repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.ProductId == id && c.OrderId == null).FirstOrDefault().Id;
                        repo.Delete((int)id);
                    }
                    var product = prorepo.GetById((int)pid);
                    product.Unit = product.Unit + 1;
                    prorepo.Update(product);
                    return Redirect(ur);
                }
                else
                {
                    return Redirect(ur);
                }
            }
            else
            {
                return Redirect(ur);
            }
        }


        public ActionResult Delete(int? id, int? pid, int? quantity, string ur)
        {
            if (id != null)
            {
                if (repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.ProductId == pid && c.OrderId == null).FirstOrDefault() != null)
                {
                    int cartid = repo.GetAll().Where(c => c.CustomerId == (int)Session["customerid"] && c.ProductId == pid && c.OrderId == null).FirstOrDefault().Id;
                    repo.Delete((int)cartid);
                    var product = prorepo.GetById((int)pid);
                    product.Unit = product.Unit + (int)quantity;
                    prorepo.Update(product);
                    return Redirect(ur);
                }
                else
                {
                    return Redirect(ur);
                }
            }
            else
            {
                return Redirect(ur);
            }    
        }
	}
}