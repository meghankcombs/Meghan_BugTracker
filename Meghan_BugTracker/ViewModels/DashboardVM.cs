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
        public List<TicketNotification> RecentNotifications { get; set; }
        public List<TicketHistory> RecentHistories { get; set; }

        public DashboardVM()
        {
            RecentProjects = new List<Project>();
            RecentTickets = new List<Ticket>();
            RecentNotifications = new List<TicketNotification>();
            RecentHistories = new List<TicketHistory>();
        }
    }
}