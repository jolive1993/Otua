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
    public class MessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Messages
        public ActionResult Index()
        {
            return View(db.Messages.ToList());
        }
        public ActionResult MyMessages()
        {
            string userId = User.Identity.GetUserId();
            List<Message> messages = db.Messages.Where(m => m.Recp == userId).ToList();
            messages.OrderBy(m => m.Date);
            return View("Index", messages);
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            message = FindRoot(message.Id);
            List<Message> children = FindChildren(message.Id);
            if (children.Count() > 0)
            {
                children.Insert(0, message);
                if (User.Identity.GetUserId() == children.Last().Recp)
                {
                    children.Last().Status = 1;
                    db.Entry(children.Last()).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return View("Conversation", children);
            }
            else
            {
                if (User.Identity.GetUserId() == message.Recp)
                {
                    message.Status = 1;
                    db.Entry(message).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return View(message);
            }
        }

        // GET: Messages/Create
        public ActionResult Create(int id)
        {
            TempData["requestid"] = id;
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Content,Author,AuthorUn,Recp,RecpUn,Date,RequestID,Status,ParentID,Subject")] Message message)
        {
            int requestId = (int)TempData["requestid"];
            Request request = db.Requests.Find(requestId);
            message.Author = User.Identity.GetUserId();
            message.AuthorUn = User.Identity.GetUserName();
            message.Recp = request.Author;
            message.RecpUn = request.AuthorUn;
            message.Date = DateTime.Now;
            message.RequestID = requestId;
            message.Status = 0;
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("MyMessages");
            }

            return View(message);
        }
        public ActionResult Reply(int parentId)
        {
            TempData["parentId"] = parentId;
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reply([Bind(Include = "Id,Content,Author,AuthorUn,Recp,RecpUn,Date,RequestID,Status,ParentID,Subject")] Message message)
        {
            int parentId = (int)TempData["parentId"];
            Message Omessage = db.Messages.Find(parentId);
            message.Author = User.Identity.GetUserId();
            message.AuthorUn = User.Identity.GetUserName();
            message.Recp = Omessage.Author;
            message.RecpUn = Omessage.AuthorUn;
            message.Date = DateTime.Now;
            message.Status = 0;
            message.ParentID = parentId;
            message.RequestID = Omessage.RequestID;
            message.Subject = Omessage.Subject;
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("MyMessages");
            }

            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Content,Author,AuthorUn,Recp,RecpUn,Date,RequestID,Status,ParentID")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
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
        public List<Message> FindChildren(int messageId)
        {
            List<Message> messages = new List<Message>();
            int id = messageId;
            while (true)
            {
                List<Message> message = db.Messages.Where(m => m.ParentID == id).ToList();
                if (message.Count() > 0)
                {
                    messages.Add(message[0]);
                    id = message[0].Id;
                }
                else
                {
                    break;
                }
            }
            return messages;
        }
        [NonAction]
        public Message FindRoot(int messageId)
        {
            int? parent;
            Message message = db.Messages.Find(messageId);
            parent = message.ParentID;
            while (true)
            {
                if(parent != null)
                {
                    message = db.Messages.Find(parent);
                    parent = message.ParentID;
                }
                else
                {
                    break;
                }
            }
            return message;
        }
    }
}
