using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebProject.Models;
using System.Data.Entity;

namespace WebProject.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            return View(db.Posts.Include(p => p.Fan).ToList());
        }

        public ActionResult SinglePost(int id)
        { 
            return View(db.Posts.Include(p => p.Fan).Single(post => post.ID.Equals(id)));
        }

        public ActionResult About()
        {
            ViewData["Message"] = "This is a web project by Ben Kiani and Naveh Ohana.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewData["Message"] = "Contact us!";
            return View();
        }
        public ActionResult Graphs()
        {
            return View();
        }
        public ActionResult DataGraph1()
        {
            var result = from fan in db.fans
                         join post in db.Posts on fan.ID equals post.FanID into FansPosts
                         from fanPost in FansPosts
                         group fanPost by fan.PazamInClub into FanGroupPost
                         select new { PazamInClub = FanGroupPost.Key, Number_Of_Posts = FanGroupPost.Count() };

            return Json(result, "application/json", JsonRequestBehavior.AllowGet);
        }
        public ActionResult DataGraph2()
        {
            var result = from post in db.Posts
                         select new { name = post.Title, size = 4 };

            return Json(result, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
