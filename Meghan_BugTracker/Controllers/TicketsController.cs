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
using System.Threading.Tasks;
using Meghan_BugTracker.ViewModels;

namespace Meghan_BugTracker.Controllers
{
    [RequireHttps]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketsHelper ticketHelper = new TicketsHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        // GET: Tickets
        [Authorize]
        public ActionResult Index() //Shows all tickets
        {
            //var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(db.Tickets.ToList());
        }

        [Authorize]
        public ActionResult MyIndex() //Shows all tickets by user
        {
            return View("Index", ticketHelper.ListUserTickets(User.Identity.GetUserId()));
        }

        [Authorize]
        public ActionResult SubmitterIndex() //Shows the submitter's tickets he created
        {
            return View("Index", ticketHelper.SubmitterTickets());
        }

        [Authorize]
        public ActionResult ProjectTickets(int projectId) //shows all tickets according to project
        {
            var project = db.Projects.Find(projectId);
            return View("Index", project.Tickets.ToList());
        }
        
        [Authorize]
        public ActionResult DevProjectTickets(int projectId) //show dev's tickets according to Project he/she assigned to
        {
            var project = db.Projects.Find(projectId);
            var userId = User.Identity.GetUserId();
            return View("Index", project.Tickets.Where(t => t.AssignedToUserId == userId).ToList());
        }

        [Authorize]
        public ActionResult PMProjectTickets()
        {
            var tickets = new List<Ticket>(); //empty list of tickets
            var projects = projectHelper.ListUserProjects(User.Identity.GetUserId()); //get projects this user is on
            foreach(var project in projects) //loop through each project in the projects user is on
            {
                foreach(var ticket in project.Tickets) //loop through the tickets of each individual project user is on
                {
                    tickets.Add(ticket); //add those tickets to the empty ticket list
                }
            }
            return View("Index", tickets);
        }

        // GET: Tickets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var commentVM = new TicketDetailsVM();

            Ticket ticket = db.Tickets.Find(id);

            if (ticket == null)
            {
                return HttpNotFound();
            }
            commentVM.Ticket = ticket;
            return View(commentVM);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            //As the  Submitter,  I can only create  Tickets for  Projects  I am on...
            var myProjects = projectHelper.ListUserProjects(User.Identity.GetUserId());
            if(myProjects.Count == 0)
            {
                //I cannot create a Ticket so redirect me somewhere else
                TempData["NoProjectMessage"] = "You're not assigned to any Projects.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName");
                ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
                ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name");
                ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
                ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
                ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
                return View();
            }     
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Description,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = DateTime.Now;
                var user = db.Users.Find(User.Identity.GetUserId());
                ticket.OwnerUserId = user.Id;
                ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(s => s.Name == "Unassigned").Id;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            //ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReturnUrl = Request.UrlReferrer;
            //Only Admins and PMs can do these actions
            if(User.IsInRole("Admin") || User.IsInRole("Project Manager"))
            {
                var developers = roleHelper.UsersInRole("Developer");
                ViewBag.AssignedToUserId = new SelectList(developers, "Id", "FirstName", ticket.AssignedToUserId);
                ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            }
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            //ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);

            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket, string returnUrl)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
            if (ModelState.IsValid)
            {
                ticket.Updated = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                //passing in this ticket you're about to edit BEFORE you edit and comparing it to the edited version after saving
                ticketHelper.AddTicketHistory(oldTicket, ticket);

                //pass relevant data into method
                await ticketHelper.AddTicketNotification(ticket.Id, oldTicket.AssignedToUserId, ticket.AssignedToUserId);

                return Redirect(returnUrl);
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Ticket ticket = db.Tickets.Find(id);
        //    if (ticket == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(ticket);
        //}

        // POST: Tickets/Delete/5
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
