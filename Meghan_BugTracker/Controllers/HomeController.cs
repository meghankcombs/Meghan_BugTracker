using Meghan_BugTracker.HelperModels;
using Meghan_BugTracker.Helpers;
using Meghan_BugTracker.Models;
using Meghan_BugTracker.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Meghan_BugTracker.Controllers
{
    [RequireHttps]
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
                    dashboardData.RecentProjects = db.Projects.Take(5).ToList();
                    dashboardData.RecentTickets = db.Tickets.OrderByDescending(tn => tn.Created).Take(5).ToList();
                    dashboardData.AllUsers = db.Users.ToList();
                    dashboardData.RecentAttachments = db.TicketAttachments.OrderByDescending(tn => tn.Created).Take(5).ToList();
                    dashboardData.RecentComments = db.TicketComments.OrderByDescending(c => c.Created).Take(5).ToList();
                    dashboardData.RecentHistories = db.TicketHistories.OrderByDescending(h => h.ChangedDate).Take(5).ToList();
                    break;
                case "ProjectManager":
                    dashboardData.RecentProjects = projectHelper.ListUserProjects(userId).Take(5).ToList();
                    dashboardData.RecentTickets = ticketHelper.GetMyProjectTickets(userId).Take(5).ToList();
                    dashboardData.AllUsers = db.Users.ToList();
                    dashboardData.RecentAttachments = db.TicketAttachments.Where(t => t.UserId == userId).OrderByDescending(tn => tn.Created).Take(5).ToList();
                    dashboardData.RecentComments = db.TicketComments.Where(c => c.UserId == userId).OrderByDescending(com => com.Created).Take(5).ToList();
                    dashboardData.RecentHistories = db.TicketHistories.Where(h => h.UserId == userId).OrderByDescending(his => his.ChangedDate).Take(5).ToList();
                    break;
                case "Developer":
                    dashboardData.RecentProjects = projectHelper.ListUserProjects(userId).Take(5).ToList();
                    dashboardData.RecentTickets = db.Tickets.Where(t => t.AssignedToUserId == userId).Take(5).ToList();
                    dashboardData.AllUsers = db.Users.ToList();
                    dashboardData.RecentAttachments = db.TicketAttachments.Where(a => a.Ticket.AssignedToUserId == userId).OrderByDescending(att => att.Created).Take(5).ToList();
                    dashboardData.RecentComments = db.TicketComments.Where(c => c.Ticket.AssignedToUserId == userId).OrderByDescending(com => com.Created).Take(5).ToList();
                    dashboardData.RecentHistories = db.TicketHistories.Where(h => h.Ticket.AssignedToUserId == userId).OrderByDescending(his => his.ChangedDate).Take(5).ToList();
                    break;
                case "Submitter":
                    dashboardData.RecentProjects = projectHelper.ListUserProjects(userId).Take(5).ToList();
                    dashboardData.RecentTickets = db.Tickets.Where(t => t.OwnerUserId == userId).Take(5).ToList();
                    dashboardData.AllUsers = db.Users.ToList();
                    dashboardData.RecentAttachments = db.TicketAttachments.Where(a => a.Ticket.OwnerUserId == userId).OrderByDescending(att => att.Created).Take(5).ToList();
                    dashboardData.RecentComments = db.TicketComments.Where(c => c.Ticket.OwnerUserId == userId).OrderByDescending(com => com.Created).Take(5).ToList();
                    dashboardData.RecentHistories = db.TicketHistories.Where(h => h.Ticket.OwnerUserId == userId).OrderByDescending(his => his.ChangedDate).Take(5).ToList();
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

        //public FileContentResult UserPhotos()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        //let pass User.Identity into userId
        //        String userId = User.Identity.GetUserId();

        //        if (userId == null)
        //        {
        //            //if there is no photo chosen then use Stock photo- I am using CoderFoundry image
        //            string fileName = HttpContext.Server.MapPath(@"~\MyImages\user_default.png");
        //            //convert import image into byte file that can read by using FileStream and BinaryReader Method
        //            byte[] imageData = null;
        //            FileInfo fileInfo = new FileInfo(fileName);
        //            long imageFileLength = fileInfo.Length;
        //            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //            BinaryReader br = new BinaryReader(fs);
        //            imageData = br.ReadBytes((int)imageFileLength);

        //            return File(imageData, "image/png");

        //        }
        //        // to get the user details to load user Image 
        //        var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
        //        var UserImage = bdUsers.Users.Where(photo => photo.Id == userId).FirstOrDefault();

        //        return new FileContentResult(UserImage.UserPhoto, "image/jpeg");
        //    }
        //    else
        //    {
        //        string fileName = HttpContext.Server.MapPath(@"~\MyImages\user_default.png");

        //        byte[] imageData = null;
        //        FileInfo fileInfo = new FileInfo(fileName);
        //        long imageFileLength = fileInfo.Length;
        //        FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //        BinaryReader br = new BinaryReader(fs);
        //        imageData = br.ReadBytes((int)imageFileLength);
        //        return File(imageData, "image/png");

        //    }
        //}

        //public FileContentResult LoggedInUserPhoto(string userId)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        if (userId == null)
        //        {
        //            //if there is no photo chosen then use Stock photo
        //            string fileName = HttpContext.Server.MapPath(@"~\MyImages\user_default.png");
        //            //convert import image into byte file that can read by using FileStream and BinaryReader Method
        //            byte[] imageData = null;
        //            FileInfo fileInfo = new FileInfo(fileName);
        //            long imageFileLength = fileInfo.Length;
        //            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //            BinaryReader br = new BinaryReader(fs);
        //            imageData = br.ReadBytes((int)imageFileLength);

        //            return File(imageData, "image/png");

        //        }
        //        // to get the user details to load user Image 
        //        var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
        //        var UserImage = bdUsers.Users.Where(photo => photo.Id == userId).FirstOrDefault();

        //        if (UserImage.UserPhoto == null)
        //        {
        //            //if there is no photo chosen then use Stock photo
        //            string fileName = HttpContext.Server.MapPath(@"~\MyImages\user_default.png");
        //            //convert import image into byte file that can read by using FileStream and BinaryReader Method
        //            byte[] imageData = null;
        //            FileInfo fileInfo = new FileInfo(fileName);
        //            long imageFileLength = fileInfo.Length;
        //            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //            BinaryReader br = new BinaryReader(fs);
        //            imageData = br.ReadBytes((int)imageFileLength);

        //            return File(imageData, "image/png");
        //        }
        //        else
        //        {
        //            return new FileContentResult(UserImage.UserPhoto, "image/jpeg");
        //        }
        //    }
        //    else
        //    {
        //        string fileName = HttpContext.Server.MapPath(@"~\MyImages\user_default.png");

        //        byte[] imageData = null;
        //        FileInfo fileInfo = new FileInfo(fileName);
        //        long imageFileLength = fileInfo.Length;
        //        FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //        BinaryReader br = new BinaryReader(fs);
        //        imageData = br.ReadBytes((int)imageFileLength);
        //        return File(imageData, "image/png");

        //    }
        //}
    }
}