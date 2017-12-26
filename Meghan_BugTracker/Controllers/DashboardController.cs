using Meghan_BugTracker.Helpers;
using Meghan_BugTracker.Models;
using Meghan_BugTracker.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Meghan_BugTracker.Controllers
{
    public class DashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        private TicketsHelper ticketHelper = new TicketsHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public ActionResult BasicDashboard()
        {
            var dashboardData = new DashboardVM();
            var userId = User.Identity.GetUserId();
            var myRole = "Guest";
            if (userId != null)
                myRole = roleHelper.ListUserRoles(User.Identity.GetUserId()).FirstOrDefault();

            switch (myRole)
            {
                case "Admin":
                    dashboardData.RecentProjects = db.Projects.ToList();
                    dashboardData.RecentTickets = db.Tickets.OrderByDescending(tn => tn.Created).Take(5).ToList();
                    dashboardData.RecentNotifications = db.TicketNotifications.OrderByDescending(tn => tn.Created).Take(5).ToList();
                    dashboardData.RecentHistories = db.TicketHistories.OrderByDescending(th => th.ChangedDate).Take(5).ToList();

                    break;
                case "ProjectManager":
                    dashboardData.RecentProjects = projectHelper.ListUserProjects(userId).ToList();
                    dashboardData.RecentTickets = ticketHelper.GetMyProjectTickets(userId);
                    break;
                case "Developer":
                    dashboardData.RecentProjects = projectHelper.ListUserProjects(userId).ToList();
                    dashboardData.RecentTickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                    break;
                case "Submitter":
                    dashboardData.RecentTickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                    break;
                default:
                    ViewBag.Message = "You will not be able to see any data until you are assigned to a role.";
                    break;
            }
            return View(dashboardData);
        }
    }
}
