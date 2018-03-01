using Meghan_BugTracker.HelperModels;
using Meghan_BugTracker.Helpers;
using Meghan_BugTracker.Models;
using Meghan_BugTracker.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Meghan_BugTracker.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        private TicketsHelper ticketHelper = new TicketsHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        public ActionResult Index()
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

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            var emailBody = new StringBuilder();
            emailBody.AppendLine(model.Body);

            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email from: <bold>{0}</bold>({1})</p><p>Message:</p><p>{2}</p>";
                    var from = "Bug Tracker<meghankcombs@gmail.com>";
                    model.Body = emailBody.ToString();

                    var email = new MailMessage(from, ConfigurationManager.AppSettings["emailto"])
                    {
                        Subject = "Bug Tracker Email",
                        Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
                        IsBodyHtml = true
                    };
                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);
                    TempData["Message"] = "Your message has been sent!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }
    }
}