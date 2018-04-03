using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Meghan_BugTracker.Models;
using Microsoft.AspNet.Identity;
using Meghan_BugTracker.Helpers;

namespace Meghan_BugTracker.Controllers
{
    [RequireHttps]
    public class TicketCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketsHelper ticketHelper = new TicketsHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();

        // GET: TicketComments
        [Authorize]
        public ActionResult Index()
        {
            var ticketComments = db.TicketComments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketComments.ToList());
        }

        //Comment for specific ticket
        [Authorize]
        public ActionResult TicketCommentIndex(int ticketId)
        {
            return View("Index", ticketHelper.ListTicketsComments(ticketId));
        }

        //Comments for just PMs
        [Authorize]
        public ActionResult PMTicketComments()
        {
            var ticketComments = new List<TicketComment>(); //empty list of ticket comments
            var projects = projectHelper.ListUserProjects(User.Identity.GetUserId()); //get projects this user is on
            foreach (var project in projects) //loop through each project in the projects user is on
            {
                foreach (var ticket in project.Tickets) //loop through the tickets of each individual project user is on
                {
                    foreach (var comment in ticket.TicketComments) //loop through attachment of those tickets
                    {
                        ticketComments.Add(comment); //add to empty attachment list
                    }
                }
            }
            return View("Index", ticketComments);
        }

        //Comments for Devs and Subs
        [Authorize]
        public ActionResult DevSubTicketComments()
        {
            var ticketComments = new List<TicketComment>(); //empty list of ticket attachments
            var tickets = ticketHelper.ListUserTickets(User.Identity.GetUserId());
            foreach (var ticket in tickets) //loop through tickets user is assigned to
            {
                foreach (var comment in ticket.TicketComments) //loop through attachment of those tickets
                {
                    ticketComments.Add(comment); //add to empty attachment list
                }
            }
            return View("Index", ticketComments);
        }

        // GET: TicketComments/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.TicketComments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            return View(ticketComment);
        }

        //GET: TicketComments/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            ViewBag.TicketId = id;
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketComments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Comment,TicketId")] TicketComment ticketComment)
        {
            if (ModelState.IsValid)
            {
                ticketComment.UserId = User.Identity.GetUserId();
                ticketComment.Created = DateTime.Now;
                db.TicketComments.Add(ticketComment);
                db.SaveChanges();
                ViewBag.PreviousUrl = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
                return Redirect(ViewBag.PreviousUrl);
            }

            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketComment.UserId);
            return View(ticketComment);
        }

        // GET: TicketComments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComment ticketComment = db.TicketComments.Find(id);
            if (ticketComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReturnUrl = Request.UrlReferrer;
            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketComment.UserId);
            return View(ticketComment);
        }

        // POST: TicketComments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,UserId,Created,Comment")] TicketComment ticketComment, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketComment).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(returnUrl);
            }
            return View(ticketComment);
        }

        // GET: TicketComments/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TicketComment ticketComment = db.TicketComments.Find(id);
        //    if (ticketComment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticketComment);
        //}

        // POST: TicketComments/Delete/5
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketComment ticketComment = db.TicketComments.Find(id);
            db.TicketComments.Remove(ticketComment);
            db.SaveChanges();
            ViewBag.PreviousUrl = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            return Redirect(ViewBag.PreviousUrl);
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
