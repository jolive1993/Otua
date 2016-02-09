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
using PagedList.Mvc;
using PagedList;

namespace Outa.Controllers
{
    public class RequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Requests
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var list = db.Requests.ToList();
            return View(list);
        }
        public ActionResult GridIndex(int? page, string searchString, string currentFilter)
        {
            List<Request> list;
            if (searchString != null)
            {
                list = db.Requests.Where(model => model.Tags.Contains(searchString)
                && model.Status == 0).ToList();
            }
            else
            {
                list = db.Requests.Where(model => model.Status == 0).ToList();
            }
            ViewBag.CurrentFilter = searchString;
            List<Request> sortedList = list.OrderBy(r => r.Id).ToList();
            sortedList.Reverse();
            var pageNumber = page ?? 1;
            var onePage = sortedList.ToPagedList(pageNumber, 10);
            ViewBag.onePage = onePage;
            return View();
        }
        [Authorize]
        public ActionResult MyRequests()
        {
            var userid = User.Identity.GetUserId();
            var list = db.Requests.Where(model => model.Author == userid).ToList();
            List<Request> sortedList = list.OrderBy(r => r.Status).ToList();
            return View(sortedList);
        }

        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Requests/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Content,Tags,Title,Img,Lat,Long")] Request request)
        {
            if (ModelState.IsValid)
            {
                request.Date = DateTime.Now;
                request.AuthorUn = User.Identity.GetUserName();
                request.Author = User.Identity.GetUserId();
                db.Requests.Add(request);
                db.SaveChanges();
                return RedirectToAction("GridIndex");
            }

            return View(request);
        }

        // GET: Requests/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            var user = User.Identity.GetUserId();
            if (request.Author == user)
            {
                return View(request);
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Content,Tags,Title,Lat,Long")] Request request)
        {
            if (ModelState.IsValid)
            {
                request.AuthorUn = User.Identity.GetUserName();
                request.Author = User.Identity.GetUserId();
                request.Date = DateTime.Now;
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GridIndex");
            }
            return View(request);
        }

        // GET: Requests/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            var user = User.Identity.GetUserId();
            if (request.Author == user || User.IsInRole("admin"))
            {
                return View(request);
            }
            else
            {
                return View("Forbidden");
            }
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
            db.SaveChanges();
            return RedirectToAction("GridIndex");
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
        public int getOfferCount(int itemID)
        {
            int numOffers;
            using (var context = new Outa.Models.ApplicationDbContext())
            {
                var q = String.Format("SELECT * FROM dbo.Offer WHERE o_Parent = {0}", itemID);
                var count = context.Offers.SqlQuery(q).ToList();
                numOffers = count.Count;
            }
            return numOffers;
        }
    }
}
