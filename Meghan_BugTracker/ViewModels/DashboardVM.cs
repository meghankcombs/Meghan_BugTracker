using Meghan_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Meghan_BugTracker.ViewModels
{
    public class DashboardVM
    {
        public List<Project> RecentProjects { get; set; }
        public List<Ticket> RecentTickets { get; set; }
        public List<ApplicationUser> AllUsers { get; set; }
        public List<TicketAttachment> RecentAttachments { get; set; }
        public List<TicketComment> RecentComments { get; set; }
        public List<TicketHistory> RecentHistories { get; set; }
        public List<TicketNotification> RecentNotifications { get; set; }

        public DashboardVM()
        {
            RecentProjects = new List<Project>();
            RecentTickets = new List<Ticket>();
            AllUsers = new List<ApplicationUser>();
            RecentAttachments = new List<TicketAttachment>();
            RecentComments = new List<TicketComment>();
            RecentHistories = new List<TicketHistory>();
            RecentNotifications = new List<TicketNotification>();
        }
    }
}