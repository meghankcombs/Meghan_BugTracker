using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Meghan_BugTracker.Models
{
    public class TicketPriority
    {
        public int Id { get; set; }
        [Display(Name="Priority")]
        public string Name { get; set; }

        //Navigation
        //Child of TicketPriority
        public virtual ICollection<Ticket> Tickets { get; set; }

        public TicketPriority()
        {
            this.Tickets = new HashSet<Ticket>();
        }
    }
}