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
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.ReferencedPost).Include(c => c.Fan);

            return View(comments.ToList());
        }

        // GET: Search comments by parameters
        public ActionResult SearchComments(DateTime? date, string commentedBy, string textFromComment, string title, string postTitle)
        {
            IEnumerable<Comment> commentQuery =
                        from comment in db.Comments.Include(c => c.Fan).ToList()
                        // Join every comment's post.
                        join post in db.Posts.ToList() on (comment.PostID) equals (post.ID)
                        where (!date.HasValue || comment.Date.CompareTo(date.Value) > 0) &&
                              (commentedBy.Equals("") || comment.Fan.FirstName.Contains(commentedBy) || comment.Fan.LastName.Contains(commentedBy)) &&
                              (title.Equals("") || comment.Title.Contains(title)) &&
                              (textFromComment.Equals("") || comment.Text.Contains(textFromComment)) &&
                              // Check if the post's title is the requested one.
                              (postTitle.Equals("") || post.Title.Contains(postTitle))
                        select comment;

            return View(commentQuery.ToList());
        }

        // GET: Count comments per post
        public ActionResult CommentsPerPost()
        {
            IEnumerable<CommentsPerPostCount> commentQuery =
                        (from comment in db.Comments.ToList()
                            // Join every comment's post.
                        join post in db.Posts.ToList() on (comment.PostID) equals (post.ID)
                        group comment by new
                        {
                            PostId = comment.PostID,
                            PostTitle = post.Title
                        }
                        into groupz
                        select new CommentsPerPostCount
                        {
                            PostID = groupz.Key.PostId,
                            PostTitle = groupz.Key.PostTitle,
                            Count = groupz.Count()
                        })
                        .OrderByDescending(cppc => cppc.Count);

            return View(commentQuery.ToList());
        }

        public ActionResult FromBlog(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int actualId = id.Value;

            var comments = db.Comments.Where(comment => comment.PostID.Equals(actualId)).Include(c => c.ReferencedPost).Include(c => c.Fan);
            return View(comments.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Where(c => c.ID.Equals(id.Value)).Include(c => c.ReferencedPost).Include(c => c.Fan).First();
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            //            ViewBag.PostID = new SelectList(db.Posts, "ID", "Title");
            //            ViewBag.FanID = new SelectList(db.fans, "ID", "FirstName");

            setSelectLists();

            return View();
        }

        // GET: Comments/AddCommentForPost/5
        public ActionResult AddCommentForPost(int? PostID)
        {
            if (PostID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int actualPostID = PostID.Value;

            // Create a new comment that will be filled with data 
            // and saved if the user
            Comment comment = new Comment();
            comment.PostID = actualPostID;

//            ViewBag.FanID = new SelectList(db.fans, "ID", "FirstName");
            var redirtectTo = setSelectLists();

            if (redirtectTo == null)
                return PartialView(comment);

            return redirtectTo;
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCommentForPost([Bind(Include = "ID,PostID,Title,FanID,SiteAddress,Text")] Comment comment, int? PostID)
        {
            if (PostID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int actualPostID = PostID.Value;

            if (ModelState.IsValid)
            {
                // Set the referenced post
//TODO:                comment.ReferencedPost = db.Posts.Find(actualPostID);
                comment.PostID = actualPostID;

                // TODO:
                comment.FanID = comment.FanID;

                // Set the date
                comment.Date = DateTime.Now;
                
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("SinglePost", "Home", new { id = actualPostID });
            }

//            ViewBag.PostID = new SelectList(db.Posts, "ID", "Title", comment.PostID);
//            ViewBag.FanID = new SelectList(db.fans, "ID", "FirstName", comment.FanID);

            setSelectLists(comment);

            return View(comment);
        }

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PostID,Title,FanID,SiteAddress,Text")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                // Set the referenced post
//TODO:                comment.ReferencedPost = db.Posts.Find(comment.PostID);

                // Set the date
                comment.Date = DateTime.Now;

                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

//            ViewBag.PostID = new SelectList(db.Posts, "ID", "Title", comment.PostID);
// TODO: FanID???            ViewBag.FanID = new SelectList(db.fans, "ID", "FirstName", comment.Fan);

            setSelectLists(comment);

            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }

//            ViewBag.PostID = new SelectList(db.Posts, "ID", "Title", comment.PostID);
//            ViewBag.FanID = new SelectList(db.fans, "ID", "FirstName", comment.FanID);

            setSelectLists(comment);

            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PostID,Title,FanID,SiteAddress,Text")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                // Set the referenced post
//TODO:                comment.ReferencedPost = db.Posts.Find(comment.PostID);

                // Set the date
                comment.Date = DateTime.Now;
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

//            ViewBag.PostID = new SelectList(db.Posts, "ID", "Title", comment.PostID);
//            ViewBag.FanID = new SelectList(db.fans, "ID", "FirstName", comment.FanID);

            setSelectLists(comment);

            return View(comment);
        }

        private ActionResult setSelectLists()
        {
            ViewBag.PostID = new SelectList(db.Posts, "ID", "Title");

            try
            {
                if (((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst("FanID") != null)
                {
                    int MyFanID = int.Parse(((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst("FanID").Value);

                    var fans = db.fans.ToList().Where(fan => fan.ID.Equals(MyFanID))
                                 .Select(fan => new
                                 {
                                     FanID = fan.ID,
                                     FullName = string.Format("{0} {1} ({2})", fan.FirstName, fan.LastName, fan.Gender.ToString())
                                 });

                    ViewBag.FanID = new SelectList(fans, "FanID", "FullName");

                    return null;
                }

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }
        }

        private void setSelectLists(Comment comment)
        {
            ViewBag.PostID = new SelectList(db.Posts, "ID", "Title", comment.PostID);

            var fans = db.fans.Where(fan => fan.ID == comment.FanID).ToList()
                         .Select(fan => new
                         {
                             FanID = fan.ID,
                             FullName = string.Format("{0} {1} ({2})", fan.FirstName, fan.LastName, fan.Gender.ToString())
                         });


            ViewBag.FanID = new SelectList(fans, "FanID", "FullName", comment.FanID);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Where(c => c.ID.Equals(id.Value)).Include(c => c.ReferencedPost).Include(c => c.Fan).First();
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
//            DeleteAll();

            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void DeleteAll()
        {
            foreach (Comment c in db.Comments)
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
    }
}
