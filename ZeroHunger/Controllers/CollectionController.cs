using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using ZeroHunger.EF;
using ZeroHunger.Auth;

namespace ZeroHunger.Controllers
    
{
    [Logged]
    public class CollectionController : Controller
    {
        // GET: Collection
        public ActionResult ResDash()
        {
            var db = new ZeroHungerEntities();

            return View(db.Restaurents.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Restaurent model)
        {
            var db = new ZeroHungerEntities();
            db.Restaurents.Add(model);
            db.SaveChanges();
            return RedirectToAction("ResDash");
        }
        [AdminLogged]
        public ActionResult AdminDash() 
        {
            var db = new ZeroHungerEntities();

            return View(db.Restaurents.ToList());
        }
        [AdminLogged]
        public ActionResult Accept(int id ) 
        {
            ZeroHungerEntities db = new ZeroHungerEntities();
            List<Collection> cart = null;
            if (Session["cart"]==null)
            {
                cart= new List<Collection>();   
            }
            else
            {
                cart = (List<Collection>)Session["cart"];
            }
            var restaurent = db.Restaurents.Find(id);
            cart.Add(new Collection()

            {
                res_id= restaurent.id,
                res_name=restaurent.name,
                food_name=restaurent.food,
                quantity=1,
                expire=restaurent.quantity,

            }
                );
            Session["cart"] = cart;
            TempData["Msg"] = "Succuessfully Added";
            return RedirectToAction("AdminDash");
        }

        public ActionResult Reject()
        {
            return View();
        }
        [AdminLogged]
        public ActionResult AcceptedList() {

            var cart = (List<Collection>)Session["cart"];
            if (cart!=null)
            {
                return View(cart);
            }
            return RedirectToAction("AdminDash");
        }
        [HttpGet]
        [AdminLogged]
        public ActionResult Assign (int id)
        {
            var cart = (List<Collection>)Session["cart"];
            if (cart != null)
            {
                var item = cart.FirstOrDefault(c=>c.res_id==id);
                if (item != null)
                {
                    return View(item);
                }
                
            }
            return RedirectToAction("AcceptedList");
            
        }
        [HttpPost]
        [AdminLogged]
        public ActionResult Assign (Collection deliver) 
        {
            var cart = (List<Collection>)Session["cart"];
            if(cart != null)
            {
                var item = cart.FirstOrDefault(c=>c.res_id==deliver.res_id);
                if (item != null)
                {
                    item.res_id= deliver.res_id;
                    item.res_name= deliver.res_name;
                    item.food_name= deliver.food_name; 
                    item.quantity= deliver.quantity;
                    item.expire= deliver.expire;
                    item.emp_id= deliver.emp_id;
                    Session["cart"] = cart;
                    var db = new ZeroHungerEntities();
                    var dbitem = new Collection();
                    if (dbitem != null)
                    {
                        dbitem.res_id = item.res_id;
                        dbitem.res_name = item.res_name;
                        dbitem.food_name = item.food_name;
                        dbitem.quantity = item.quantity;
                        dbitem.expire = item.expire;
                       // dbitem.emp_name = emp.name;
                        dbitem.emp_id = item.emp_id;
                        dbitem.status = "Delivered";
                        dbitem.ordertime = DateTime.Now.ToString();
                        db.Collections.Add(dbitem);
                        db.SaveChanges();
                    }
                    return RedirectToAction("History");
                
                }

            }
            return RedirectToAction("AdminDash");
        }
        [AdminLogged]
        public ActionResult History() 
        {
            var db = new ZeroHungerEntities();

            return View(db.Collections.ToList());
            
        }
       
    }
}