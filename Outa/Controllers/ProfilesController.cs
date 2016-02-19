using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Outa.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace Outa.Controllers
{
    public class ProfilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ObjectContext objectContext = ((IObjectContextAdapter) new ApplicationDbContext()).ObjectContext;

        // GET: Profiles
        public ActionResult Index()
        {
            return View(db.Profiles.ToList());
        }

        // GET: Profiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)

            {
                var userId = User.Identity.GetUserId();
                var profiles = db.Profiles.Where(model => model.UserID == userId).ToList();
                id = profiles[0].Id;
                if(id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }
        public ActionResult DetailsByUserId(string uid)
        {
            int? id;
            var profiles = db.Profiles.Where(model => model.UserID == uid).ToList();
            id = profiles[0].Id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View("Details", profile);
        }

        // GET: Profiles/Create
        [ChildActionOnly]
        public ActionResult ReviewsForUser(string id)
        {
            Profile profile = db.Profiles.Where(p => p.UserID == id).ToList()[0];
            List<Review> reviewList = new List<Review>();
            ObjectSet<Offer> offers = objectContext.CreateObjectSet<Offer>();
            ObjectSet<Transaction> transcations = objectContext.CreateObjectSet<Transaction>();
            ObjectSet<Review> reviews = objectContext.CreateObjectSet<Review>();
            var q =
                    (
                    from Offer in offers
                    join Transaction in transcations
                    on Offer.Id
                    equals Transaction.OfferId
                    join Review in reviews
                    on Transaction.Id
                    equals Review.TrnsactionId
                    where Offer.o_Author == id
                    select new
                    {
                        Id = Review.Id,
                        UserId = Review.UserId,
                        UserName = Review.UserName,
                        Content = Review.Content,
                        Rating = Review.Rating,
                        TrnsactionId = Review.TrnsactionId

                    });
            foreach (var item in q)
            {
                Review r = new Review();
                r.Id = item.Id;
                r.UserId = item.UserId;
                r.UserName = item.UserName;
                r.Content = item.Content;
                r.Rating = item.Rating;
                r.TrnsactionId = item.TrnsactionId;
                reviewList.Add(r);
            }
            if (reviewList.Count() > 0)
            {
                decimal? rating = (reviewList.Sum(r => r.Rating) / reviewList.Count());
                if (profile.Rating != rating)
                {
                    profile.Rating = rating;
                    db.Entry(profile).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return PartialView("_Reviews", reviewList);
        }
        public ActionResult Create()
        {
            var user = User.Identity.GetUserId();
            var profile = db.Profiles.Where(model => model.UserID == user).ToList();
            if (profile.Count < 1)
            {
                return View();
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserName,UserID,Lat,Img,Long,Description,Rating")] Profile profile)
        {
            Console.WriteLine(profile);
            if (ModelState.IsValid)
            {
                profile.UserID = User.Identity.GetUserId();
                profile.UserName = User.Identity.GetUserName();
                decimal rating = 0;
                profile.Rating = rating;
                db.Profiles.Add(profile);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(profile);
        }

        // GET: Profiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,UserID,Lat,Long,Description,Rating")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profile);
        }

        // GET: Profiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Profile profile = db.Profiles.Find(id);
            db.Profiles.Remove(profile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [NonAction]
        public string GetProfilePic(string id)
        {
            string imgUrl;
            Profile profile = db.Profiles.Where(p => p.UserID == id).ToList()[0];
            imgUrl = profile.Img;
            return imgUrl;
        }
    }
}
