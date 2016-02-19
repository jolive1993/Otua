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
        public ActionResult GridIndex(int? page, string searchString, string currentFilter, string latlng)
        {
            double lat;
            double longt;
            if(latlng == null)
            {
                if (User.Identity.GetUserId() != null)
                {
                    string userId = User.Identity.GetUserId();
                    var profile = db.Profiles.Where(p => p.UserID == userId).ToList();
                    lat = Convert.ToDouble(profile[0].Lat);
                    longt = Convert.ToDouble(profile[0].Long);
                }
                else
                {
                    lat = 43.0389025;
                    longt = -87.90647360000003;
                }
            }
            else
            {
                string[] latngArray;
                latngArray = latlng.Split(',');
                lat = Convert.ToDouble(latngArray[0]);
                longt = Convert.ToDouble(latngArray[1]);
            }
            ViewBag.Lat = lat;
            ViewBag.Long = longt;
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
            SortedList<double, Request> sortedList = new SortedList<double, Models.Request>();
            foreach(Request r in list)
            {
                double key;
                double latdif;
                double longdif;
                latdif = Convert.ToDouble(r.Lat) - lat;
                longdif = Convert.ToDouble(r.Long) - longt;
                if(latdif < 0)
                {
                    latdif = latdif * (-1);
                }
                if(longdif < 0)
                {
                    longdif = longdif * (-1);
                }
                key = latdif + longdif;
                sortedList.Add(key, r);
            }
            var finalList = sortedList.Values.ToList();
            var pageNumber = page ?? 1;
            var onePage = finalList.ToPagedList(pageNumber, 10);
            ViewBag.onePage = onePage;
            return View();
        }
        [Authorize]
        public ActionResult MyRequests()
        {
            var userid = User.Identity.GetUserId();
            var list = db.Requests.Where(model => model.Author == userid && model.Status < 3).ToList();
            return View(list);
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
