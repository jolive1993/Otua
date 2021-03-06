﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Outa.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

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
            foreach(Offer o in list)
            {
                Request r = db.Requests.Find(o.o_Parent);
                if(User.Identity.GetUserId() == r.Author)
                {
                    o.ReadStatus = 1;
                    db.Entry(o).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
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
            sortedList.Reverse();
            return View(list);
        }
        public ActionResult CompletedOffers(string userid)
        {
            var list = db.Offers.Where(model => model.o_Author == userid && model.o_Status == 2).ToList();
            var sortedList = list.OrderByDescending(i => i.o_Date).Take(10).ToList();
            if(sortedList.Count > 0)
            {
                return PartialView(list);
            }
            else
            {
                return PartialView("Sad");
            }
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
                offer.ReadStatus = 0;
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
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                string phoneNumber = userManager.GetPhoneNumber(offer.o_Author);
                if(phoneNumber != null)
                {
                    Request req = db.Requests.Find(offer.o_Parent);
                    string message = string.Format("Your offer on {0} has been accepted", req.Title);
                    userManager.SendSms(offer.o_Author, message);
                }
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
        [NonAction]
        public string getRequestTitle(int itemID)
        {
            string title;
            using (var context = new Outa.Models.ApplicationDbContext())
            {
                Request request = context.Requests.Find(itemID);
                title = request.Title;
            }
            return title;
        }
    }
}
