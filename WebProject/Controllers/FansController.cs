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
    public class FansController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Fans
        public ActionResult Index()
        {
            return View(db.fans.ToList());
        }

        // GET: Search fans by parameters
        public ActionResult SearchFans(DateTime? birthday, string firstName, string lastName, int pazam)
        {
            IEnumerable<Fan> fanQuery =
                        from fan in db.fans.ToList()
                        where (!birthday.HasValue || fan.BirthDay.CompareTo(birthday.Value) > 0) &&
                              (firstName.Equals("") || fan.FirstName.Contains(firstName)) &&
                              (lastName.Equals("") || fan.LastName.Contains(lastName)) &&
                              fan.PazamInClub >= pazam
                        select fan;

            return View(fanQuery.ToList());
        }

        // GET: Fans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fan fan = db.fans.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        // GET: Fans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Gender,BirthDay,PazamInClub")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                // Get the user from the database, and set the fan's userID to it.
                ApplicationUser user = db.Users.First(usr => usr.UserName.Equals(User.Identity.Name));
                fan.UserID = user.Id;

                db.fans.Add(fan);
                db.SaveChanges();

                // Now that the fan has been saved, it has an ID.
                // So set the user's FanID to it.
                user.FanID = fan.ID;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index","Home");
            }

            return View(fan);
        }

        // GET: Fans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fan fan = db.fans.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        // POST: Fans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Gender,BirthDay,PazamInClub")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fan);
        }

        // GET: Fans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            db.SaveChanges();

            Fan fan = db.fans.Find(id);
            if (fan == null)
            {
                return HttpNotFound();
            }
            return View(fan);
        }

        // POST: Fans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fan fan = db.fans.Find(id);
            db.fans.Remove(fan);
            db.SaveChanges();

            // TODO: Remove the AspUser row from the database.

            return RedirectToAction("Index");
        }

        /*      private void DeleteAllCommentsForFan(int FanID)
                {
                    foreach (Comment c in db.Comments.Where(c => c.FanID.Equals(FanID)))
                    {
                        Comment c2 = db.Comments.Find(c.ID);
                        c2.FanID = UNKNOWN_ID;
        //                db.Comments.Remove(c2);
                    }

                    db.SaveChanges();
                }
        

        private void DeleteAllPostsForFan(int FanID)
        {
            foreach (Post p in db.Posts.Where(p => p.FanID.Equals(FanID)))
            {
                // First delete all the comments for this post
 //               DeleteAllCommentsForPost(p.ID, FanID);

                Post p2 = db.Posts.Find(p.ID);

                db.Posts.Remove(p2);
            }

            // TODO:
            db.SaveChanges();
        }

        private void DeleteAllCommentsForPost(int PostID, int FanID)
        {
            foreach (Comment c in db.Comments.Where(c => c.PostID.Equals(PostID)))
            {
                Comment c2 = db.Comments.Find(c.ID);

                // Make sure we dont delete this fan's comments again
//                if (c2.FanID != FanID)
                {
                    db.Comments.Remove(c2);
                }
            }

            // TODO:
            db.SaveChanges();
        }
        */
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
