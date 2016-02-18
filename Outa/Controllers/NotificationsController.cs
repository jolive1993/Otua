using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Outa.Models;
using Outa.ViewModels;
using Microsoft.AspNet.Identity;

namespace Outa.Controllers
{
    public class NotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Notifications
        public ActionResult Notifications(string userId)
        {
            _Notification notification = new _Notification();
            List<Request> requests = db.Requests.Where(r => r.Author == userId && r.Status == 0).ToList();
            List<int> requestIds = new List<int>();
            foreach (Request r in requests)
            {
                requestIds.Add(r.Id);
            }
            notification.Messages = db.Messages.Where(m => m.Recp == userId && m.Status == 0).ToList();
            notification.Offers = db.Offers.Where(o => requestIds.Contains(o.o_Parent) && o.ReadStatus == 0).ToList();
            return PartialView(notification);
        }
    }
}