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
    public class RequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Requests
        public ActionResult Index()
        {
            var list = db.Requests.Where(model => model.Status == 0).ToList();
            return View(list);
        }
        public ActionResult GridIndex()
        {
            var list = db.Requests.Where(model => model.Status == 0).ToList();
            return View(list);
        }
        public ActionResult MyRequests()
        {
            var userid = User.Identity.GetUserId();
            var list = db.Requests.Where(model => model.Author == userid).ToList();
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
        public ActionResult Create([Bind(Include = "Content,Tags,Title,Img")] Request request)
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
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Content,Tags,Title")] Request request)
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
            return View(request);
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
    }
}
