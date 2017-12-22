using Meghan_BugTracker.Helpers;
using Meghan_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Meghan_BugTracker.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projHelper = new ProjectsHelper();
        private TicketsHelper ticketHelper = new TicketsHelper();

        // GET: Admin/UserIndex
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult UserIndex()
        {
            return View(db.Users.ToList());
        }

        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult IndexOfSubs()
        {            
            return View("UserIndex", roleHelper.UsersInRole("Submitter"));
        }

        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult IndexOfDevs()
        {
            return View("UserIndex", roleHelper.UsersInRole("Developer"));
        }

        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult IndexOfPMs()
        {
            return View("UserIndex", roleHelper.UsersInRole("Project Manager"));
        }

        // GET: ProjectAssign
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult ProjectAssign(string id)
        {
            ViewBag.UserId = id;
            var occupiedProjects = projHelper.ListUserProjects(id).Select(p => p.Id);
            ViewBag.AllProjects = new MultiSelectList(db.Projects, "Id", "Name", occupiedProjects);
            return View();
        }

        //POST: ProjectAssign
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectAssign(string userId, List<int> AllProjects)
        {
            //Remove user from all projects
            foreach(var proj in projHelper.ListUserProjects(userId))
            {
                projHelper.RemoveUserFromProject(userId, proj.Id);//need to specify Id here because passing in entire project in foreach loop
            }

            //Add user to projects
            if(AllProjects != null)
            {
                foreach (var projId in AllProjects) //already using Id's here for variable projId so don't need to denote proj.Id below
                {
                    projHelper.AddUserToProject(userId, projId);
                }
            }

            return RedirectToAction("UserIndex");
        }

        // GET: TicketAssign
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult TicketAssign(string id)
        {
            ViewBag.UserId = id;
            var occupiedTickets = ticketHelper.ListUserTickets(id).Select(p => p.Id);
            var currentProjTix = ticketHelper.GetMyProjectTickets(id);
            ViewBag.AllTickets = new MultiSelectList(currentProjTix, "Id", "Title", occupiedTickets);
            return View();
        }

        //POST: ProjectAssign
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TicketAssign(string userId, List<int> AllTickets)
        {
            //Remove user from all tickets
            foreach (var ticket in ticketHelper.ListUserTickets(userId))
            {
                ticketHelper.RemoveUserFromTicket(userId, ticket.Id);//need to specify Id here because passing in entire ticket in foreach loop
            }

            //Add user to projects
            if (AllTickets != null)
            {
                foreach (var ticketId in AllTickets) //already using Id's here for variable projId so don't need to denote proj.Id below
                {
                    ticketHelper.AddUserToTicket(userId, ticketId);
                    await ticketHelper.AddTicketNotification(ticketId, "", userId);
                }
            }
            return RedirectToAction("UserIndex");
        }

        // GET: RoleAssign
        [Authorize(Roles = "Admin")]
        public ActionResult RoleAssign(string id)
        {
            ViewBag.UserId = id;
            var occupiedRole = roleHelper.ListUserRoles(id).FirstOrDefault(); //show role user is currently occupying
            var mostRoles = db.Roles.Where(r => r.Name != "Admin"); //show every role except Admin
            ViewBag.AllRoles = new SelectList(mostRoles, "Name", "Name", occupiedRole);
            //"Name" coming from Seed method and AspNetRoles property "Name"
            return View();
        }

        //POST: RoleAssign
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAssign(string userId, string AllRoles)
        {
            if (String.IsNullOrEmpty(userId))
            {
                return RedirectToAction("RoleAssign", new { id = userId });
            }

            //Go through occupied roles and remove user from all
            foreach (var role in roleHelper.ListUserRoles(userId))
            {
                roleHelper.RemoveUserFromRole(userId, role);
            }

            if(!String.IsNullOrEmpty(AllRoles))
            {
                roleHelper.AddUserToRole(userId, AllRoles);
            }

            return RedirectToAction("UserIndex");
        }

        //GET & POST REMOVE ROLE
        [Authorize(Roles = "Admin")]
        public ActionResult RemoveRole(string userId)
        {
            var roles = roleHelper.ListUserRoles(userId);
            foreach(var role in roles)
            {
                roleHelper.RemoveUserFromRole(userId, role);
            }
            return RedirectToAction("UserIndex");
        }
        
        //SCAFFOLDED ACTIONS:
        #region
        // GET: Admin/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Admin/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Admin/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: Admin/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Admin/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Admin/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        #endregion
    }
}
