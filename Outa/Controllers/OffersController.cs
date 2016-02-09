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

namespace Outa.Controllers
{
    public class OffersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Offers
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var list = db.Offers.ToList();
            return View(list);
        }
        [Authorize]
        public ActionResult IndexByParent(int id)
        {
            var list = db.Offers.Where(model => model.o_Parent == id).ToList();
            return View(list);
        }
        [Authorize]
        public ActionResult Transactions()
        {
            var userid = User.Identity.GetUserId();
            var requests = db.Requests.Where(model => model.Author == userid).ToList();
            List<int> requestids = new List<int>();
            foreach (Request r in requests)
            {
                requestids.Add(r.Id);
            }
            var list = db.Offers.Where(model => requestids.Contains(model.o_Parent) && model.o_Status == 1).ToList();
            return View(list);
        }
        [Authorize]
        public ActionResult MyOffers()
        {
            var userid = User.Identity.GetUserId();
            var list = db.Offers.Where(model => model.o_Author == userid).ToList();
            List<Offer> sortedList = list.OrderBy(o => o.o_Status).ToList();
            return View(list);
        }

        // GET: Offers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // GET: Offers/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            TempData["parent"] = id;
            return View();
        }
        // POST: Offers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "o_Content,o_Author,o_AuthorUn,o_Date,o_Price,o_Parent")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                offer.o_AuthorUn = User.Identity.GetUserName();
                offer.o_Author = User.Identity.GetUserId();
                offer.o_Date = DateTime.Now;
                offer.o_Parent = (int)TempData["parent"];
                db.Offers.Add(offer);
                db.SaveChanges();
                return RedirectToAction("MyOffers");
            }

            return View(offer);
        }

        // GET: Offers/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            var user = User.Identity.GetUserId();
            if (offer.o_Author == user)
            {
                return View(offer);
            }
            else
            {
                return View("Forbidden");
            }
        }
        [Authorize]
        public ActionResult Accept(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            var request = db.Requests.Find(offer.o_Parent);
            if (request.Author == User.Identity.GetUserId())
            {
                request.Status = 1;
                offer.o_Status = 1;
                db.Entry(offer).State = EntityState.Modified;
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return View(offer);
            }
            return View("Forbidden");
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,o_Content,o_Author,o_AuthorUn,o_Date,o_Price,o_Parent")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                offer.o_AuthorUn = User.Identity.GetUserName();
                offer.o_Author = User.Identity.GetUserId();
                offer.o_Date = DateTime.Now;
                db.Entry(offer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyOffers");
            }
            return View(offer);
        }

        // GET: Offers/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            var user = User.Identity.GetUserId();
            if (offer.o_Author == user && offer.o_Status == 0)
            {
                return View(offer);
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Offer offer = db.Offers.Find(id);
            db.Offers.Remove(offer);
            db.SaveChanges();
            return RedirectToAction("MyOffers");
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
