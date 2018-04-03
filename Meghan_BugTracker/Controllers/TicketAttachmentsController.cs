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
using System.IO;
using Meghan_BugTracker.Helpers;
using System.Threading.Tasks;

namespace Meghan_BugTracker.Controllers
{
    [RequireHttps]
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketsHelper ticketHelper = new TicketsHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();

        // GET: TicketAttachments
        [Authorize]
        public ActionResult Index()
        {
            var ticketAttachments = db.TicketAttachments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketAttachments.ToList());
        }

        //Attachments on specific ticket
        [Authorize]
        public ActionResult TicketAttachmentIndex(int ticketId)
        {
            return View("Index", ticketHelper.ListTicketsAttachments(ticketId));
        }

        //Attachments for just PMs
        [Authorize]
        public ActionResult PMTicketAttachments()
        {
            var ticketAttachments = new List<TicketAttachment>(); //empty list of ticket attachments
            var projects = projectHelper.ListUserProjects(User.Identity.GetUserId()); //get projects this user is on
            foreach (var project in projects) //loop through each project in the projects user is on
            {
                foreach (var ticket in project.Tickets) //loop through the tickets of each individual project user is on
                {
                    foreach(var attachment in ticket.TicketAttachments) //loop through attachment of those tickets
                    {
                        ticketAttachments.Add(attachment); //add to empty attachment list
                    }
                }
            }
            return View("Index", ticketAttachments);
        }

        //Attachments for Devs and Subs
        [Authorize]
        public ActionResult DevSubTicketAttachments()
        {
            var ticketAttachments = new List<TicketAttachment>(); //empty list of ticket attachments
            var tickets = ticketHelper.ListUserTickets(User.Identity.GetUserId());
            foreach (var ticket in tickets) //loop through tickets user is assigned to
            {
                foreach (var attachment in ticket.TicketAttachments) //loop through attachment of those tickets
                {
                    ticketAttachments.Add(attachment); //add to empty attachment list
                }
            }
            return View("Index", ticketAttachments);
        }

        // GET: TicketAttachments/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            ViewBag.TicketId = id;
            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title");
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: TicketAttachments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TicketId,FileUrl,Description")] TicketAttachment ticketAttachment, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (ImageUploadValidator.IsWebFriendlyFile(file))
                {
                    var fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Path.Combine(Server.MapPath("~/MyImages/"), fileName));
                    ticketAttachment.FileUrl = "/MyImages/" + fileName;
                }

                ticketAttachment.UserId = User.Identity.GetUserId();
                ticketAttachment.Created = DateTime.Now;
                db.TicketAttachments.Add(ticketAttachment);
                db.SaveChanges();

                await ticketHelper.AddTicketNotification(ticketAttachment);
                ViewBag.PreviousUrl = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
                return Redirect(ViewBag.PreviousUrl);
            }

            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReturnUrl = Request.UrlReferrer;
            //ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,FileUrl,Description,Created,UserId")] TicketAttachment ticketAttachment, HttpPostedFileBase file, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUploadValidator.IsWebFriendlyFile(file))
                {
                    var fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Path.Combine(Server.MapPath("~/MyImages/"), fileName));
                    ticketAttachment.FileUrl = "/MyImages/" + fileName;
                }

                ticketAttachment.UserId = User.Identity.GetUserId();
                db.Entry(ticketAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(returnUrl);
            }
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketAttachment.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserId);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
        //    if (ticketAttachment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticketAttachment);
        //}

        // POST: TicketAttachments/Delete/5
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachment);
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
