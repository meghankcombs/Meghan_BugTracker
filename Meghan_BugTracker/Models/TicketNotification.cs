using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Meghan_BugTracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }
        [Display(Name = "Notification")]
        public int TicketId { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }

        //Navigation
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Recipient { get; set; }
    }
}