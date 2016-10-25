//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web.Mvc;
//using WebProject.Models;
//using System.Data.Entity;

//namespace WebProject.Controllers
//{
//    public class HomeController : Controller
//    {
//        private ApplicationDbContext db = new ApplicationDbContext();

//        // GET: Posts
//        public ActionResult Index()
//        {
//            return View(db.Posts.Include(p => p.Fan).ToList());
//        }

//        public ActionResult SinglePost(int id)
//        { 
//            return View(db.Posts.Include(p => p.Fan).Single(post => post.ID.Equals(id)));
//        }

//        public ActionResult About()
//        {
//            ViewData["Message"] = "This is a web project by Ben Kiani and Naveh Ohana.";
//            return View();
//        }

//        public ActionResult Contact()
//        {
//            ViewData["Message"] = "Contact us!";
//            return View();
//        }
//        public ActionResult Graphs()
//        {
//            return View();
//        }
//        public ActionResult DataGraph1()
//        {
//            var result = from fan in db.fans
//                         join post in db.Posts on fan.ID equals post.FanID into FansPosts
//                         from fanPost in FansPosts
//                         group fanPost by fan.PazamInClub into FanGroupPost
//                         select new { PazamInClub = FanGroupPost.Key, Number_Of_Posts = FanGroupPost.Count() };

//            return Json(result, "application/json", JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult DataGraph2()
//        {
//            IEnumerable<dynamic> result = from fan in db.fans
//                                          join post in db.Posts on fan.ID equals post.FanID
//                                          select new
//                                          {
//                                              name = fan.FirstName + " " + fan.LastName,
//                                              postId = post.ID,
//                                              children = post.Comments.Select(comment => new { name = comment.Fan.FirstName + " " + comment.Fan.LastName })
//                                          };

//            // Key = postId
//            Dictionary<int, List<Object>> allPosts = new Dictionary<int, List<object>>();

//            foreach (var currentPost in result)
//            {
//                if (!allPosts.ContainsKey(currentPost.postId))
//                {
//                    allPosts.Add(currentPost.postId, new List<Object>());
//                }

//                allPosts[currentPost.postId].Add(currentPost.children);
//            }

//            List<dynamic> finalList = new List<dynamic>();

//            foreach (var currentPost in allPosts)
//            {
//                int postID = currentPost.Key;

//                Post post = db.Posts.First(p => p.ID.Equals(postID));
//                Fan fan = db.fans.First(f => f.ID.Equals(post.FanID));

//                finalList.Add(new { name = fan.FirstName +
//                                           " " +
//                                           fan.LastName,
//                                    children = currentPost.Value });
//            }

//            //var result = from post in db.Posts
//            //             select new
//            //             {
//            //                 name = post.Title,
//            //                 children = post.Comments.Select(comment => new { name = comment.Fan.FirstName + " " + comment.Fan.LastName })
//            //             };

//            return Json(new { name = "Root", children = result }, "application/json", JsonRequestBehavior.AllowGet);
//        }

//        public ActionResult Error()
//        {
//            return View();
//        }
//    }
//}

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
            IEnumerable<dynamic> result = from fan in db.fans
                                          join post in db.Posts on fan.ID equals post.FanID
                                          select new
                                          {
                                              name = fan.FirstName + " " + fan.LastName,
                                              postId = post.ID,
                                              children = post.Comments.Select(comment => new { name = comment.Fan.FirstName + " " + comment.Fan.LastName })
                                          };

            // Key = postId
            Dictionary<int, List<Object>> allPosts = new Dictionary<int, List<object>>();

            foreach (var currentPost in result)
            {
                if (!allPosts.ContainsKey(currentPost.postId))
                {
                    allPosts.Add(currentPost.postId, new List<Object>());
                }

                allPosts[currentPost.postId].Add(currentPost.children);
            }

            List<dynamic> finalList = new List<dynamic>();

            foreach (var currentPost in allPosts)
            {
                int postID = currentPost.Key;

                Post post = db.Posts.First(p => p.ID.Equals(postID));
                Fan fan = db.fans.First(f => f.ID.Equals(post.FanID));

                finalList.Add(new
                {
                    name = fan.FirstName +
                           " " +
                           fan.LastName,
                    children = currentPost.Value
                });
            }

            //var result = from post in db.Posts
            //             select new
            //             {
            //                 name = post.Title,
            //                 children = post.Comments.Select(comment => new { name = comment.Fan.FirstName + " " + comment.Fan.LastName })
            //             };

            return Json(new { name = "Root", children = result }, "application/json", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
