using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.Fan);

            return View(posts.ToList());
        }

        // GET: Search posts by parameters
        public ActionResult SearchPosts(DateTime? date, string bloggerName, string textFromComments, int minCommentsPerPost)
        {
            IEnumerable<Post> postQuery =
                        from post in db.Posts.Include(p => p.Fan).ToList()
                        where (!date.HasValue || post.Date.CompareTo(date.Value) > 0) &&
                              (bloggerName.Equals("") || post.Fan.FirstName.Contains(bloggerName) || post.Fan.LastName.Contains(bloggerName)) &&
                              ((minCommentsPerPost.Equals(0)) || (post.Comments != null && post.Comments.Count >= minCommentsPerPost)) &&
                              (textFromComments.Equals("") || post.Comments.Exists(comment => comment.Text.Contains(textFromComments)))
                        select post;

            return View(postQuery.ToList());
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Include(p => p.Fan).First(p => p.ID == id.Value);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            setSelectLists();
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "ID,Title,FanID,SiteAddress,Text,Picture,Video")] Post post)
        {
            if (ModelState.IsValid)
            {
                // Create the post's date according to the creation time.
                post.Date = DateTime.Now;
                
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            setSelectLists(post);
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Include(p => p.Fan).First(p => p.ID == id.Value);
            if (post == null)
            {
                return HttpNotFound();
            }

            setSelectLists(post);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Title,FanID,SiteAddress,Text,Picture,Video")] Post post)
        {
            if (ModelState.IsValid)
            {
                // Update the date
                post.Date = DateTime.Now;

                // Refresh the comments
                post.Comments = post.Comments;

                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            setSelectLists(post);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Include(p => p.Fan).First(p => p.ID == id.Value);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            // First delete all the comments for this post
            DeleteAllCommentsForPost(id);

            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
   
        private void DeleteAllCommentsForPost(int PostID)
        {
            foreach (Comment c in db.Comments.Where(c=>c.PostID.Equals(PostID)))
            {
                Comment c2 = db.Comments.Find(c.ID);
                db.Comments.Remove(c2);
            }
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void setSelectLists()
        {
            int MyFanID = int.Parse(((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst("FanID").Value);

            var fans = db.fans.ToList().Where(fan => fan.ID.Equals(MyFanID))
                         .Select(fan => new
                         {
                             FanID = fan.ID,
                             FullName = string.Format("{0} {1} ({2})", fan.FirstName, fan.LastName, fan.Gender.ToString())
                         });

            ViewBag.FanID = new SelectList(fans, "FanID", "FullName");
        }

        private void setSelectLists(Post post)
        {
            var fans = db.fans.Where(fan => fan.ID == post.FanID).ToList()
                         .Select(fan => new
                         {
                             FanID = fan.ID,
                             FullName = string.Format("{0} {1} ({2})", fan.FirstName, fan.LastName, fan.Gender.ToString())
                         });

            ViewBag.FanID = new SelectList(fans, "FanID", "FullName", post.FanID);
        }
    }
}