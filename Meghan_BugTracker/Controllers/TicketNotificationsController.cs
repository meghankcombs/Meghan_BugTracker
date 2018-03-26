using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Meghan_BugTracker.Models;
using Meghan_BugTracker.Helpers;
using Microsoft.AspNet.Identity;

namespace Meghan_BugTracker.Controllers
{
    [RequireHttps]
    [Authorize(Roles = "Developer")]
    public class TicketNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketsHelper ticketHelper = new TicketsHelper();

        // GET: TicketNotifications
        public ActionResult Index()
        {
            var ticketNotifications = db.TicketNotifications.Include(t => t.Ticket).Include(t => t.Recipient);
            return View(ticketNotifications.ToList());
        }

        [Authorize]
        public ActionResult MyIndex() //Shows all notifications by user
        {
            return View("Index", ticketHelper.ListUserNotifications(User.Identity.GetUserId()));
        }

        // GET: TicketNotifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotification ticketNotification = db.TicketNotifications.Find(id);
            if (ticketNotification == null)
            {
                return HttpNotFound();
            }
            return View(ticketNotification);
        }

        // GET: TicketNotifications/Create
        //public ActionResult Create()
        //{
        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
        //    return View();
        //}

        // POST: TicketNotifications/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,TicketId,UserId")] TicketNotification ticketNotification)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        db.TicketNotifications.Add(ticketNotification);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotification.TicketId);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketNotification.RecipientId);
        //    return View(ticketNotification);
        //}

        // GET: TicketNotifications/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TicketNotification ticketNotification = db.TicketNotifications.Find(id);
        //    if (ticketNotification == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotification.TicketId);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketNotification.UserId);
        //    return View(ticketNotification);
        //}

        // POST: TicketNotifications/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,TicketId,UserId")] TicketNotification ticketNotification)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(ticketNotification).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketNotification.TicketId);
        //    ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketNotification.UserId);
        //    return View(ticketNotification);
        //}

        // GET: TicketNotifications/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TicketNotification ticketNotification = db.TicketNotifications.Find(id);
        //    if (ticketNotification == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticketNotification);
        //}

        // POST: TicketNotifications/Delete/5
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketNotification ticketNotification = db.TicketNotifications.Find(id);
            db.TicketNotifications.Remove(ticketNotification);
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
    }
}
