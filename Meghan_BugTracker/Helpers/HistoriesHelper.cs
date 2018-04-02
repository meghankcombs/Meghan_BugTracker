using Meghan_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Meghan_BugTracker.Helpers
{
    public static class HistoriesHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static TicketHistory history = new TicketHistory();

        public static string HistoryValueName(int ticketHistoryId, bool oldValue)
        {
            //STILL NEEDS WORK
            var ticket = db.Tickets.Where(t => t.TicketHistories.Any(h => h.Id == ticketHistoryId)).FirstOrDefault();
            var value = ticket.GetType().GetProperty(ticket.TicketHistories.First(h => h.Id == ticketHistoryId).Property).GetValue(ticket);
            return value.ToString();
        }
    }
}